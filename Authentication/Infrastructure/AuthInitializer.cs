using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authentication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Authentication.Infrastructure
{
    public class AuthInitializer
    {
        private AuthInitializer(){}
        public static async Task InitializeAsync(IServiceProvider services)
        {
            try
            {
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var rolesManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
                var options = services.GetRequiredService<AuthenticationServiceOptions>();

                await AuthInitializer.InitializeRolesAsync(userManager, rolesManager, options.Roles.ToList());
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<AuthInitializer>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }
        
        public static async Task InitializeRolesAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<int>> roleManager,
            List<string> roles)
        {
            foreach (var role in roles)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                    await roleManager.CreateAsync(new IdentityRole<int>(role));
            }
        }
    }
}