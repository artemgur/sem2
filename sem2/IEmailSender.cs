using System.Threading.Tasks;

namespace sem2
{
    public interface IEmailSender
    {
        public Task<bool> SendEmailAsync(string email, string subject, string message);
    }
}