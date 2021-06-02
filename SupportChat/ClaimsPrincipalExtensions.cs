using System.Security.Claims;
using sem2_FSharp;

namespace SupportChat
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool HasSupportClaim(this ClaimsPrincipal user) => user.IsInRole("support");
    }
}