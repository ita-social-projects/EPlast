using EPlast.BussinessLayer.AccessManagers.Interfaces;
using EPlast.BussinessLayer.DTO.Account;
using EPlast.BussinessLayer.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
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

        public AccountService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IRepositoryWrapper repoWrapper,
            ILogger<AccountService> logger,
            IEmailConfirmation emailConfirmation,
            IHostingEnvironment env,
            IUserAccessManager userAccessManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repoWrapper = repoWrapper;
            _logger = logger;
            _emailConfirmation = emailConfirmation;
            _env = env;
            _userAccessManager = userAccessManager;
        }

        public async Task<IEnumerable<AuthenticationScheme>> GetAuthenticationSchemesAsync()
        {
            var externalLogins = await _signInManager.GetExternalAuthenticationSchemesAsync();
            return externalLogins;
        }

        public async Task<SignInResult> SignIn(User user, string Password, bool RememberMe, bool flag)
        {
            var result = await _signInManager.PasswordSignInAsync(user, Password, RememberMe, flag);
            return result;
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            return result;
        }

        public async Task<IdentityResult> Create(RegisterDto registerDto)
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

        public async void AddRoleAsync(User user)
        {
            await _userManager.AddToRoleAsync(user, "Прихильник");
        }

        public async void SendEmailUserForRegistration(User user, RegisterDto registerDto)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(
                nameof(ConfirmingEmail),
                "Account",
                new { code = code, userId = user.Id },
                protocol: HttpContext.Request.Scheme);

            user.EmailSendedOnRegister = DateTime.Now;
            await _userManager.UpdateAsync(user);
            await _emailConfirmation.SendEmailAsync(registerDto.Email, "Підтвердження реєстрації ",
                $"Підтвердіть реєстрацію, перейшовши за :  <a href='{confirmationLink}'>посиланням</a> ", "Адміністрація сайту EPlast");
        }

        public async Task<User> FindByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user;
        }

        public async void SignOut()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> IsEmailConfirmedInUser(User user)
        {
            bool result = await _userManager.IsEmailConfirmedAsync(user);
            return result;
        }
        public async void SendEmailForResetingPassword(User user, ForgotPasswordDto forgotPasswordDto)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action(
                nameof(ResetPassword),
                "Account",
                new { userId = user.Id, code = HttpUtility.UrlEncode(code) },
                protocol: HttpContext.Request.Scheme);

            user.EmailSendedOnForgotPassword = DateTime.Now;
            await _userManager.UpdateAsync(user);
            await _emailConfirmation.SendEmailAsync(forgotPasswordDto.Email, "Скидування пароля",
                $"Для скидування пароля перейдіть за : <a href='{callbackUrl}'>посиланням</a>", "Адміністрація сайту EPlast");
        }
        public async Task<IdentityResult> ResetPassword(User user, ResetPasswordDto resetPasswordDto)
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

        public async Task<User> GetUser(ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipal);
            return user;
        }

        public async Task<IdentityResult> ChangePasswordForUser(User user, ChangePasswordDto changePasswordDto)
        {
            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, 
                changePasswordDto.NewPassword);
            return result;
        }

        public async void RefreshSignIn(User user)
        {
            await _signInManager.RefreshSignInAsync(user);
        }

        public AuthenticationProperties GetAuthenticationProperties(string provider, string returnUrl)
        {
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return properties;
        }

        public async void GoogleAuthentication(string email, User user, ExternalLoginInfo externalLoginInfo)
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
