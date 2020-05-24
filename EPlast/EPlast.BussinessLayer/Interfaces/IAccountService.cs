using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using EPlast.BussinessLayer.DTO.Account;
using EPlast.DataAccess.Entities;
using EPlast.BussinessLayer.DTO;

namespace EPlast.BussinessLayer.Interfaces
{
    public interface IAccountService
    {
        Task<SignInResult> SignInAsync(LoginDto loginDto);
        void SignOutAsync();
        Task<IdentityResult> CreateUserAsync(RegisterDto registerDto);
        Task<IdentityResult> ConfirmEmailAsync(UserDTO userDto, string code);
        Task<IdentityResult> ChangePasswordAsync(UserDTO userDto, ChangePasswordDto changePasswordDto);
        void RefreshSignInAsync(UserDTO userDto);
        AuthenticationProperties GetAuthProperties(string provider, string returnUrl);
        Task<ExternalLoginInfo> GetInfoAsync();
        Task<SignInResult> GetSignInResultAsync(ExternalLoginInfo externalLoginInfo);
        Task<bool> IsEmailConfirmedAsync(UserDTO userDto);
        Task<string> AddRoleAndTokenAsync(RegisterDto registerDto);
        Task<string> GenerateConfToken(UserDTO userDto);
        Task<string> GenerateResetTokenAsync(UserDTO userDto);
        Task<IdentityResult> ResetPasswordAsync(UserDTO userDto, ResetPasswordDto resetPasswordDto);
        void CheckingForLocking(UserDTO userDto);
        Task<IEnumerable<AuthenticationScheme>> GetAuthSchemesAsync();
        Task<UserDTO> FindByEmailAsync(string email);
        Task<UserDTO> FindByIdAsync(string id);
        string GetIdForUser(ClaimsPrincipal claimsPrincipal);
        int GetTimeAfterRegistr(UserDTO userDto);
        int GetTimeAfterReset(UserDTO userDto);
        Task<UserDTO> GetUserAsync(ClaimsPrincipal claimsPrincipal);
        void SendEmailRegistr(string confirmationLink, UserDTO userDto);
        void SendEmailRegistr(string confirmationLink, RegisterDto registerDto);
        void SendEmailReseting(string confirmationLink, ForgotPasswordDto forgotPasswordDto);
        Task GoogleAuthentication(string email, ExternalLoginInfo externalLoginInfo);
        Task FacebookAuthentication(string email, ExternalLoginInfo externalLoginInfo);
    }
}
