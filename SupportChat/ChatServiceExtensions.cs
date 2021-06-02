using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace SupportChat
{
    public static class ChatServicesExtensions
    {
        public static void AddSupportChat(this IServiceCollection services)
        {
            services.AddSingleton<IUserIdProvider, UserIdProvider>();
            services.AddScoped<IChatDatabase, MongoDB>();
            services.AddSignalR();
        }
    }
}