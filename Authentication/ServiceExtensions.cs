using System;
using System.Threading.Tasks;
using Authentication.Infrastructure;
using Authentication.Models;
using Authentication.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddAuthenticationServices(
            this IServiceCollection services,
            Action<AuthenticationServiceOptions> optionsBuilder, 
            Func<int, UserBuilder, IServiceProvider, Task> buildHandler = null)
        {
            var options = new AuthenticationServiceOptions();
            optionsBuilder(options);
            if(buildHandler != null)
                foreach(var userBuilder in options.Users)
                    userBuilder.ContinueWith(buildHandler);

            services.AddSingleton(options);

            services.AddDbContext<UserContext>(opt =>
                opt.UseNpgsql(options.ConnectionString));

            services.AddIdentity<ApplicationUser, IdentityRole<int>>(opts =>
                {
                    opts.User.RequireUniqueEmail = true;
                    opts.SignIn.RequireConfirmedEmail = true;
                })
                .AddRoles<IdentityRole<int>>()
                .AddRoleManager<RoleManager<IdentityRole<int>>>()
                .AddEntityFrameworkStores<UserContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<AuthenticationService>();
            services.AddScoped<RolesService>();
            services.AddScoped<UserManager>();

            return services;
        }
    }
}