using EPlast.BLL.DTO.Account;
using EPlast.BLL.DTO.UserProfiles;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces
{
    public interface IAuthEmailService
    {
        /// <summary>
        /// Sending email for registration
        /// </summary>
        /// <param name="confirmationLink"></param>
        /// <param name="userDto"></param>
        /// <returns>Result of sending email</returns>
        Task<bool> SendEmailRegistr(string email);

        /// <summary>
        /// Sending email reminder
        /// </summary>
        /// <param name="citiesUrl"></param>
        /// <param name="userDTO"></param>
        /// <returns>Result of sending email</returns>
        Task SendEmailReminder(string citiesUrl, UserDTO userDTO);

        /// <summary>
        /// Sending email for password reset
        /// </summary>
        /// <param name="confirmationLink"></param>
        /// <param name="forgotPasswordDto"></param>
        /// <returns>Result of sending email</returns>
        Task SendEmailReseting(string confirmationLink, ForgotPasswordDto forgotPasswordDto);
    }
}
