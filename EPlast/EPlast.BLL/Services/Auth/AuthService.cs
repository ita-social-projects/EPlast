﻿using AutoMapper;
using EPlast.BLL.DTO.Account;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using EPlast.BLL.Models;
using EPlast.DataAccess.Repositories;
using Newtonsoft.Json;
using NLog.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace EPlast.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailConfirmation _emailConfirmation;
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IUrlHelper _Url;
        private readonly IHttpContextAccessor _contextAccessor;
        public AuthService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailConfirmation emailConfirmation,
            IMapper mapper,
            IRepositoryWrapper repoWrapper
            , IUrlHelperFactory urlHelperFactory
            , IActionContextAccessor actionContextAccessor
            , IHttpContextAccessor contextAccessor
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailConfirmation = emailConfirmation;
            _mapper = mapper;
            _repoWrapper = repoWrapper;
            _Url = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            _contextAccessor = contextAccessor;
        }

        ///<inheritdoc/>
        public async Task<Microsoft.AspNetCore.Identity.SignInResult> SignInAsync(LoginDto loginDto)
        {
            var user = _userManager.FindByEmailAsync(loginDto.Email);
            var result = await _signInManager.PasswordSignInAsync(user.Result, loginDto.Password, loginDto.RememberMe, true);
            return result;
        }

        ///<inheritdoc/>
        public async void SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        ///<inheritdoc/>
        public async Task<IdentityResult> CreateUserAsync(RegisterDto registerDto)
        {
            var user = new User()
            {
                Email = registerDto.Email,
                UserName = registerDto.Email,
                LastName = registerDto.SurName,
                FirstName = registerDto.Name,
                RegistredOn = DateTime.Now,
                ImagePath = "default_user_image.png",
                SocialNetworking = false,
                UserProfile = new UserProfile()
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            return result;
        }

        ///<inheritdoc/>
        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return result;
        }

        ///<inheritdoc/>
        public async Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword,
                changePasswordDto.NewPassword);
            return result;
        }

        ///<inheritdoc/>
        public async void RefreshSignInAsync(UserDTO userDto)
        {
            await _signInManager.RefreshSignInAsync(_mapper.Map<UserDTO, User>(userDto));
        }

        ///<inheritdoc/>
        public AuthenticationProperties GetAuthProperties(string provider, string returnUrl)
        {
            AuthenticationProperties properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, returnUrl);
            return properties;
        }

        ///<inheritdoc/>
        public async Task<ExternalLoginInfo> GetInfoAsync()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            return info;
        }

        ///<inheritdoc/>
        public async Task<Microsoft.AspNetCore.Identity.SignInResult> GetSignInResultAsync(ExternalLoginInfo externalLoginInfo)
        {
            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider,
                    externalLoginInfo.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            return signInResult;
        }

        ///<inheritdoc/>
        public async Task<bool> IsEmailConfirmedAsync(UserDTO userDto)
        {
            bool result = await _userManager.IsEmailConfirmedAsync(_mapper.Map<UserDTO, User>(userDto));
            return result;
        }

        ///<inheritdoc/>
        public async Task<string> AddRoleAndTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            await _userManager.AddToRoleAsync(user, "Зареєстрований користувач");
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return code;
        }

        ///<inheritdoc/>
        public async Task<string> GenerateConfToken(UserDTO userDto)
        {
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(_mapper.Map<UserDTO, User>(userDto));
            return code;
        }

        ///<inheritdoc/>
        public async Task<string> GenerateResetTokenAsync(UserDTO userDto)
        {
            var user = _mapper.Map<UserDTO, User>(userDto);
            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
            return code;
        }

        ///<inheritdoc/>
        public async Task<IdentityResult> ResetPasswordAsync(string userId, ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Code, resetPasswordDto.Password);
            return result;
        }

        ///<inheritdoc/>
        public async Task CheckingForLocking(UserDTO userDto)
        {
            if (await _userManager.IsLockedOutAsync(_mapper.Map<UserDTO, User>(userDto)))
            {
                await _userManager.SetLockoutEndDateAsync(_mapper.Map<UserDTO, User>(userDto), DateTimeOffset.UtcNow);
            }
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<AuthenticationScheme>> GetAuthSchemesAsync()
        {
            var externalLogins = await _signInManager.GetExternalAuthenticationSchemesAsync();
            return externalLogins;
        }

        ///<inheritdoc/>
        public async Task<UserDTO> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return _mapper.Map<User, UserDTO>(user);
        }

        ///<inheritdoc/>
        public async Task<UserDTO> FindByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return _mapper.Map<User, UserDTO>(user);
        }

        ///<inheritdoc/>
        public async Task<string> GetIdForUser(User user)
        {
            var currentUserId = await _userManager.GetUserIdAsync(user);
            return currentUserId;
        }

        ///<inheritdoc/>
        public int GetTimeAfterRegistr(UserDTO userDto)
        {
            IDateTimeHelper dateTimeConfirming = new DateTimeHelper();
            var user = _mapper.Map<UserDTO, User>(userDto);
            int totalTime = (int)dateTimeConfirming.GetCurrentTime().Subtract(user.EmailSendedOnRegister).TotalMinutes;
            return totalTime;
        }

        ///<inheritdoc/>
        public int GetTimeAfterReset(UserDTO userDto)
        {
            IDateTimeHelper dateTimeResetingPassword = new DateTimeHelper();
            dateTimeResetingPassword.GetCurrentTime();
            int totalTime = (int)dateTimeResetingPassword.GetCurrentTime().Subtract(userDto.EmailSendedOnForgotPassword).TotalMinutes;
            return totalTime;
        }

        ///<inheritdoc/>
        public UserDTO GetUser(User user)
        {
            return _mapper.Map<User, UserDTO>(user);
        }

        ///<inheritdoc/>
        public async Task<bool> SendEmailRegistr(string email)
        {
            string token = await this.AddRoleAndTokenAsync(email);
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
        public async Task SendEmailReminder(string cityListUrl, UserDTO userDTO)
        {
            var user = await _userManager.FindByEmailAsync(userDTO.Email);
            user.EmailSendedOnRegister = DateTime.Now;
            //await _userManager.UpdateAsync(user);
            await _emailConfirmation.SendEmailAsync(user.Email, "Вітаємо у системі!",
                $"Ви успішно активували свій акаунт!\nНе забудьте доєднатись до осередку, перейшовши за :  <a href='{cityListUrl}'>посиланням</a> ", "Адміністрація сайту EPlast");
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

        ///<inheritdoc/>
        public async Task<UserDTO> GetGoogleUserAsync(string providerToken)
        {
            string googleApiTokenInfoUrl =
                ConfigSettingLayoutRenderer.DefaultConfiguration.GetSection("GoogleAuthentication")["GoogleApiTokenInfoUrl"];
            var httpClient = new HttpClient();
            var requestUri = new Uri(string.Format(googleApiTokenInfoUrl, providerToken));

            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(requestUri);

            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpRequestException("Status code isn`t correct");
            }

            var response = await httpResponseMessage.Content.ReadAsStringAsync();
            var googleApiTokenInfo = JsonConvert.DeserializeObject<GoogleApiTokenInfo>(response);
            var user = await _userManager.FindByEmailAsync(googleApiTokenInfo.Email);
            if (user == null)
            {
                user = new User
                {
                    UserName = googleApiTokenInfo.Email,
                    Email = googleApiTokenInfo.Email,
                    FirstName = googleApiTokenInfo.GivenName,
                    LastName = googleApiTokenInfo.FamilyName ?? googleApiTokenInfo.GivenName,
                    SocialNetworking = true,
                    ImagePath = "default_user_image.png",
                    EmailConfirmed = true,
                    RegistredOn = DateTime.Now,
                    UserProfile = new UserProfile(),
                };
                var createResult = await _userManager.CreateAsync(user);
                if (createResult.Succeeded)
                {
                    await _emailConfirmation.SendEmailAsync(user.Email, "Повідомлення про реєстрацію",
                     "Ви зареєструвались в системі EPlast використовуючи свій Google-акаунт. ", "Адміністрація сайту EPlast");
                }
                else
                    throw new ArgumentException("Failed creation of user");
                await _userManager.AddToRoleAsync(user, "Прихильник");
            }
            await _signInManager.SignInAsync(user, isPersistent: false);
            return _mapper.Map<User, UserDTO>(user);

        }
        public async Task<UserDTO> FacebookLoginAsync(FacebookUserInfo facebookUser)
        {
            var user = await _userManager.FindByEmailAsync(facebookUser.Email);
            if (user == null)
            {
                user = new User
                {
                    SocialNetworking = true,
                    UserName = facebookUser.Email ?? facebookUser.UserId,
                    FirstName = facebookUser.Name.Split(' ')[0],
                    Email = facebookUser.Email ?? "facebookdefaultmail@gmail.com",
                    LastName = facebookUser.Name.Split(' ')[1],
                    ImagePath = "default_user_image.png",
                    EmailConfirmed = true,
                    RegistredOn = DateTime.Now,
                    UserProfile = new UserProfile()
                    {
                        Birthday = DateTime.Parse(facebookUser.Birthday, CultureInfo.InvariantCulture),
                        GenderID = _repoWrapper.Gender.FindByCondition(x => x.Name == facebookUser.Gender).FirstOrDefault()?.ID,
                    }
                };
                var createResult = await _userManager.CreateAsync(user);
                if (createResult.Succeeded && user.Email != "facebookdefaultmail@gmail.com")
                {
                    await _emailConfirmation.SendEmailAsync(user.Email, "Повідомлення про реєстрацію",
                        "Ви зареєструвались в системі EPlast використовуючи свій Facebook-акаунт. ", "Адміністрація сайту EPlast");
                }
                await _userManager.AddToRoleAsync(user, "Прихильник");
            }
            await _signInManager.SignInAsync(user, isPersistent: false);
            return _mapper.Map<User, UserDTO>(user);
        }

    }
}
