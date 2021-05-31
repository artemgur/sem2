using System.Security.Claims;
using DomainModels;

namespace SupportChat
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool HasSupportClaim(this ClaimsPrincipal user) => user.HasClaim("support", "support");
    }
}