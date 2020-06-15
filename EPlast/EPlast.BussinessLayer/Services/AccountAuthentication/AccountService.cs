using AutoMapper;
using EPlast.BussinessLayer.DTO.Account;
using EPlast.BussinessLayer.DTO.UserProfiles;
using EPlast.BussinessLayer.Interfaces;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EPlast.BussinessLayer.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailConfirmation _emailConfirmation;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;


        public AccountService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailConfirmation emailConfirmation,
            IMapper mapper,
            IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailConfirmation = emailConfirmation;
            _mapper = mapper;
            _config = config;
        }

        public async Task<SignInResult> SignInAsync(LoginDTO loginDto)
        {
            var user = _userManager.FindByEmailAsync(loginDto.Email);
            var result = await _signInManager.PasswordSignInAsync(user.Result, loginDto.Password, loginDto.RememberMe, true);
            return result;
        }

        public async void SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> CreateUserAsync(RegisterDTO registerDto)
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

        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string code) 
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return result;
        }

        public async Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordDTO changePasswordDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword,
                changePasswordDto.NewPassword);
            return result;
        }

        public async void RefreshSignInAsync(UserDTO userDto)
        {
            await _signInManager.RefreshSignInAsync(_mapper.Map<UserDTO, User>(userDto));
        }

        public AuthenticationProperties GetAuthProperties(string provider, string returnUrl)
        {
            AuthenticationProperties properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, returnUrl);
            return properties;
        }

        public async Task<ExternalLoginInfo> GetInfoAsync()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            return info;
        }

        public async Task<SignInResult> GetSignInResultAsync(ExternalLoginInfo externalLoginInfo)
        {
            SignInResult signInResult = await _signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider,
                    externalLoginInfo.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            return signInResult;
        }

        public async Task<bool> IsEmailConfirmedAsync(UserDTO userDto)
        {
            bool result = await _userManager.IsEmailConfirmedAsync(_mapper.Map<UserDTO, User>(userDto));
            return result;
        }

        public async Task<string> AddRoleAndTokenAsync(RegisterDTO registerDto) 
        {
            var user = await _userManager.FindByEmailAsync(registerDto.Email);
            await _userManager.AddToRoleAsync(user, "Прихильник");
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return code;
        }

        public async Task<string> GenerateConfToken(UserDTO userDto)
        {
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(_mapper.Map<UserDTO, User>(userDto));
            return code;
        }

        public async Task<string> GenerateResetTokenAsync(UserDTO userDto)
        {
            var user = _mapper.Map<UserDTO, User>(userDto);
            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
            return code;
        }

        public async Task<IdentityResult> ResetPasswordAsync(string userId, ResetPasswordDTO resetPasswordDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ResetPasswordAsync(user, HttpUtility.UrlDecode(resetPasswordDto.Code), resetPasswordDto.Password);
            return result;
        }

        public async Task CheckingForLocking(UserDTO userDto)
        {
            if (await _userManager.IsLockedOutAsync(_mapper.Map<UserDTO, User>(userDto)))
            {
                await _userManager.SetLockoutEndDateAsync(_mapper.Map<UserDTO, User>(userDto), DateTimeOffset.UtcNow);
            }
        }

        public async Task<IEnumerable<AuthenticationScheme>> GetAuthSchemesAsync()
        {
            var externalLogins = await _signInManager.GetExternalAuthenticationSchemesAsync();
            return externalLogins;
        }

        public async Task<UserDTO> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return _mapper.Map<User, UserDTO>(user);
        }

        public async Task<UserDTO> FindByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return _mapper.Map<User, UserDTO>(user);
        }

        public string GetIdForUser(ClaimsPrincipal claimsPrincipal)
        {
            var currentUserId = _userManager.GetUserId(claimsPrincipal);
            return currentUserId;
        }

        public int GetTimeAfterRegistr(UserDTO userDto)
        {
            IDateTimeHelper dateTimeConfirming = new DateTimeHelper();
            var user = _mapper.Map<UserDTO, User>(userDto);
            int totalTime = (int)dateTimeConfirming.GetCurrentTime().Subtract(user.EmailSendedOnRegister).TotalMinutes;
            return totalTime;
        }

        public int GetTimeAfterReset(UserDTO userDto)
        {
            IDateTimeHelper dateTimeResetingPassword = new DateTimeHelper();
            dateTimeResetingPassword.GetCurrentTime();
            int totalTime = (int)dateTimeResetingPassword.GetCurrentTime().Subtract(userDto.EmailSendedOnForgotPassword).TotalMinutes;
            return totalTime;
        }

        public async Task<UserDTO> GetUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipal);
            return _mapper.Map<User, UserDTO>(user);
        }

        public async Task SendEmailRegistr(string confirmationLink, UserDTO userDto)
        {
            var user = await _userManager.FindByIdAsync(userDto.Id);
            user.EmailSendedOnRegister = DateTime.Now;
            await _userManager.UpdateAsync(user); 
            await _emailConfirmation.SendEmailAsync(user.Email, "Підтвердження реєстрації ",
                $"Підтвердіть реєстрацію, перейшовши за :  <a href='{confirmationLink}'>посиланням</a> ", "Адміністрація сайту EPlast");
        }

        public async Task SendEmailRegistr(string confirmationLink, RegisterDTO registerDto)
        {
            var user = await _userManager.FindByEmailAsync(registerDto.Email);
            user.EmailSendedOnRegister = DateTime.Now;
            await _userManager.UpdateAsync(user);  
            await _emailConfirmation.SendEmailAsync(user.Email, "Підтвердження реєстрації ",
                $"Підтвердіть реєстрацію, перейшовши за :  <a href='{confirmationLink}'>посиланням</a> ", "Адміністрація сайту EPlast");
        }

        public async Task SendEmailReseting(string confirmationLink, ForgotPasswordDTO forgotPasswordDto) 
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            user.EmailSendedOnForgotPassword = DateTime.Now;
            await _userManager.UpdateAsync(user); 
            await _emailConfirmation.SendEmailAsync(forgotPasswordDto.Email, "Скидування пароля",
                $"Для скидування пароля перейдіть за : <a href='{confirmationLink}'>посиланням</a>", "Адміністрація сайту EPlast");
        }

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

        private string GenerateJSONWebToken(LoginDTO userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
