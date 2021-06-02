using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Authentication.Infrastructure;
using Authentication.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Services
{
    public class AuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly UserManager _user;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly AuthenticationServiceOptions _options;

        public AuthenticationService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AuthenticationServiceOptions options, RoleManager<IdentityRole<int>> roleManager, UserManager user)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _options = options;
            _roleManager = roleManager;
            _user = user;
        }

        public AuthenticationProperties GetExternalAuthenticationProperties(string provider, string redirectUri)
        {
            return _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUri);
        }

        public async Task<ExternalLoginResult> ExternalLogin()
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if(info == null)
                return ExternalLoginResult.Failed(new IdentityError()
                {
                    Code = "InfoNotPresent",
                    Description = "Не удалось получить аунтификационные данные"
                });

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            if (result.Succeeded && _user.GetUserId().TryGetValue(out var userId))
                return ExternalLoginResult.Success(userId, email);

            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                await _signInManager.SignInAsync(existingUser, false);
                return ExternalLoginResult.Success(existingUser.Id, existingUser.Email);
            }
            
            var newUser = new ApplicationUser()
            {
                Email = email,
                UserName = email,
                EmailConfirmed = true
            };

            var createResult = await _userManager.CreateAsync(newUser);
            if (!createResult.Succeeded)
                return ExternalLoginResult.Failed(createResult);

            var idResult = await _userManager.AddLoginAsync(newUser, info);
            if (!idResult.Succeeded)
                return ExternalLoginResult.Failed(idResult);;

            await _signInManager.SignInAsync(newUser, false);
            return ExternalLoginResult.Success(newUser.Id, newUser.Email);
        }

        public async Task<Result<string>> GeneratePasswordResetTokenAsync(string email)
        { 
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                return Result.Failure<string>("Пользователь не найден");
            
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            return Result.Success(code);
        }

        public async Task<Result<string>> GenerateEmailConfimationToken(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if(user == null)
                return Result.Failure<string>("Пользователь не найден");

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return Result.Success(code);
        }

        public async Task<IdentityResult> ConfirmEmail(int userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if(user == null)
                return IdentityResult.Failed(new IdentityError()
                {
                    Code = "UserNotFound",
                    Description = "Пользователь не найден"
                });

            return await _userManager.ConfirmEmailAsync(user, code);
        }
        
        public async Task<IdentityResult> ConfirmPasswordChange(string email, string code, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
                return IdentityResult.Failed(new IdentityError()
                {
                    Code = "UserNotFound",
                    Description = "Пользователь не найден"
                });

            return await _userManager.ResetPasswordAsync(user, code, newPassword);
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<(IdentityResult result, int id)> RegisterUser(RegisterModel model)
        {
            var user = new ApplicationUser()
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            var id = user.Id;
            if(result.Succeeded)
                if(_options.DefaultRole != null)
                    result = await _userManager.AddToRolesAsync(user, new []{_options.DefaultRole});
            return (result, id);
        }

        public async Task<SignInResult> Login(LoginModel model)
        {
            var result = await _signInManager
                .PasswordSignInAsync(model.Email,
                    model.Password,
                    model.RememberMe,
                    false);

            return result;
        }

        /*public async Task<IdentityResult> EditUser(EditModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if(user == null)
                return IdentityResult.Failed(new IdentityError()
                {
                    Code = "UserNotFound",
                    Description = "Пользователь не найден"
                });

            user.UserName = model.NewUserName;

            var result = await _userManager.UpdateAsync(user);
            return result;
        }*/

        public async Task<IdentityResult> DeleteUser(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if(user == null)
                return IdentityResult.Failed(new IdentityError()
                {
                    Code = "UserNotFound",
                    Description = "Пользователь не найден"
                });

            return await _userManager.DeleteAsync(user);
        }
    }
}