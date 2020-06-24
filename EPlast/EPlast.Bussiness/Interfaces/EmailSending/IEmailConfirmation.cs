using System.Threading.Tasks;

namespace EPlast.Bussiness.Interfaces
{
    public interface IEmailConfirmation
    {
        Task SendEmailAsync(string email, string subject, string message, string title);
    }
}
