using AOP;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace SupportChat
{
    public static class ChatServicesExtensions
    {
        public static void AddSupportChat(this IServiceCollection services)
        {
            services.AddSingleton<IUserIdProvider, UserIdProvider>();
            services.AddScoped<IChatDatabase>(sp =>
                AdviceCreator.Create<IChatDatabase, ChatDatabaseAdvice>(new MongoDB()));
            services.AddSignalR();
        }
    }
}