using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using EPlast.BLL.DTO.Account;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Models;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using NLog.Extensions.Logging;

namespace EPlast.BLL.Services
{
    [Obsolete("Planned to be removed")]
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AuthService(UserManager<User> userManager,
                           SignInManager<User> signInManager,
                           IMapper mapper,
                           IRepositoryWrapper repoWrapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _repoWrapper = repoWrapper;
        }

        ///<inheritdoc/>
        public async Task<string> AddRoleAndTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            await _userManager.AddToRoleAsync(user, Roles.RegisteredUser);
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return HttpUtility.UrlEncode(code);
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
        public async Task CheckingForLocking(UserDto userDto)
        {
            if (await _userManager.IsLockedOutAsync(_mapper.Map<UserDto, User>(userDto)))
            {
                await _userManager.SetLockoutEndDateAsync(_mapper.Map<UserDto, User>(userDto), DateTimeOffset.UtcNow);
            }
        }

        public async Task<IdentityResult> CreateUserAsync(RegisterDto registerDto)
        {
            var user = new User()
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                FatherName = registerDto.FatherName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                UserName = registerDto.Email,
                RegistredOn = DateTime.Now,
                ImagePath = "default_user_image.png",
                UserProfile = new UserProfile()
                {
                    Birthday = registerDto.Birthday,
                    TwitterLink = registerDto.TwitterLink,
                    InstagramLink = registerDto.InstagramLink,
                    FacebookLink = registerDto.FacebookLink,
                    Address = registerDto.Address,
                    GenderID = registerDto.GenderId
                }
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            return result;
        }

        public async Task<UserDto> FacebookLoginAsync(FacebookUserInfo facebookUser)
        {
            var user = await _userManager.FindByEmailAsync(facebookUser.Email);
            if (user != null)
            {
                await _signInManager.SignInAsync(user, false, null);
                return _mapper.Map<User, UserDto>(user);
            }
            return null;
        }

        ///<inheritdoc/>
        public async Task<UserDto> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return _mapper.Map<User, UserDto>(user);
        }

        ///<inheritdoc/>
        public async Task<UserDto> FindByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return _mapper.Map<User, UserDto>(user);
        }

        ///<inheritdoc/>
        public async Task<string> GenerateConfToken(UserDto userDto)
        {
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(_mapper.Map<UserDto, User>(userDto));
            return code;
        }

        ///<inheritdoc/>
        public async Task<string> GenerateResetTokenAsync(UserDto userDto)
        {
            var user = _mapper.Map<UserDto, User>(userDto);
            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
            return code;
        }

        ///<inheritdoc/>
        public AuthenticationProperties GetAuthProperties(string provider, string returnUrl)
        {
            AuthenticationProperties properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, returnUrl);
            return properties;
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<AuthenticationScheme>> GetAuthSchemesAsync()
        {
            var externalLogins = await _signInManager.GetExternalAuthenticationSchemesAsync();
            return externalLogins;
        }

        ///<inheritdoc/>
        public async Task<UserDto> GetGoogleUserAsync(string providerToken)
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
            if (user != null)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return _mapper.Map<User, UserDto>(user);
            }
            return null;
        }

        ///<inheritdoc/>
        public async Task<string> GetIdForUserAsync(User user)
        {
            var currentUserId = await _userManager.GetUserIdAsync(user);
            return currentUserId;
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
        public int GetTimeAfterRegister(UserDto userDto)
        {
            return (int)(DateTime.Now - userDto.EmailSendedOnRegister).TotalMinutes;
        }

        ///<inheritdoc/>
        public int GetTimeAfterReset(UserDto userDto)
        {
            return (int)(DateTime.Now - userDto.EmailSendedOnForgotPassword).TotalMinutes;
        }

        ///<inheritdoc/>
        public UserDto GetUser(User user)
        {
            return _mapper.Map<User, UserDto>(user);
        }

        ///<inheritdoc/>
        public async Task<bool> IsEmailConfirmedAsync(UserDto userDto)
        {
            bool result = await _userManager.IsEmailConfirmedAsync(_mapper.Map<UserDto, User>(userDto));
            return result;
        }

        ///<inheritdoc/>
        public async Task<bool> RefreshSignInAsync(UserDto userDto)
        {
            try
            {
                await _signInManager.RefreshSignInAsync(_mapper.Map<UserDto, User>(userDto));
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        ///<inheritdoc/>
        public async Task<IdentityResult> ResetPasswordAsync(string userId, ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Code, resetPasswordDto.Password);
            return result;
        }

        ///<inheritdoc/>
        public async Task<SignInResult> SignInAsync(LoginDto loginDto)
        {
            var user = _userManager.FindByEmailAsync(loginDto.Email);
            var result = await _signInManager.PasswordSignInAsync(user.Result, loginDto.Password, loginDto.RememberMe, true);
            return result;
        }

        /// <inheritdoc />
        public async Task DeleteUserIfEmailNotConfirmedAsync()
        {
            var users = await _repoWrapper.User.GetAllAsync(x => !x.EmailConfirmed);
            foreach (var user in users)
            {
                if ((DateTime.Now - user.RegistredOn).TotalHours <= 12) continue;
                _repoWrapper.User.Delete(user);
                await _repoWrapper.SaveAsync();
            }
        }

        ///<inheritdoc/>
        public async void SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
