using EPlast.BLL.Models;
using EPlast.DataAccess.Entities;
using System;
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
        /// Get email for city admin to inform about new plast member in his city
        /// </summary>
        /// <param name="userFirstName">User first name</param>
        /// <param name="userLastName">User last name</param>
        /// <param name="userBirthday">User birthday</param>
        /// <returns>Email content</returns>
        EmailModel GetCityAdminAboutNewPlastMemberEmail(string userFirstName, string userLastName, DateTime? userBirthday);

        /// <summary>
        /// Get email to inform user about exclude from city followers
        /// </summary>
        /// <param name="cityUrl">City url</param>
        /// <param name="cityName">City name</param>
        /// <returns>Email content</returns>
        EmailModel GetCityRemoveFollowerEmail(string cityUrl, string cityName);

        /// <summary>
        /// Get email to inform user about his new supporter role when he was approved
        /// </summary>
        /// <returns>Email content</returns>
        EmailModel GetCityToSupporterRoleOnApproveEmail();

        /// <summary>
        /// Get email to inform or weekly remind admin about new city follower
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="userFirstName">User first name</param>
        /// <param name="userLastName">User last name</param>
        /// <param name="isReminder">Determinate whether its first email or weekly reminder</param>
        /// <returns>Email content</returns>
        Task<EmailModel> GetCityAdminAboutNewFollowerEmailAsync(string userId, string userFirstName, string userLastName, bool isReminder);

        /// <summary>
        /// Get email to inform user about approve in city
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="cityUrl">City url</param>
        /// <param name="cityName">City name</param>
        /// <returns>Email content</returns>
        Task<EmailModel> GetCityApproveEmailAsync(string userId, string cityUrl, string cityName);

        /// <summary>
        /// Get email to inform user about exclude from city
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="cityUrl">City url</param>
        /// <param name="cityName">City name</param>
        /// <returns>Email content</returns>
        Task<EmailModel> GetCityExcludeEmailAsync(string userId, string cityUrl, string cityName);

        /// <summary>
        /// Get email for reminding to join city
        /// </summary>
        /// <param name="citiesUrl">Cities url</param>
        /// <param name="userId">User Id</param>
        /// <returns>Email content</returns>
        Task<EmailModel> GetAuthJoinToCityReminderEmailAsync(string citiesUrl, string userId);

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
