using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using EPlast.BussinessLayer.DTO.Account;
using EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Interfaces
{
    public interface IAccountService
    {
        Task<SignInResult> SignInAsync(LoginDto loginDto);
        void SignOutAsync();
        Task<IdentityResult> CreateUserAsync(RegisterDto registerDto);
        Task<IdentityResult> ConfirmEmailAsync(User user, string code);
        Task<IdentityResult> ChangePasswordAsync(User user, ChangePasswordDto changePasswordDto);
        void RefreshSignInAsync(User user);
        AuthenticationProperties GetAuthProperties(string provider, string returnUrl);
        Task<ExternalLoginInfo> GetInfoAsync();
        Task<SignInResult> GetSignInResultAsync(ExternalLoginInfo externalLoginInfo);
        Task<bool> IsEmailConfirmedAsync(User user);
        Task<string> AddRoleAndTokenAsync(RegisterDto registerDto);
        Task<string> GenerateConfToken(User user);
        Task<string> GenerateResetTokenAsync(User user);
        Task<IdentityResult> ResetPasswordAsync(User user, ResetPasswordDto resetPasswordDto);
        void CheckingForLocking(User user);
        Task<IEnumerable<AuthenticationScheme>> GetAuthSchemesAsync();
        Task<User> FindByEmailAsync(string email);
        Task<User> FindByIdAsync(string id);
        string GetIdForUser(ClaimsPrincipal claimsPrincipal);
        int GetTimeAfterRegistr(User user);
        int GetTimeAfterReset(User user);
        Task<User> GetUserAsync(ClaimsPrincipal claimsPrincipal);
        void SendEmailRegistr(string confirmationLink, User user);
        void SendEmailRegistr(string confirmationLink, RegisterDto registerDto);
        void SendEmailReseting(string confirmationLink, ForgotPasswordDto forgotPasswordDto);
        Task GoogleAuthentication(string email, ExternalLoginInfo externalLoginInfo);
        void FacebookAuthentication(string email, ExternalLoginInfo externalLoginInfo);
    }
}
