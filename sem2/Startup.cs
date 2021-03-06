using System;
using Authentication;
using Authorization;
using Authorization.Models;
using Authorization.Services;
using DomainModels;
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
                    opts.AddRole("manager");
                    opts.AddRole("support");
                    
                    opts.AddUser(userBuilder =>
                    {
                        userBuilder.Email = "support@itis.sem2.ru";
                        userBuilder.Password = "Qwerty1.";
                        userBuilder.AdditionalInfo["FirstName"] = "Artur";
                        userBuilder.AdditionalInfo["Surname"] = "Zagitov";
                        userBuilder.AddRole("support");
                    });


                    opts.AddUser(userBuilder =>
                    {
                        userBuilder.Email = "armgnv@gmail.com";
                        userBuilder.Password = "Aaaa1!";
                        userBuilder.AdditionalInfo["FirstName"] = "Artem";
                        userBuilder.AdditionalInfo["Surname"] = "Gurianov";
                        userBuilder.AddRole("support");
                    });
                    
                    opts.AddUser(userBuilder =>
                    {
                        userBuilder.Email = "admin@itis.sem2.ru";
                        userBuilder.Password = "Qwerty1.";
                        userBuilder.AdditionalInfo["FirstName"] = "Artur";
                        userBuilder.AdditionalInfo["Surname"] = "Zagitov";

                        userBuilder.AddRole("admin");
                    });
                },
                async (id, userInfo, serviceProvider) =>
                {
                    var dbContext = serviceProvider.GetService<ApplicationContext>();
                    string firstName = "";
                    if (userInfo.AdditionalInfo.ContainsKey("FirstName"))
                        firstName = userInfo.AdditionalInfo["FirstName"];
                    
                    string surname = "";
                    if (userInfo.AdditionalInfo.ContainsKey("Surname"))
                        surname = userInfo.AdditionalInfo["Surname"];
                    var user = new User()
                    {
                        Id = id,
                        FirstName = firstName,
                        Surname = surname,
                        Email = userInfo.Email
                    };
                    
                    var image = DomainModels.User.DefaultImage;
                    dbContext.ImageMetadata.Add(image);
                    await dbContext.SaveChangesAsync();
        
                    user.Image = image;
                    dbContext.Users.Add(user);
                    await dbContext.SaveChangesAsync();
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
                opts.AddPolicy("HasAdminPanelAccess", policy =>
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

            services.AddWebOptimizer(); //Minification https://github.com/ligershark/WebOptimizer
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
            
            app.UseWebOptimizer(); //Minification
            
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "AdminPanel",
                    pattern: "{controller=AdminPanel}/{action=Products}/{id?}");
                endpoints.MapHub<ChatHub>("/chat");
            });
        }
    }
}