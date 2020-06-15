using EPlast.BussinessLayer.DTO.Account;
using EPlast.BussinessLayer.DTO.UserProfiles;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Interfaces
{
    public interface IAccountService
    {
        Task<SignInResult> SignInAsync(LoginDTO loginDto);
        void SignOutAsync();
        Task<IdentityResult> CreateUserAsync(RegisterDTO registerDto);
        Task<IdentityResult> ConfirmEmailAsync(string userId, string code);
        Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordDTO changePasswordDto);
        void RefreshSignInAsync(UserDTO userDto);
        AuthenticationProperties GetAuthProperties(string provider, string returnUrl);
        Task<ExternalLoginInfo> GetInfoAsync();
        Task<SignInResult> GetSignInResultAsync(ExternalLoginInfo externalLoginInfo);
        Task<bool> IsEmailConfirmedAsync(UserDTO userDto);
        Task<string> AddRoleAndTokenAsync(RegisterDTO registerDto);
        Task<string> GenerateConfToken(UserDTO userDto);
        Task<string> GenerateResetTokenAsync(UserDTO userDto);
        Task<IdentityResult> ResetPasswordAsync(string userId, ResetPasswordDTO resetPasswordDto);
        Task CheckingForLocking(UserDTO userDto);
        Task<IEnumerable<AuthenticationScheme>> GetAuthSchemesAsync();
        Task<UserDTO> FindByEmailAsync(string email);
        Task<UserDTO> FindByIdAsync(string id);
        string GetIdForUser(ClaimsPrincipal claimsPrincipal);
        int GetTimeAfterRegistr(UserDTO userDto);
        int GetTimeAfterReset(UserDTO userDto);
        Task<UserDTO> GetUserAsync(ClaimsPrincipal claimsPrincipal);
        Task SendEmailRegistr(string confirmationLink, UserDTO userDto);
        Task SendEmailRegistr(string confirmationLink, RegisterDTO registerDto);
        Task SendEmailReseting(string confirmationLink, ForgotPasswordDTO forgotPasswordDto);
        Task GoogleAuthentication(string email, ExternalLoginInfo externalLoginInfo);
        Task FacebookAuthentication(string email, ExternalLoginInfo externalLoginInfo);
    }
}
