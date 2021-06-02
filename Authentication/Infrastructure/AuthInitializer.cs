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
                await AuthInitializer.InitializeUsersAsync(userManager, rolesManager, options.Users.ToList(), options.DefaultRole, services);
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
                if (!(await roleManager.RoleExistsAsync(role)))
                    await roleManager.CreateAsync(new IdentityRole<int>(role));
            }
        }

        public static async Task InitializeUsersAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<int>> roleManager,
            List<UserBuilder> userBuilders,
            string defaultRole,
            IServiceProvider serviceProvider)
        {
            foreach (var userBuilder in userBuilders)
            {
                var user = await userManager.FindByEmailAsync(userBuilder.Email);
                if (user == null)
                {
                    user = new ApplicationUser()
                    {
                        UserName = userBuilder.Email,
                        Email = userBuilder.Email,
                        EmailConfirmed = true
                    };
                    
                    await userManager.CreateAsync(user, userBuilder.Password);
                    
                    if(defaultRole != null)
                        await userManager.AddToRoleAsync(user, defaultRole);
                    
                    await userBuilder.ExternalBuild(user.Id, serviceProvider);
                }

                foreach (var role in userBuilder.Roles)
                {
                    if(!(await userManager.IsInRoleAsync(user, role)))
                        await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}