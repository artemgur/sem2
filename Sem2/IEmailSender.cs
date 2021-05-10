using System.Threading.Tasks;

namespace Sem2
{
    public interface IEmailSender
    {
        public Task<bool> SendEmailAsync(string email, string subject, string message);
    }
}