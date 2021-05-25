using System.Threading.Tasks;
using Authentication.Infrastructure;
using Authentication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Services
{
    public class UserManager
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly AuthenticationServiceOptions _options;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private User _user;

        public UserManager(UserManager<User> userManager, SignInManager<User> signInManager, AuthenticationServiceOptions options, RoleManager<IdentityRole<int>> roleManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _options = options;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
        }

        private async Task<User> GetUser()
        {
            if (_user == null)
                _user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            return _user;
        }

        public async Task<bool> HasRole(string role, User user = null)
        {
            return await _userManager.IsInRoleAsync(user ?? await GetUser(), role);
        }

        public async Task<Result<string>> GetEmail(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if(user == null)
                return Result.Failure<string>("Пользователь не найден");
            
            return Result.Success(user.Email);
        }

        public async Task<Result<string>> GetEmail()
        {
            if(!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                return Result.Failure<string>("Пользователь не аунтифицирован");

            var user = await GetUser();
            return Result.Success<string>(user.Email);
        }
    }
}