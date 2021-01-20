using AutoMapper;
using EPlast.BLL.DTO.Account;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Threading.Tasks;

namespace EPlast.BLL.Services
{
    public class AuthEmailService : IAuthEmailService
    {
        private readonly IEmailConfirmation _emailConfirmation;
        private readonly IAuthService _authService;
        private readonly UserManager<User> _userManager;
        private readonly IUrlHelper _Url;
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthEmailService(
            IEmailConfirmation emailConfirmation,
            IAuthService authService,
            UserManager<User> userManager,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor,
            IHttpContextAccessor contextAccessor)
        {
            _emailConfirmation = emailConfirmation;
            _authService = authService;
            _userManager = userManager;
            _Url = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            _contextAccessor = contextAccessor;
        }

        ///<inheritdoc/>
        public async Task<bool> SendEmailRegistr(string email)
        {
            string token = await _authService.AddRoleAndTokenAsync(email);
            var user = await _userManager.FindByEmailAsync(email);
            string confirmationLink = _Url.Action(
                        action: "confirmingEmail",
                        controller: "Auth",
                        values: new { token, userId = user.Id },
                        protocol: _contextAccessor.HttpContext.Request.Scheme);
            user.EmailSendedOnRegister = DateTime.Now;
            
            return (await _emailConfirmation.SendEmailAsync(
                email: email,
                subject: "Підтвердження реєстрації ",
                message: $"Підтвердіть реєстрацію, перейшовши за :  <a href='{confirmationLink}'>посиланням</a> ",
                title: "Адміністрація сайту EPlast"));
        }

        ///<inheritdoc/>
        public async Task SendEmailReminder(string citiesUrl, UserDTO userDTO)
        {
            var user = await _userManager.FindByEmailAsync(userDTO.Email);
            user.EmailSendedOnRegister = DateTime.Now;
            await _emailConfirmation.SendEmailAsync(user.Email, "Вітаємо у системі!",
                $"Ви успішно активували свій акаунт!\nНе забудьте доєднатись до осередку, перейшовши за :  <a href='{citiesUrl}'>посиланням</a> ", "Адміністрація сайту EPlast");
        }

        ///<inheritdoc/>
        public async Task SendEmailReseting(string confirmationLink, ForgotPasswordDto forgotPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            user.EmailSendedOnForgotPassword = DateTime.Now;
            await _userManager.UpdateAsync(user);
            await _emailConfirmation.SendEmailAsync(forgotPasswordDto.Email, "Скидування пароля",
                $"Для скидування пароля перейдіть за : <a href='{confirmationLink}'>посиланням</a>", "Адміністрація сайту EPlast");
        }
    }
}
