using EPlast.BussinessLayer.DTO.Account;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<AuthenticationScheme>> GetAuthenticationSchemes();
        Task<SignInResult> SignIn(LoginDto loginDto);
        Task<User> FindByEmailAsync(string email);
        Task<bool> IsEmailConfirmedInUser(User user);
        Task<IdentityResult> CreateUser(RegisterDto registerDto);
        Task<string> AddRoleAndToken(RegisterDto registerDto);
        void SendEmailUserForRegistration(string confirmationLink, RegisterDto registerDto);
        Task<User> FindByIdAsync(string id);
        Task<IdentityResult> ConfirmEmail(User user, string code);
        int GetTimeAfterRegistering(User user);
        
        /*void SignOut();
        
        void SendEmailForResetingPassword(User user, ForgotPasswordDto forgotPasswordDto);
        Task<IdentityResult> ResetPassword(User user, ResetPasswordDto resetPasswordDto);
        void CheckingForLocking(User user);
        Task<User> GetUser(ClaimsPrincipal claimsPrincipal);
        Task<IdentityResult> ChangePasswordForUser(User user, ChangePasswordDto changePasswordDto);
        void RefreshSignIn(User user);
        AuthenticationProperties GetAuthenticationProperties(string provider, string returnUrl);

        void GoogleAuthentication(string email, User user, ExternalLoginInfo externalLoginInfo);
        void FacebookAuthentication(string email, ExternalLoginInfo externalLoginInfo);*/
    }
}
