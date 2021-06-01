using System.Security.Claims;

namespace Authentication.Infrastructure
{
    public static class PrincipalExtensions
    {
        public static int GetId(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated 
                || !int.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
                return -1;

            return userId;
        }
    }
}