using EPlast.BLL.DTO.Account;
using EPlast.BLL.DTO.UserProfiles;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EPlast.BLL.Models;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Interfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Login in system
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns>Result of logining in system</returns>
        Task<SignInResult> SignInAsync(LoginDto loginDto);

        /// <summary>
        /// Logout in system
        /// </summary>
        void SignOutAsync();

        /// <summary>
        /// Creating user in database
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns>Result of creating user in system</returns>
        Task<IdentityResult> CreateUserAsync(RegisterDto registerDto);

        /// <summary>
        /// Confirming Email after registration
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns>Result of confirming email in system</returns>
        Task<IdentityResult> ConfirmEmailAsync(string userId, string code);

        /// <summary>
        /// Changing password for user account
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="changePasswordDto"></param>
        /// <returns>Result of changing password</returns>
        Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto);

        /// <summary>
        /// Refresh signin credentials
        /// </summary>
        /// <param name="userDto"></param>
        void RefreshSignInAsync(UserDTO userDto);

        /// <summary>
        /// Get authentication properties for user
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="returnUrl"></param>
        /// <returns>Authentication properties for user</returns>
        AuthenticationProperties GetAuthProperties(string provider, string returnUrl);

        /// <summary>
        /// Get information about external login
        /// </summary>
        /// <returns>External login information</returns>
        Task<ExternalLoginInfo> GetInfoAsync();

        /// <summary>
        /// Get sign in result
        /// </summary>
        /// <param name="externalLoginInfo"></param>
        /// <returns>Sign in result</returns>
        Task<SignInResult> GetSignInResultAsync(ExternalLoginInfo externalLoginInfo);

        /// <summary>
        /// Checking if email was confirmed
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns>Result of confirming email</returns>
        Task<bool> IsEmailConfirmedAsync(UserDTO userDto);

        /// <summary>
        /// Add role and token for user after registration
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns>Result of adding role and token</returns>
        Task<string> AddRoleAndTokenAsync(string email);

        /// <summary>
        /// Generating confirmation token
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns>Result of generating token</returns>
        Task<string> GenerateConfToken(UserDTO userDto);

        /// <summary>
        /// Generating reset token 
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns>Result of generating reset token</returns>
        Task<string> GenerateResetTokenAsync(UserDTO userDto);

        /// <summary>
        /// Reseting password for user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="resetPasswordDto"></param>
        /// <returns>Returs result of reseting password</returns>
        Task<IdentityResult> ResetPasswordAsync(string userId, ResetPasswordDto resetPasswordDto);

        /// <summary>
        /// Checking if user was locked
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns>Returns locking result</returns>
        Task CheckingForLocking(UserDTO userDto);

        /// <summary>
        /// Get authentication scheme for user
        /// </summary>
        /// <returns>Returns authentication scheme</returns>
        Task<IEnumerable<AuthenticationScheme>> GetAuthSchemesAsync();

        /// <summary>
        /// Finding by email in database
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Returns user from database</returns>
        Task<UserDTO> FindByEmailAsync(string email);

        /// <summary>
        /// Finding by id in database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns user from database</returns>
        Task<UserDTO> FindByIdAsync(string id);

        /// <summary>
        /// Get id for user from database
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns id of user</returns>
        Task<string> GetIdForUser(User user);

        /// <summary>
        /// Returns time after registration for user
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns>Time after registration</returns>
        int GetTimeAfterRegistr(UserDTO userDto);

        /// <summary>
        /// Returns time after reseting password
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns>Time after reseting password</returns>
        int GetTimeAfterReset(UserDTO userDto);

        /// <summary>
        /// Get current user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>User who was logged in</returns>
        UserDTO GetUser(User user);

        /// <summary>
        /// Sending email for registration
        /// </summary>
        /// <param name="confirmationLink"></param>
        /// <param name="userDto"></param>
        /// <returns>Result of sending email</returns>
        Task<bool> SendEmailRegistr(string email);

        /// <summary>
        /// Sending email for registration
        /// </summary>
        /// <param name="confirmationLink"></param>
        /// <param name="registerDto"></param>
        /// <returns>Result of sending email</returns>
        Task SendEmailReseting(string confirmationLink, ForgotPasswordDto forgotPasswordDto);

        /// <summary>
        /// Sending email reminder
        /// </summary>
        /// <param name="cityListUrl"></param>
        /// <param name="userDTO"></param>
        /// <returns>Result of sending email</returns>
        Task SendEmailReminder(string cityListUrl, UserDTO userDTO);

        /// <summary>
        ///  Validates google token and gets user information
        /// </summary>
        /// <param name="providerToken"></param>
        /// <returns>Returns Google user information</returns>
        /// 
        Task<UserDTO> GetGoogleUserAsync(string providerToken);

        /// <summary>
        /// Sign in using Facebook
        /// </summary>
        /// <param name="facebookUser"></param>
        Task<UserDTO> FacebookLoginAsync(FacebookUserInfo facebookUser);
    }
}
