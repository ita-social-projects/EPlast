using AutoMapper;
using EPlast.BLL.DTO.Account;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using EPlast.BLL.Models;
using Newtonsoft.Json;
using NLog.Fluent;

namespace EPlast.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailConfirmation _emailConfirmation;
        private readonly IMapper _mapper;

        public AuthService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailConfirmation emailConfirmation,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailConfirmation = emailConfirmation;
            _mapper = mapper;
        }

        ///<inheritdoc/>
        public async Task<SignInResult> SignInAsync(LoginDto loginDto)
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
        public async Task<SignInResult> GetSignInResultAsync(ExternalLoginInfo externalLoginInfo)
        {
            SignInResult signInResult = await _signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider,
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
        public async Task<string> AddRoleAndTokenAsync(RegisterDto registerDto) 
        {
            var user = await _userManager.FindByEmailAsync(registerDto.Email);
            await _userManager.AddToRoleAsync(user, "Прихильник");
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
        public string GetIdForUser(ClaimsPrincipal claimsPrincipal)
        {
            var currentUserId = _userManager.GetUserId(claimsPrincipal);
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
        public async Task<UserDTO> GetUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipal);
            return _mapper.Map<User, UserDTO>(user);
        }

        ///<inheritdoc/>
        public async Task SendEmailRegistr(string confirmationLink, UserDTO userDto)
        {
            var user = await _userManager.FindByIdAsync(userDto.Id);
            user.EmailSendedOnRegister = DateTime.Now;
            await _userManager.UpdateAsync(user);
            await _emailConfirmation.SendEmailAsync(user.Email, "Підтвердження реєстрації ",
                $"Підтвердіть реєстрацію, перейшовши за :  <a href='{confirmationLink}'>посиланням</a> ", "Адміністрація сайту EPlast");
        }

        ///<inheritdoc/>
        public async Task SendEmailRegistr(string confirmationLink, RegisterDto registerDto)
        {
            var user = await _userManager.FindByEmailAsync(registerDto.Email);
            user.EmailSendedOnRegister = DateTime.Now;
            await _userManager.UpdateAsync(user);  
            await _emailConfirmation.SendEmailAsync(user.Email, "Підтвердження реєстрації ",
                $"Підтвердіть реєстрацію, перейшовши за :  <a href='{confirmationLink}'>посиланням</a> ", "Адміністрація сайту EPlast");
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
        public async Task GetGoogleUserInfoAsync(string providerToken)
        {
            string GoogleApiTokenInfoUrl = "https://www.googleapis.com/oauth2/v3/tokeninfo?id_token={0}";
            var httpClient = new HttpClient();
            var requestUri = new Uri(string.Format(GoogleApiTokenInfoUrl, providerToken));

            HttpResponseMessage httpResponseMessage;
            try
            {
                httpResponseMessage = httpClient.GetAsync(requestUri).Result;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }

            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception();
            }

            var response = httpResponseMessage.Content.ReadAsStringAsync().Result;
            var googleApiTokenInfo = JsonConvert.DeserializeObject<GoogleApiTokenInfo>(response);

            //if (await _userManager.FindByEmailAsync(googleApiTokenInfo.aud)!=null)
            //{
            //    Log.WarnFormat("Google API Token Info aud field ({0}) not containing the required client id", googleApiTokenInfo.aud);
            //    return null;
            //}
            var user = await _userManager.FindByEmailAsync(googleApiTokenInfo.email);
            if (user == null)
            {
                user = new User
                {
                    UserName = googleApiTokenInfo.name,
                    Email = googleApiTokenInfo.email,
                    FirstName = googleApiTokenInfo.given_name,
                    LastName = googleApiTokenInfo.family_name,
                    SocialNetworking = true,
                    ImagePath = googleApiTokenInfo.picture,
                    EmailConfirmed = true,
                    RegistredOn = DateTime.Now,
                    UserProfile = new UserProfile()
                };
                await _userManager.CreateAsync(user);
                await _emailConfirmation.SendEmailAsync(user.Email, "Повідомлення про реєстрацію",
                    "Ви зареєструвались в системі EPlast використовуючи свій Google-акаунт ", "Адміністрація сайту EPlast");
            }
            await _userManager.AddToRoleAsync(user, "Прихильник");
            await _userManager.AddLoginAsync(user, new UserLoginInfo( user.Email, Guid.NewGuid().ToString(),user.UserName));
            await _signInManager.SignInAsync(user, isPersistent: false);
      
        }
        ///<inheritdoc/>
        public async Task GoogleAuthentication(string email, ExternalLoginInfo externalLoginInfo)
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

        ///<inheritdoc/>
        public async Task FacebookAuthentication(string email, ExternalLoginInfo externalLoginInfo)
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
