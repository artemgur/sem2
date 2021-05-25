using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authentication.Models;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Services
{
    public class RolesService
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly UserManager<User> _userManager;

        public RolesService(RoleManager<IdentityRole<int>> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IdentityResult> CreateRole(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return IdentityResult.Failed(new IdentityError()
            {
                Code = "NameIsEmpty",
                Description = "Название роли не задано"
            });

            return await _roleManager.CreateAsync(new IdentityRole<int>(name));
        }

        public async Task<IdentityResult> DeleteRole(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if(role == null)
                return IdentityResult.Failed(new IdentityError()
                {
                    Code = "RoleNotFound",
                    Description = "Роль не найдена"
                });

            return await _roleManager.DeleteAsync(role);
        }

        public async Task<IdentityResult> EditUserRoles(int userId, List<string> roles)
        {
            User user = await _userManager.FindByIdAsync(userId.ToString());
            if(user == null)
                return IdentityResult.Failed(new IdentityError()
                {
                    Code = "UserNotFound",
                    Description = "Пользователь не найден"
                });
            
            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.ToList();
            var addedRoles = roles.Except(userRoles);
            var removedRoles = userRoles.Except(roles);
 
            var result = await _userManager.AddToRolesAsync(user, addedRoles);
            if (!result.Succeeded)
                return result;
            result = await _userManager.RemoveFromRolesAsync(user, removedRoles);
            return result;
        }
    }
}