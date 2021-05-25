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
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly AuthenticationServiceOptions _options;

        public AuthenticationService(UserManager<User> userManager, SignInManager<User> signInManager, AuthenticationServiceOptions options, RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _options = options;
            _roleManager = roleManager;
        }

        public AuthenticationProperties GetExternalAuthenticationProperties(string provider, string redirectUri)
        {
            return _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUri);
        }

        public async Task<IdentityResult> ExternalLogin()
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if(info == null)
                return IdentityResult.Failed(new IdentityError()
                {
                    Code = "InfoNotPresent",
                    Description = "Не удалось получить аунтификационные данные"
                });

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            if (result.Succeeded)
                return IdentityResult.Success;
            
            var newUser = new User()
            {
                Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                EmailConfirmed = true
            };

            var createResult = await _userManager.CreateAsync(newUser);
            if (!createResult.Succeeded)
                return createResult;

            var idResult = await _userManager.AddLoginAsync(newUser, info);
            if (!idResult.Succeeded)
                return idResult;

            await _signInManager.SignInAsync(newUser, false);
            return IdentityResult.Success;
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
            var user = new User()
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