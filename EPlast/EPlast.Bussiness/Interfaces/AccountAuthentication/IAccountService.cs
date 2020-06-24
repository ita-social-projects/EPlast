using EPlast.BusinessLogicLayer.DTO.Account;
using EPlast.BusinessLogicLayer.DTO.UserProfiles;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BusinessLogicLayer.Interfaces
{
    public interface IAccountService
    {
        Task<SignInResult> SignInAsync(LoginDto loginDto);
        void SignOutAsync();
        Task<IdentityResult> CreateUserAsync(RegisterDto registerDto);
        Task<IdentityResult> ConfirmEmailAsync(string userId, string code);
        Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto);
        void RefreshSignInAsync(UserDTO userDto);
        AuthenticationProperties GetAuthProperties(string provider, string returnUrl);
        Task<ExternalLoginInfo> GetInfoAsync();
        Task<SignInResult> GetSignInResultAsync(ExternalLoginInfo externalLoginInfo);
        Task<bool> IsEmailConfirmedAsync(UserDTO userDto);
        Task<string> AddRoleAndTokenAsync(RegisterDto registerDto);
        Task<string> GenerateConfToken(UserDTO userDto);
        Task<string> GenerateResetTokenAsync(UserDTO userDto);
        Task<IdentityResult> ResetPasswordAsync(string userId, ResetPasswordDto resetPasswordDto);
        Task CheckingForLocking(UserDTO userDto);
        Task<IEnumerable<AuthenticationScheme>> GetAuthSchemesAsync();
        Task<UserDTO> FindByEmailAsync(string email);
        Task<UserDTO> FindByIdAsync(string id);
        string GetIdForUser(ClaimsPrincipal claimsPrincipal);
        int GetTimeAfterRegistr(UserDTO userDto);
        int GetTimeAfterReset(UserDTO userDto);
        Task<UserDTO> GetUserAsync(ClaimsPrincipal claimsPrincipal);
        Task SendEmailRegistr(string confirmationLink, UserDTO userDto);
        Task SendEmailRegistr(string confirmationLink, RegisterDto registerDto);
        Task SendEmailReseting(string confirmationLink, ForgotPasswordDto forgotPasswordDto);
        Task GoogleAuthentication(string email, ExternalLoginInfo externalLoginInfo);
        Task FacebookAuthentication(string email, ExternalLoginInfo externalLoginInfo);
    }
}
