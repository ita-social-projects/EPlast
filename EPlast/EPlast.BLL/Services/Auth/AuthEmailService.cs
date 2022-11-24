using EPlast.BLL.DTO.Account;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.HostURL;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using NLog.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Auth
{
    public class AuthEmailService : IAuthEmailService
    {
        private readonly IHostUrlService _hostURLService;
        private readonly IAuthService _authService;
        private readonly IEmailSendingService _emailSendingService;
        private readonly IEmailContentService _emailContentService;
        private readonly UserManager<User> _userManager;

        public AuthEmailService(IEmailSendingService emailSendingService,
                                IEmailContentService emailContentService,
                                IAuthService authService,
                                UserManager<User> userManager,
                                IHostUrlService hostURLService)
        {
            _emailSendingService = emailSendingService;
            _emailContentService = emailContentService;
            _authService = authService;
            _userManager = userManager;
            _hostURLService = hostURLService;
        }

        /// <inheritdoc />
        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return result;
        }

        /// <inheritdoc />
        public async Task<bool> SendEmailGreetingAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            user.EmailSendedOnRegister = DateTime.Now;
            var emailContent = _emailContentService.GetAuthGreetingEmail(_hostURLService.CitiesURL);
            return await _emailSendingService.SendEmailAsync(user.Email, emailContent.Subject, emailContent.Message, emailContent.Title);
        }

        public async Task<bool> SendEmailJoinToCityReminderAsync(string email, string userId)
        {
            var emailContent = await _emailContentService.GetAuthJoinToCityReminderEmailAsync(_hostURLService.CitiesURL, userId);
            return await _emailSendingService.SendEmailAsync(email, emailContent.Subject, emailContent.Message, emailContent.Title);
        }

        /// <inheritdoc />
        public async Task<bool> SendEmailRegistrAsync(string email)
        {
            var token = await _authService.AddRoleAndTokenAsync(email);
            var user = await _userManager.FindByEmailAsync(email);
            var confirmationLink = _hostURLService.GetConfirmEmailApiURL(userId: user.Id, token: token);
            user.EmailSendedOnRegister = DateTime.Now;
            var emailContent = _emailContentService.GetAuthRegisterEmail(confirmationLink);
            return await _emailSendingService.SendEmailAsync(email, emailContent.Subject, emailContent.Message, emailContent.Title);
        }

        /// <inheritdoc />
        public async Task SendEmailResetingAsync(string confirmationLink, ForgotPasswordDto forgotPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            user.EmailSendedOnForgotPassword = DateTime.Now;
            await _userManager.UpdateAsync(user);
            var emailContent = _emailContentService.GetAuthResetPasswordEmail(confirmationLink);
            await _emailSendingService.SendEmailAsync(forgotPasswordDto.Email, emailContent.Subject, emailContent.Message, emailContent.Title);
        }
    }
}
