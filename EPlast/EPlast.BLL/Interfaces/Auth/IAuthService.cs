using EPlast.BLL.DTO.Account;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Models;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Add role and token for user after registration
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns>Result of adding role and token</returns>
        Task<string> AddRoleAndTokenAsync(string email);

        /// <summary>
        /// Changing password for user account
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="changePasswordDto"></param>
        /// <returns>Result of changing password</returns>
        Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto);

        /// <summary>
        /// Checking if user was locked
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns>Returns locking result</returns>
        Task CheckingForLocking(UserDTO userDto);

        /// <summary>
        /// Creating user in database
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns>Result of creating user in system</returns>
        Task<IdentityResult> CreateUserAsync(RegisterDto registerDto);

        /// <summary>
        /// Sign in using Facebook
        /// </summary>
        /// <param name="facebookUser"></param>
        Task<UserDTO> FacebookLoginAsync(FacebookUserInfo facebookUser);

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
        /// Get authentication properties for user
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="returnUrl"></param>
        /// <returns>Authentication properties for user</returns>
        AuthenticationProperties GetAuthProperties(string provider, string returnUrl);

        /// <summary>
        /// Get authentication scheme for user
        /// </summary>
        /// <returns>Returns authentication scheme</returns>
        Task<IEnumerable<AuthenticationScheme>> GetAuthSchemesAsync();

        /// <summary>
        /// Validates google token and gets user information
        /// </summary>
        /// <param name="providerToken"></param>
        /// <returns>Returns Google user information</returns>
        Task<UserDTO> GetGoogleUserAsync(string providerToken);

        /// <summary>
        /// Get id for user from database
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns id of user</returns>
        Task<string> GetIdForUserAsync(User user);

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
        /// Returns time after registration for user
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns>Time after registration</returns>
        int GetTimeAfterRegister(UserDTO userDto);

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
        /// Checking if email was confirmed
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns>Result of confirming email</returns>
        Task<bool> IsEmailConfirmedAsync(UserDTO userDto);

        /// <summary>
        /// Refresh signin credentials
        /// </summary>
        /// <param name="userDto"></param>
        Task<bool> RefreshSignInAsync(UserDTO userDto);

        /// <summary>
        /// Reseting password for user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="resetPasswordDto"></param>
        /// <returns>Returs result of reseting password</returns>
        Task<IdentityResult> ResetPasswordAsync(string userId, ResetPasswordDto resetPasswordDto);

        /// <summary>
        /// Login in system
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns>Result of logining in system</returns>
        Task<SignInResult> SignInAsync(LoginDto loginDto);

        /// <summary>
        /// Delete users without confirmed email
        /// </summary>
        Task DeleteUserIfEmailNotConfirmedAsync();

        /// <summary>
        /// Logout in system
        /// </summary>
        void SignOutAsync();
    }
}
