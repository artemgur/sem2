using System;
using Authentication;
using Authorization;
using Authorization.Models;
using Authorization.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using sem2.Infrastructure.Services;
using sem2_FSharp;
using SupportChat;

// using sem2_FSharp.Middleware;

namespace sem2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            var settings = Configuration.GetSection("EmailSettings").Get<EmailSettings>();
            services.AddSingleton<IEmailSender, EmailService>();
            services.AddSingleton(settings);
            
            services.AddAuthenticationServices(opts =>
            {
                opts.ConnectionString = Configuration.GetConnectionString("HerokuUsers");
                opts.DefaultRole = "user";
                opts.AddRole("user");
                opts.AddRole("admin");
                opts.AddRole("support");
            });
            
            services.AddAuthentication()
                .AddGoogle(opts =>
                {
                    IConfigurationSection googleAuthNSection =
                        Configuration.GetSection("Authentication:Google");

                    opts.ClientId = googleAuthNSection["ClientId"];
                    opts.ClientSecret = googleAuthNSection["ClientSecret"];
                    opts.SignInScheme = IdentityConstants.ExternalScheme;
                });
            
            var connectionString = Configuration.GetConnectionString("Heroku");
            services.AddDbContext<ApplicationContext>(option =>
                option.UseNpgsql(connectionString, b => b.MigrationsAssembly("sem2")));

            services.AddSingleton<CommandService>();
            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("HasEditPermission", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("manager", "admin");
                });
                opts.AddPolicy("NotBanned", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.AddRequirements();
                });
            });

            services.AddScoped<PermissionService>();
            services.AddScoped<IPaymentService, FakePaymentService>();
            services.AddControllersWithViews();
            services.AddHttpContextAccessor();

            services.AddSupportChat();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Catalog}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "AdminPanel",
                    pattern: "{controller=AdminPanel}/{action=Products}/{id?}");
                endpoints.MapHub<ChatHub>("/chat");
            });
        }
    }
}