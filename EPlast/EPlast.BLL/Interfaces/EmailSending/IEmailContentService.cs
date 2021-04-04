using EPlast.BLL.Models;
using EPlast.DataAccess.Entities;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces
{
    /// <summary>
    /// Returns emails contents needed for EPlast
    /// </summary>
    public interface IEmailContentService
    {
        /// <summary>
        /// Get email for Facebook registration 
        /// </summary>
        /// <returns>Email content</returns>
        EmailModel GetAuthFacebookRegisterEmail();

        /// <summary>
        /// Get email for Google registration 
        /// </summary>
        /// <returns>Email content</returns>
        EmailModel GetAuthGoogleRegisterEmail();

        /// <summary>
        /// Get email for registration greeting
        /// </summary>
        /// <param name="citiesUrl">Cities url</param>
        /// <returns>Email content</returns>
        EmailModel GetAuthGreetingEmail(string citiesUrl);

        /// <summary>
        /// Get email for reminding to join city
        /// </summary>
        /// <param name="citiesUrl">Cities url</param>
        /// <param name="userId">User Id</param>
        /// <returns>Email content</returns>
        Task<EmailModel> GetAuthJoinToCityReminderEmailAsync(string citiesUrl, string userId);

        /// <summary>
        /// Get email for registration confirmation
        /// </summary>
        /// <param name="confirmationLink">Registration confirmation link</param>
        /// <returns>Email content</returns>
        EmailModel GetAuthRegisterEmail(string confirmationLink);

        /// <summary>
        /// Get email for password resetting
        /// </summary>
        /// <param name="confirmationLink">Password resetting confirmation link</param>
        /// <returns>Email content</returns>
        EmailModel GetAuthResetPasswordEmail(string confirmationLink);

        /// <summary>
        /// Get email to inform user about approve or exclude from city
        /// </summary>
        /// <param name="cityUrl">City url</param>
        /// <param name="cityName">City name</param>
        /// <param name="isApproved">Is user approved or removed</param>
        /// <returns>Email content</returns>
        EmailModel GetCityApproveEmail(string cityUrl, string cityName, bool isApproved);

        /// <summary>
        /// Get email to inform user that he was confirmed by other user
        /// </summary>
        /// <param name="vaucheeUser">User that was confirmed</param>
        /// <param name="vaucherUser">User that confirmed</param>
        /// <returns>Email content</returns>
        Task<EmailModel> GetConfirmedUserEmailAsync(User vaucheeUser, User vaucherUser);

        /// <summary>
        /// Get email to inform user that other user removed his confirmation
        /// </summary>
        /// <param name="vaucheeUser">User whose confirmation was removed</param>
        /// <param name="vaucherUser">User who removed confirmation</param>
        /// <returns></returns>
        Task<EmailModel> GetCanceledUserEmailAsync(User vaucheeUser, User vaucherUser);

        /// <summary>
        /// Get email to greet new plast members
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>Email content</returns>
        Task<EmailModel> GetGreetingForNewPlastMemberEmailAsync(string userId);

    }
}
