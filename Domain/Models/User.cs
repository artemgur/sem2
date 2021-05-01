using System.Collections.Generic;
using System.Linq;

namespace DomainModels.Models
{
    public class User
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; private set; }
        public byte[] Salt { get; private set; }
        public string Email { get; private set; }
        public ICollection<Subscription> Subscriptions { get; private set; }
        public bool IsConfirmed { get; private set; }

        private User()
        {
            
        }

        public static User CreateUser(IEnumerable<User> users, string name, string surname, string email,
            string password)
        {
            if (users.Any(x => x.Email == email))
                return null;
            var user = new User
            {
                Name = name,
                Surname = surname,
                Email = email,
                IsConfirmed = false
            };
            user.SetPassword(password);
            return user;
        }

        public void SetConfirmed() => IsConfirmed = true;

        private void SetPassword(string password)
        {
            var salt = PasswordHasher.GenerateSalt();
            var hashedPassword = PasswordHasher.HashPassword(password, salt);
            Password = hashedPassword;
            Salt = salt;
        }

        public bool IsPasswordCorrect(string password)
        {
            var hashedPassword = PasswordHasher.HashPassword(password, Salt);
            return hashedPassword == Password;
        }
    }
}