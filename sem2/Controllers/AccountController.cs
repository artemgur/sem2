using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Authentication.Services;
using Authorization.Services;
using DomainModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using sem2.Models.ViewModels;
using sem2.Models.ViewModels.AccountModels;

namespace sem2.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationContext _dbContext;
        private readonly ILogger<AccountController> _logger;
        private readonly IEmailSender _sender;
        private readonly AuthenticationService _authenticationService;
        private readonly UserManager _userManager;

        public AccountController(ILogger<AccountController> logger, ApplicationContext dbContext,
            IEmailSender sender,
            AuthenticationService authenticationService, UserManager userManager)
        {
            _logger = logger;
            _dbContext = dbContext;
            _sender = sender;
            _authenticationService = authenticationService;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        public IActionResult GoogleLogin()
        {
            string redirectUri = Url.Action("GoogleResponse");
            var properties = _authenticationService.GetExternalAuthenticationProperties("Google", redirectUri);
            return new ChallengeResult("Google", properties);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await _authenticationService.ExternalLogin();
            if (!result.Succeeded)
                return RedirectToAction("Login");

            var user = _dbContext.Users.FirstOrDefault(u => u.Id == result.UserId);
            if (user == null)
                await CreateUser(result.UserEmail, result.UserId);
            return RedirectToAction("Profile", "Profile");
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(EmailModel emailModel)
        {
            if (ModelState.IsValid)
            {
                var email = emailModel.Email;
                var result = await _authenticationService.GeneratePasswordResetTokenAsync(email);
                
                if (!result.IsSuccessful)
                    ModelState.AddModelError("", $"Пользователя с Email: {email} не существует");
                else
                {
                    string keyHtmlVersion = HttpUtility.UrlEncode(result.Value);
                    var success = await _sender.SendEmailAsync(email, "Сброс пароля",
                        $"Перейдите по ссылке для сброса пароля: \n {Url.Action("NewPassword", "Account", null, Request.Scheme)}?key={keyHtmlVersion}&email={email}");
                    if (!success)
                        ModelState.AddModelError("", $"Письмо не может быть отправлено, т.к оно заблокированно по подозрению в спаме.\n {Url.Action("NewPassword", "Account", null, Request.Scheme)}?key={keyHtmlVersion}&email={email}");
                }
                return View("ResetPasswordEmail", email);
            }
            return View(new EmailModel());
        }

        [HttpGet]
        public async Task<IActionResult> NewPassword(string key, string email)
        {
            return View(new PasswordResetModel
            {
                Email = email,
                Key = key
            });
        }

        [HttpPost]
        public async Task<IActionResult> NewPassword(PasswordResetModel passwordResetModel)
        {
            var model = new PasswordResetModel
            {
                Email = passwordResetModel.Email,
                Key = passwordResetModel.Key
            };
            if (ModelState.IsValid)
            {
                var result = await _authenticationService.ConfirmPasswordChange(passwordResetModel.Email,
                    passwordResetModel.Key, passwordResetModel.NewPassword);
                if(result.Succeeded)
                    return View("PasswordChangeSuccessfull");

                ModelState.AddIdentityErrors(result.Errors);
                return View(model);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authenticationService.Login(new Authentication.Models.LoginModel()
                {
                    Email = model.Email,
                    Password = model.Password,
                    RememberMe = model.RememberMe != null
                });
                
                if(result.Succeeded)
                    return RedirectToAction("Index", "Home");
                
                if(result.RequiresTwoFactor)
                    ModelState.AddModelError("", "Требуется подтверждение Email адреса");
                else
                    ModelState.AddModelError("", "Неверный логин или пароль");
            }
        
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var res = await _authenticationService.RegisterUser(new Authentication.Models.RegisterModel()
                {
                    Email = model.Email,
                    Password = model.Password
                });
                
                if (res.result.Succeeded)
                {
                    await CreateUser(model.Email, res.id, model.FirstName, model.Surname);
                    return Redirect($"{Url.Action("ConfirmEmail")}?userId={res.id}");
                }
                else
                    ModelState.AddIdentityErrors(res.result.Errors);
            }
        
            return View(model);
        }

        private async Task CreateUser(string email, int id, string firstName = "", string surName = "")
        {
            var user = new User()
            {
                Id = id,
                FirstName = firstName,
                Surname = surName,
                Email = email
            };
                    
            var image = DomainModels.User.DefaultImage;
            _dbContext.ImageMetadata.Add(image);
            await _dbContext.SaveChangesAsync();
        
            user.Image = image;
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }
        
        public async Task<IActionResult> ConfirmEmail(int userId)
        {
            var result = await _authenticationService.GenerateEmailConfimationToken(userId);
            if (!result.IsSuccessful)
                return RedirectToAction("Register");

            var key = result.Value;
            string keyHtmlVersion = HttpUtility.UrlEncode(key);
            var email = await _userManager.GetEmail(userId);
            var success = await _sender.SendEmailAsync(email.Value, "Подтверждение Email",
                $"Перейдите по ссылке для окончания регистрации: \n {Url.Action("EmailConfirmationEnd", "Account", null, Request.Scheme)}?key={keyHtmlVersion}&userId={userId}");
            if (!success)
                ModelState.AddModelError("", $"Письмо не может быть отправлено, т.к оно заблокированно по подозрению в спаме.\n {Url.Action("EmailConfirmationEnd", "Account", null, Request.Scheme)}?key={keyHtmlVersion}&userId={userId}");
            
            return View(model: email.Value);
        }
        
        public async Task<IActionResult> EmailConfirmationEnd(string key, int userId)
        {
            var result = await _authenticationService.ConfirmEmail(userId, key);
            if(!result.Succeeded)
                ModelState.AddIdentityErrors(result.Errors);
        
            return View(userId);
        }
        
        public async Task<IActionResult> Logout()
        {
            await _authenticationService.Logout();
            return RedirectToAction("Login");
        }
    }
}