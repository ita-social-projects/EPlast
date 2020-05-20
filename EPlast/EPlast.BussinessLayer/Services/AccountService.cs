using EPlast.BussinessLayer.AccessManagers.Interfaces;
using EPlast.BussinessLayer.DTO.Account;
using EPlast.BussinessLayer.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace EPlast.BussinessLayer.Services
{
    public class AccountService : IAccountService
    {
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly ILogger _logger;
        private readonly IEmailConfirmation _emailConfirmation;
        private readonly IHostingEnvironment _env;
        private readonly IUserAccessManager _userAccessManager;
        private readonly IDateTimeHelper _dateTimeHelper;

        public AccountService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IRepositoryWrapper repoWrapper,
            ILogger<AccountService> logger,
            IEmailConfirmation emailConfirmation,
            IHostingEnvironment env,
            IUserAccessManager userAccessManager,
            IDateTimeHelper dateTimeHelper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repoWrapper = repoWrapper;
            _logger = logger;
            _emailConfirmation = emailConfirmation;
            _env = env;
            _userAccessManager = userAccessManager;
            _dateTimeHelper = dateTimeHelper;
        }

        public async Task<SignInResult> SignInAsync(LoginDto loginDto)
        {
            var user = _userManager.FindByEmailAsync(loginDto.Email);
            var result = await _signInManager.PasswordSignInAsync(user.Result, loginDto.Password, loginDto.RememberMe, true);
            return result;
        }

        public async void SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> CreateUserAsync(RegisterDto registerDto)
        {
            var user = new User()
            {
                Email = registerDto.Email,
                UserName = registerDto.Email,
                LastName = registerDto.SurName,
                FirstName = registerDto.Name,
                RegistredOn = DateTime.Now,
                ImagePath = "default.png",
                SocialNetworking = false,
                UserProfile = new UserProfile()
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            return result;
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string code)
        {
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return result;
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, ChangePasswordDto changePasswordDto)
        {
            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword,
                changePasswordDto.NewPassword);
            return result;
        }

        public async void RefreshSignInAsync(User user)
        {
            await _signInManager.RefreshSignInAsync(user);
        }

        public AuthenticationProperties GetAuthProperties(string provider, string returnUrl)
        {
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, returnUrl);
            return properties;
        }

        public async Task<ExternalLoginInfo> GetInfoAsync()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            return info;
        }

        public async Task<SignInResult> GetSignInResultAsync(ExternalLoginInfo externalLoginInfo)
        {
            var signInResult = await _signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider,
                    externalLoginInfo.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            return signInResult;
        }

        public async Task<bool> IsEmailConfirmedAsync(User user)
        {
            bool result = await _userManager.IsEmailConfirmedAsync(user);
            return result;
        }

        public async Task<string> AddRoleAndTokenAsync(RegisterDto registerDto)    //тут перевірити (FindByEmail) чи дійсно робить "Прихильник"
        {
            var user = await _userManager.FindByEmailAsync(registerDto.Email);
            await _userManager.AddToRoleAsync(user, "Прихильник");
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return code;
        }

        public async Task<string> GenerateConfToken(User user)
        {
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return code;
        }

        public async Task<string> GenerateResetTokenAsync(User user)
        {
            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
            return code;
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, ResetPasswordDto resetPasswordDto)
        {
            var result = await _userManager.ResetPasswordAsync(user, HttpUtility.UrlDecode(resetPasswordDto.Code), resetPasswordDto.Password);
            return result;
        }

        public async void CheckingForLocking(User user)
        {
            if (await _userManager.IsLockedOutAsync(user))
            {
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
            }
        }

        public async Task<IEnumerable<AuthenticationScheme>> GetAuthSchemesAsync()
        {
            var externalLogins = await _signInManager.GetExternalAuthenticationSchemesAsync();
            return externalLogins;
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            return result;
        }

        public async Task<User> FindByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user;
        }

        public string GetIdForUser(ClaimsPrincipal claimsPrincipal)
        {
            var currentUserId = _userManager.GetUserId(claimsPrincipal);
            return currentUserId;
        }

        public int GetTimeAfterRegistr(User user)
        {
            IDateTimeHelper dateTimeConfirming = new DateTimeHelper();
            int totalTime = (int)dateTimeConfirming.GetCurrentTime().Subtract(user.EmailSendedOnRegister).TotalMinutes;
            return totalTime;
        }

        public int GetTimeAfterReset(User user)
        {
            IDateTimeHelper dateTimeResetingPassword = new DateTimeHelper();
            dateTimeResetingPassword.GetCurrentTime();
            int totalTime = (int)dateTimeResetingPassword.GetCurrentTime().Subtract(user.EmailSendedOnForgotPassword).TotalMinutes;
            return totalTime;
        }

        public async Task<User> GetUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipal);
            return user;
        }

        public async void SendEmailRegistr(string confirmationLink, User user)
        {
            user.EmailSendedOnRegister = DateTime.Now;
            await _userManager.UpdateAsync(user); 
            await _emailConfirmation.SendEmailAsync(user.Email, "Підтвердження реєстрації ",
                $"Підтвердіть реєстрацію, перейшовши за :  <a href='{confirmationLink}'>посиланням</a> ", "Адміністрація сайту EPlast");
        }

        public async void SendEmailRegistr(string confirmationLink, RegisterDto registerDto)
        {
            var user = _userManager.FindByEmailAsync(registerDto.Email).Result;
            user.EmailSendedOnRegister = DateTime.Now;
            await _userManager.UpdateAsync(user);  
            await _emailConfirmation.SendEmailAsync(user.Email, "Підтвердження реєстрації ",
                $"Підтвердіть реєстрацію, перейшовши за :  <a href='{confirmationLink}'>посиланням</a> ", "Адміністрація сайту EPlast");
        }

        public async void SendEmailReseting(string confirmationLink, ForgotPasswordDto forgotPasswordDto)
        {
            var user = _userManager.FindByEmailAsync(forgotPasswordDto.Email).Result;
            user.EmailSendedOnForgotPassword = DateTime.Now;
            await _userManager.UpdateAsync(user); 
            await _emailConfirmation.SendEmailAsync(forgotPasswordDto.Email, "Скидування пароля",
                $"Для скидування пароля перейдіть за : <a href='{confirmationLink}'>посиланням</a>", "Адміністрація сайту EPlast");
        }

        public async void GoogleAuthentication(string email, ExternalLoginInfo externalLoginInfo)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new User
                {
                    SocialNetworking = true,
                    UserName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email),
                    Email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email),
                    FirstName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.GivenName),
                    LastName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Surname),
                    ImagePath = "default.png",
                    EmailConfirmed = true,
                    RegistredOn = DateTime.Now,
                    UserProfile = new UserProfile()
                };
                await _userManager.CreateAsync(user);
                await _emailConfirmation.SendEmailAsync(user.Email, "Повідомлення про реєстрацію",
            "Ви зареєструвались в системі EPlast використовуючи свій Google-акаунт ", "Адміністрація сайту EPlast");
            }
            await _userManager.AddToRoleAsync(user, "Прихильник");
            await _userManager.AddLoginAsync(user, externalLoginInfo);
            await _signInManager.SignInAsync(user, isPersistent: false);
        }

        public async void FacebookAuthentication(string email, ExternalLoginInfo externalLoginInfo)
        {
            var nameIdentifier = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var identifierForSearching = email ?? nameIdentifier;
            var user = _userManager.Users.FirstOrDefault(u => u.UserName == identifierForSearching);
            if (user == null)
            {
                user = new User
                {
                    SocialNetworking = true,
                    UserName = (email ?? nameIdentifier),
                    FirstName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.GivenName),
                    Email = (email ?? "facebookdefaultmail@gmail.com"),
                    LastName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Surname),
                    ImagePath = "default.png",
                    EmailConfirmed = true,
                    RegistredOn = DateTime.Now,
                    UserProfile = new UserProfile()
                };
                await _userManager.CreateAsync(user);
            }
            await _userManager.AddToRoleAsync(user, "Прихильник");
            await _userManager.AddLoginAsync(user, externalLoginInfo);
            await _signInManager.SignInAsync(user, isPersistent: false);
        }
    }
}
