using Microsoft.AspNetCore.Identity;

namespace Authentication.Models
{
    public class ExternalLoginResult
    {
        public IdentityResult IdentityResult;
        public bool Succeeded => IdentityResult.Succeeded;
        public int UserId;
        public string UserEmail;

        public static ExternalLoginResult Failed(IdentityError error)
        {
            return new ExternalLoginResult()
            {
                IdentityResult = IdentityResult.Failed(error)
            };
        }
        
        public static ExternalLoginResult Failed(IdentityResult result)
        {
            return new ExternalLoginResult()
            {
                IdentityResult = result
            };
        }

        public static ExternalLoginResult Success(int userId, string userEmail)
        {
            return new ExternalLoginResult()
            {
                IdentityResult = IdentityResult.Success,
                UserId = userId,
                UserEmail = userEmail
            };
        }
    }
}