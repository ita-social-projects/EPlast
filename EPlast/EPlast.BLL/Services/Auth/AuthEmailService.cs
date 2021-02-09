using EPlast.BLL.DTO.Account;
using EPlast.BLL.Interfaces;
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
        private readonly IActionContextAccessor _actionContextAccessor;

        private readonly IAuthService _authService;

        private readonly IHttpContextAccessor _contextAccessor;

        private readonly IEmailSendingService _emailSendingService;

        private readonly IUrlHelperFactory _urlHelperFactory;

        private readonly UserManager<User> _userManager;

        public AuthEmailService(
            IEmailSendingService emailSendingService,
            IAuthService authService,
            UserManager<User> userManager,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor,
            IHttpContextAccessor contextAccessor)
        {
            _emailSendingService = emailSendingService;
            _authService = authService;
            _userManager = userManager;

            _contextAccessor = contextAccessor;
            _urlHelperFactory = urlHelperFactory;
            _actionContextAccessor = actionContextAccessor;
        }

        /// <inheritdoc />
        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return result;
        }

        /// <inheritdoc />
        public async Task SendEmailGreetingAsync(string email)
        {
            var citiesUrl = ConfigSettingLayoutRenderer.DefaultConfiguration.GetSection("URLs")["Cities"];
            var user = await _userManager.FindByEmailAsync(email);
            user.EmailSendedOnRegister = DateTime.Now;
            await _emailSendingService.SendEmailAsync(user.Email,
                                                      "Вітаємо у системі!",
                                                      $"Ви успішно активували свій акаунт!\nНе забудьте доєднатись до осередку, перейшовши за :  <a href='{citiesUrl}'>посиланням</a> ",
                                                      "Адміністрація сайту EPlast");
        }

        public async Task<bool> SendEmailJoinToCityReminderAsync(string email)
        {
            var citiesUrl = ConfigSettingLayoutRenderer.DefaultConfiguration.GetSection("URLs")["Cities"];
            var sendResult = await _emailSendingService.SendEmailAsync(email,
                                                                       "Нагадування про приєднання до станиці",
                                                                       "Нагадуємо, що вам необхідно приєднатись до станиці.\n"
                                                                       + $"Зробити це можна, перейшовши за :  <a href='{citiesUrl}'>посиланням</a> ",
                                                                       "Адміністрація сайту EPlast");
            return sendResult;
        }

        /// <inheritdoc />
        public async Task<bool> SendEmailRegistrAsync(string email)
        {
            IUrlHelper _url = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
            var token = await _authService.AddRoleAndTokenAsync(email);
            var user = await _userManager.FindByEmailAsync(email);
            var confirmationLink = _url.Action("confirmingEmail",
                                               "Auth",
                                               new { token, userId = user.Id },
                                               _contextAccessor.HttpContext.Request.Scheme);
            user.EmailSendedOnRegister = DateTime.Now;

            return await _emailSendingService.SendEmailAsync(email,
                                                             "Підтвердження реєстрації ",
                                                             $"Підтвердіть реєстрацію, перейшовши за :  <a href='{confirmationLink}'>посиланням</a> ",
                                                             "Адміністрація сайту EPlast");
        }

        /// <inheritdoc />
        public async Task SendEmailResetingAsync(string confirmationLink, ForgotPasswordDto forgotPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            user.EmailSendedOnForgotPassword = DateTime.Now;
            await _userManager.UpdateAsync(user);
            await _emailSendingService.SendEmailAsync(forgotPasswordDto.Email,
                                                      "Скидування пароля",
                                                      $"Для скидування пароля перейдіть за : <a href='{confirmationLink}'>посиланням</a>",
                                                      "Адміністрація сайту EPlast");
        }
    }
}