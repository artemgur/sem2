using System.Security.Claims;
using System.Threading.Tasks;
using Authentication.Infrastructure;
using Authentication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Services
{
    public class UserManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly AuthenticationServiceOptions _options;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ApplicationUser _applicationUser;

        public UserManager(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AuthenticationServiceOptions options, RoleManager<IdentityRole<int>> roleManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _options = options;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
        }

        private async Task<ApplicationUser> GetUser()
        {
            if (_applicationUser == null)
                _applicationUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            return _applicationUser;
        }
        
        

        public async Task<bool> HasRole(string role, ApplicationUser applicationUser = null)
        {
            return await _userManager.IsInRoleAsync(applicationUser ?? await GetUser(), role);
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

        public Result<int> GetUserId()
        {
            var userPrincipal = _httpContextAccessor.HttpContext.User;
            if(userPrincipal.Identity.IsAuthenticated ||
               !int.TryParse(userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier), out var res))
                return Result.Failure<int>("Пользователь не аунтифицирован");

            return Result.Success(res);
        }
    }
}