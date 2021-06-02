using Authentication.Infrastructure;
using Microsoft.AspNetCore.SignalR;

namespace SupportChat
{
    //Needed for correct identification of users by SignalR
    //https://docs.microsoft.com/en-us/aspnet/core/signalr/authn-and-authz?view=aspnetcore-5.0#use-claims-to-customize-identity-handling
    //https://docs.microsoft.com/en-us/aspnet/core/signalr/groups?view=aspnetcore-5.0
    public class UserIdProvider : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.GetId().ToString();
        }
    }
}