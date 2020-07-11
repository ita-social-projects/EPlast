using AutoMapper;
using EPlast.BLL.DTO.Account;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Services.Interfaces;
using EPlast.BLL.Services.Jwt;
using EPlast.Resources;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly INationalityService _nationalityService;
        private readonly IEducationService _educationService;
        private readonly IReligionService _religionService;
        private readonly IWorkService _workService;
        private readonly IGenderService _genderService;
        private readonly IDegreeService _degreeService;
        private readonly IUserManagerService _userManagerService;
        private readonly IConfirmedUsersService _confirmedUserService;
        private readonly ILoggerService<AccountController> _loggerService;
        private readonly IStringLocalizer<AuthenticationErrors> _resourceForErrors;
        private readonly JwtOptions _jwtOptions;

        public AccountController(IUserService userService,
            INationalityService nationalityService,
            IEducationService educationService,
            IReligionService religionService,
            IWorkService workService,
            IGenderService genderService,
            IDegreeService degreeService,
            IConfirmedUsersService confirmedUserService,
            IUserManagerService userManagerService,
            IMapper mapper,
            ILoggerService<AccountController> loggerService,
            IAccountService accountService,
            IStringLocalizer<AuthenticationErrors> resourceForErrors,
            IOptions<JwtOptions> jwtOptions)
        {
            _accountService = accountService;
            _userService = userService;
            _nationalityService = nationalityService;
            _religionService = religionService;
            _degreeService = degreeService;
            _workService = workService;
            _educationService = educationService;
            _genderService = genderService;
            _confirmedUserService = confirmedUserService;
            _mapper = mapper;
            _userManagerService = userManagerService;
            _loggerService = loggerService;
            _resourceForErrors = resourceForErrors;
            _jwtOptions = jwtOptions.Value;
        }

        [HttpPost("signin")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _accountService.FindByEmailAsync(loginDto.Email);
                    if (user == null)
                    {
                        return BadRequest(_resourceForErrors["Login-NotRegistered"]);
                    }
                    else
                    {
                        if (!await _accountService.IsEmailConfirmedAsync(user))
                        {
                            return BadRequest(_resourceForErrors["Login-NotConfirmed"]);
                        }
                    }
                    var result = await _accountService.SignInAsync(loginDto);
                    if (result.IsLockedOut)
                    {
                        return BadRequest(_resourceForErrors["Account-Locked"]);
                    }
                    if (result.Succeeded)                                //винести в сервіс
                    {
                        var claims = new[] {
                             new Claim(ClaimTypes.Name, user.Email),
                             new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                             new Claim(JwtRegisteredClaimNames.FamilyName, user.Id),
                             new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                         };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.key));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                          issuer: _jwtOptions.issuer,
                          audience: _jwtOptions.issuer,
                          claims: claims,
                          expires: DateTime.Now.AddMinutes(30),  //тут добавити 2 години
                          signingCredentials: creds);

                        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
                    }
                    else
                    {
                        return BadRequest(_resourceForErrors["Login-InCorrectPassword"]);
                    }
                }
                return Ok();
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return BadRequest();
            }
        }

        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]RegisterDto registerDto) //+
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(_resourceForErrors["Register-InCorrectData"]);
                }
                var registeredUser = await _accountService.FindByEmailAsync(registerDto.Email);
                if (registeredUser != null)
                {
                    return BadRequest(_resourceForErrors["Register-RegisteredUser"]);
                }
                else
                {
                    var result = await _accountService.CreateUserAsync(registerDto);
                    if (!result.Succeeded)
                    {
                        return BadRequest(_resourceForErrors["Register-InCorrectPassword"]);
                    }
                    else
                    {
                        string code = await _accountService.AddRoleAndTokenAsync(registerDto);
                        var userDto = await _accountService.FindByEmailAsync(registerDto.Email);
                        string confirmationLink = Url.Action(
                            nameof(ConfirmingEmail),
                            "Account",
                            new { code = code, userId = userDto.Id },
                              protocol: HttpContext.Request.Scheme);
                        await _accountService.SendEmailRegistr(confirmationLink, userDto);
            
                        return Ok(_resourceForErrors["Confirm-Registration"]);
                    }
                }
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return BadRequest();
            }
        }

        [HttpGet("confirmingEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmingEmail(string userId, string code)
        {
            var userDto = await _accountService.FindByIdAsync(userId);
            if (userDto == null)
            {
                return BadRequest();
            }
            int totalTime = _accountService.GetTimeAfterRegistr(userDto);
            if (totalTime < 180)
            {
                if (string.IsNullOrWhiteSpace(userId) && string.IsNullOrWhiteSpace(code))
                {
                    return BadRequest();
                }
                var result = await _accountService.ConfirmEmailAsync(userDto.Id, code);
           
                if (result.Succeeded) 
                {
                    return Ok("ConfirmedEmail");
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                //return View("ConfirmEmailNotAllowed", userDto);
                return Ok("ConfirmedEmailNotAllowed");
            }
        }

        /*[HttpGet("confirmedEmail")]   тут будемо просто слати повідомлення що підтвердив
        public IActionResult ConfirmedEmail()
        {
            return Ok("ConfirmedEmail");
        }*/

        [HttpGet("resendEmailForRegistering")]
        [AllowAnonymous]
        public async Task<IActionResult> ResendEmailForRegistering(string userId)
        {
            var userDto = await _accountService.FindByIdAsync(userId);
            if (userDto == null)
            {
                return BadRequest();
            }
            string code = await _accountService.GenerateConfToken(userDto);
            var confirmationLink = Url.Action(
                nameof(ConfirmingEmail),
                "Account",
                new { code = code, userId = userDto.Id },
                protocol: HttpContext.Request.Scheme);
            await _accountService.SendEmailRegistr(confirmationLink, userDto);
            
            return Ok("ResendEmailConfirmation");
        }

        /*[HttpGet("accountLocked")]     тут також будемо слати повідомлення що підтвердив
        [AllowAnonymous]
        public IActionResult AccountLocked()
        {
            return Ok("AccountLocked");
        }*/

        [HttpGet("logout")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Logout()
        {
            _accountService.SignOutAsync();
            return Ok();
        }

        [HttpPost("forgotPassword")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotpasswordDto)//+
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userDto = await _accountService.FindByEmailAsync(forgotpasswordDto.Email);
                    if (userDto == null || !(await _accountService.IsEmailConfirmedAsync(userDto)))
                    {
                        return BadRequest(_resourceForErrors["Forgot-NotRegisteredUser"]);
                    }
                    string code = await _accountService.GenerateResetTokenAsync(userDto);
                    string confirmationLink = Url.Action(
                        nameof(ResetPassword),
                        "Account",
                        new { userId = userDto.Id, code = HttpUtility.UrlEncode(code) },
                        protocol: HttpContext.Request.Scheme);
                    await _accountService.SendEmailReseting(confirmationLink, forgotpasswordDto);
                    return Ok(_resourceForErrors["ForgotPasswordConfirmation"]);
                }
                return Ok("ForgotPassword"); //тут ше подивитись шо іменно вертати
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return BadRequest();
            }
        }

        [HttpGet("resetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string userId, string code = null)
        {
            var userDto = await _accountService.FindByIdAsync(userId);
            if (userDto == null)
            {
                return BadRequest();
            }
            int totalTime = _accountService.GetTimeAfterReset(userDto);
            if (totalTime < 180)
            {
                if (string.IsNullOrWhiteSpace(code))
                {
                    return BadRequest();
                }
                else
                {
                    return Ok("ResetPassword");
                }
            }
            else
            {
                //return View("ResetPasswordNotAllowed", userDto);
                return Ok("ResetPasswordNotAllowed");
            }
        }

        [HttpPost("resetPassword")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetpasswordDto) //+
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(_resourceForErrors["ModelIsNotValid"]);
                }
                var userDto = await _accountService.FindByEmailAsync(resetpasswordDto.Email);
                if (userDto == null)
                {
                    return BadRequest(_resourceForErrors["Reset-NotRegisteredUser"]);
                }
                var result = await _accountService.ResetPasswordAsync(userDto.Id, resetpasswordDto);
                if (result.Succeeded)
                {
                    await _accountService.CheckingForLocking(userDto);
                    return Ok(_resourceForErrors["ResetPasswordConfirmation"]);
                }
                else
                {
                    return BadRequest(_resourceForErrors["Reset-PasswordProblems"]);
                }
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return BadRequest();
            }
        }

        [HttpGet("changePassword")]
        //[Authorize]
        public async Task<IActionResult> ChangePassword()
        {
            var userDto = await _accountService.GetUserAsync(User);
            var result = userDto.SocialNetworking;
            if (result != true)
            {
                return Ok("ChangePassword");
            }
            else
            {
                return Ok("ChangePasswordNotAllowed");
            }
        }

        [HttpPost("changePassword")]
        //[Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changepasswordDto)//+ приходить все норм
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userDto = await _accountService.GetUserAsync(User);  // перше треба зробити логін
                    if (userDto == null)
                    {
                        return BadRequest(); // тут просто вийдіть з сайту
                    }
                    var result = await _accountService.ChangePasswordAsync(userDto.Id, changepasswordDto);
                    if (!result.Succeeded)
                    {
                        return BadRequest(_resourceForErrors["Change-PasswordProblems"]);
                    }
                    _accountService.RefreshSignInAsync(userDto);
                    return Ok(_resourceForErrors["ChangePasswordConfirmation"]);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return BadRequest();
            }
        }

        /*[HttpPost("externalLogin")]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            string redirectUrl = Url.Action("ExternalLoginCallBack",
                "Account",
                new { ReturnUrl = returnUrl });
            AuthenticationProperties properties = _accountService.GetAuthProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }*/

        /*[HttpGet("externalLoginCallBack")]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallBack(string returnUrl = null, string remoteError = null)
        {
            try
            {
                returnUrl = returnUrl ?? Url.Content("~/Account/UserProfile");
                LoginDto loginDto = new LoginDto
                {
                    ReturnUrl = returnUrl,
                    ExternalLogins = (await _accountService.GetAuthSchemesAsync()).ToList()
                };

                if (remoteError != null)
                {
                    return BadRequest(_resourceForErrors["Error-ExternalLoginProvider"]);
                }
                var info = await _accountService.GetInfoAsync();
                if (info == null)
                {
                    return BadRequest(_resourceForErrors["Error-ExternalLoginInfo"]);
                }

                var signInResult = await _accountService.GetSignInResultAsync(info);
                if (signInResult.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    var email = "defaultEmail";
                        //info.Principal.FindFirstValue(ClaimTypes.Email);
                    if (info.LoginProvider.ToString() == "Google")
                    {
                        if (email != null)
                        {
                            await _accountService.GoogleAuthentication(email, info);
                            return LocalRedirect(returnUrl);
                        }
                    }
                    else if (info.LoginProvider.ToString() == "Facebook")
                    {
                        await _accountService.FacebookAuthentication(email, info);
                        return LocalRedirect(returnUrl);
                    }
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return BadRequest();
            }
        }*/
    }
}