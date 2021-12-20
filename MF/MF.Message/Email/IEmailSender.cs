using System.Threading.Tasks;

namespace MF.Message.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}