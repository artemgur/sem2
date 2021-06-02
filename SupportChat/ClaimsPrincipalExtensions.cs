using System.Security.Claims;
using Authentication.Infrastructure;
using sem2_FSharp;

namespace SupportChat
{
    public static class ClaimsPrincipalExtensions
    {
        //TODO Артур
        public static bool HasSupportClaim(this ClaimsPrincipal user) => user.GetId() == 3;
    }
}