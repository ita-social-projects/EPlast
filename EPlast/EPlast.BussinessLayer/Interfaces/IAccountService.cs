using AutoMapper.Configuration.Conventions;
using EPlast.BussinessLayer.DTO.Account;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;
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
        int GetTimeAfterReseting(User user);//тут мож по трохи іншому назвати метод
        void SignOut();
        Task<string> GenerateResetToken(User user);
        void SendEmailForResetingPassword(string confirmationLink, ForgotPasswordDto forgotPasswordDto);
        Task<IdentityResult> ResetPassword(User user, ResetPasswordDto resetPasswordDto);
        void CheckingForLocking(User user);
        AuthenticationProperties GetAuthenticationProperties(string provider, string returnUrl);
        Task<ExternalLoginInfo> GetInfo();
        Task<SignInResult> GetSignInRes(ExternalLoginInfo externalLoginInfo);
        string GetIdForUser(ClaimsPrincipal claimsPrincipal);
        void GoogleAuthentication(string email, ExternalLoginInfo externalLoginInfo);
        void FacebookAuthentication(string email, ExternalLoginInfo externalLoginInfo);


        /*
        Task<IdentityResult> ResetPassword(User user, ResetPasswordDto resetPasswordDto);
        Task<User> GetUser(ClaimsPrincipal claimsPrincipal);
        Task<IdentityResult> ChangePasswordForUser(User user, ChangePasswordDto changePasswordDto);
        void RefreshSignIn(User user);
        AuthenticationProperties GetAuthenticationProperties(string provider, string returnUrl);
        */
    }
}
