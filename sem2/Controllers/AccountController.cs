using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authentication.Services;
using Authorization.Services;
using DomainModels;
using Konscious.Security.Cryptography;
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
                    var success = await _sender.SendEmailAsync(email, "Сброс пароля",
                        $"Перейдите по ссылке для сброса пароля: \n {Url.Action("NewPassword", "Account", null, Request.Scheme)}?key={result.Value}&email={email}");
                    if (!success)
                        ModelState.AddModelError("", $"Письмо не может быть отправлено, т.к оно заблокированно по подозрению в спаме.\n {Url.Action("NewPassword", "Account", null, Request.Scheme)}?key={result.Value}&email={email}");
                }
                return View("ResetPasswordEmail");
            }
            return View();
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
            if (ModelState.IsValid)
            {
                var result = await _authenticationService.ConfirmPasswordChange(passwordResetModel.Email,
                    passwordResetModel.Key, passwordResetModel.NewPassword);
                if(result.Succeeded)
                    return View("PasswordChangeSuccessfull");

                ModelState.AddIdentityErrors(result.Errors);
                return View();
            }
            return View();
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
                    var user = new User()
                    {
                        Id = res.id,
                        FirstName = model.FirstName,
                        Surname = model.Surname
                    };
                    
                    var image = DomainModels.User.DefaultImage;
                    _dbContext.ImageMetadata.Add(image);
                    await _dbContext.SaveChangesAsync();
        
                    user.Image = image;
                    _dbContext.Users.Add(user);
                    await _dbContext.SaveChangesAsync();
        
                    return Redirect($"{Url.Action("ConfirmEmail")}?userId={user.Id}");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
        
            return View(model);
        }
        
        public async Task<IActionResult> ConfirmEmail(int userId)
        {
            var result = await _authenticationService.GenerateEmailConfimationToken(userId);
            if (!result.IsSuccessful)
                return RedirectToAction("Register");

            var key = result.Value;
            var email = await _userManager.GetEmail(userId);
            var success = await _sender.SendEmailAsync(email.Value, "Подтверждение Email",
                $"Перейдите по ссылке для окончания регистрации: \n {Url.Action("EmailConfirmationEnd", "Account", null, Request.Scheme)}?key={key}&userId={userId}");
            if (!success)
                ModelState.AddModelError("", $"Письмо не может быть отправлено, т.к оно заблокированно по подозрению в спаме.\n {Url.Action("EmailConfirmationEnd", "Account", null, Request.Scheme)}?key={key}&userId={userId}");
            
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