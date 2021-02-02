using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces
{
    public interface IEmailConfirmation
    {
        /// <summary>
        /// Method for sending email through gmail
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <returns>Result of sending email</returns>
        Task<bool> SendEmailAsync(string email, string subject, string message, string title);
    }
}
