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
        Task<SignInResult> SignIn(User user, string password, bool rememberMe, bool flag);
        Task<User> FindByEmailAsync(string email);
        Task<IdentityResult> Create(RegisterDto registerDto);
        void SendEmailUserForRegistration(User user, RegisterDto registerDto);
        Task<User> FindByIdAsync(string id);
        void AddRoleAsync(User user);
        void SignOut();
        Task<bool> IsEmailConfirmedInUser(User user);
        void SendEmailForResetingPassword(User user, ForgotPasswordDto forgotPasswordDto);
        Task<IdentityResult> ResetPassword(User user, ResetPasswordDto resetPasswordDto);
        void CheckingForLocking(User user);
        Task<User> GetUser(ClaimsPrincipal claimsPrincipal);
        Task<IdentityResult> ChangePasswordForUser(User user, ChangePasswordDto changePasswordDto);
        void RefreshSignIn(User user);
        AuthenticationProperties GetAuthenticationProperties(string provider, string returnUrl);


    }
}
