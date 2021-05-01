using System;
using System.Text;
using PinnedMemory;

namespace DomainModels
{
    public class PasswordHasher
    {
        private static readonly Random random = new Random();

        private const int SaltLength = 16;
        
        public static string HashPassword(string password, byte[] salt)
        {
            var passwordPinnedMemory = new PinnedMemory<byte>(Encoding.UTF8.GetBytes(password));
            var argon2 = new Argon2.NetCore.Argon2(passwordPinnedMemory, salt);
            return argon2.ToString();
        }

        public static byte[] GenerateSalt()
        {
            var result = new byte[SaltLength];
            random.NextBytes(result);
            return result;
        }
    }
}