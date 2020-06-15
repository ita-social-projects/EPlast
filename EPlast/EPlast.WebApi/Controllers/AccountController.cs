using AutoMapper;
using EPlast.BussinessLayer.DTO.Account;
using EPlast.BussinessLayer.Interfaces;
using EPlast.BussinessLayer.Interfaces.UserProfiles;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.Resources;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
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
            IStringLocalizer<AuthenticationErrors> resourceForErrors)
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
        }

        [HttpGet("signin")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            try
            {
                LoginDto loginDto = new LoginDto
                {
                    ReturnUrl = returnUrl,
                    ExternalLogins = (await _accountService.GetAuthSchemesAsync()).ToList()
                };
                return Ok(loginDto);
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [HttpPost("signin")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto loginDto, string returnUrl)
        {
            try
            {
                loginDto.ReturnUrl = returnUrl;
                loginDto.ExternalLogins = (await _accountService.GetAuthSchemesAsync()).ToList();

                if (ModelState.IsValid)
                {
                    var user = await _accountService.FindByEmailAsync(loginDto.Email);
                    if (user == null)
                    {
                        ModelState.AddModelError("", _resourceForErrors["Login-NotRegistered"]);
                        return Ok(loginDto);
                    }
                    else
                    {
                        if (!await _accountService.IsEmailConfirmedAsync(user))
                        {
                            ModelState.AddModelError("", _resourceForErrors["Login-NotConfirmed"]);
                            return Ok(loginDto);
                        }
                    }
                    var result = await _accountService.SignInAsync(loginDto);
                    if (result.IsLockedOut)
                    {
                        return RedirectToAction("AccountLocked", "Account");
                    }
                    if (result.Succeeded)
                    {
                        return RedirectToAction("UserProfile", "Account");
                    }
                    else
                    {
                        ModelState.AddModelError("", _resourceForErrors["Login-InCorrectPassword"]);
                        return Ok(loginDto);
                    }
                }
                //return View("Login", loginVM);
                return Ok(loginDto);
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [HttpGet("signup")]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return Ok("signup");
        }

        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", _resourceForErrors["Register-InCorrectData"]);
                    return Ok("Register");
                }

                var registeredUser = await _accountService.FindByEmailAsync(registerDto.Email);
                if (registeredUser != null)
                {
                    ModelState.AddModelError("", _resourceForErrors["Register-RegisteredUser"]);
                    return Ok("Register");
                }
                else
                {
                    var result = await _accountService.CreateUserAsync(registerDto);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", _resourceForErrors["Register-InCorrectPassword"]);
                        return Ok("Register");
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
                      return Ok("AcceptingEmail");
                    }
                }
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [HttpGet("confirmingEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmingEmail(string userId, string code)
        {
            var userDto = await _accountService.FindByIdAsync(userId);
            if (userDto == null)
            {
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
            int totalTime = _accountService.GetTimeAfterRegistr(userDto);
            if (totalTime < 180)
            {
                if (string.IsNullOrWhiteSpace(userId) && string.IsNullOrWhiteSpace(code))
                {
                    return RedirectToAction("HandleError", "Error", new { code = 500 });
                }

                var result = await _accountService.ConfirmEmailAsync(userDto.Id, code);

                if (result.Succeeded)
                {
                    return Ok("ConfirmedEmail");
                }
                else
                {
                    return RedirectToAction("HandleError", "Error", new { code = 500 });
                }
            }
            else
            {
                //return Ok("ConfirmEmailNotAllowed", userDto);
                return Ok("ConfirmedEmailNotAllowed");
            }
        }

        [HttpGet("confirmedEmail")]
        public IActionResult ConfirmedEmail()
        {
            return Ok("ConfirmedEmail");
        }

        [HttpGet("resendEmailForRegistering")]
        [AllowAnonymous]
        public async Task<IActionResult> ResendEmailForRegistering(string userId)
        {
            var userDto = await _accountService.FindByIdAsync(userId);
            if (userDto == null)
            {
                return RedirectToAction("HandleError", "Error", new { code = 500 });
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

        [HttpGet("accountLocked")]
        [AllowAnonymous]
        public IActionResult AccountLocked()
        {
            return Ok("AccountLocked");
        }

        [HttpPost("logout")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Logout()
        {
            _accountService.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet("forgotPassword")]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return Ok("ForgotPassword");
        }

        [HttpPost("forgotPassword")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotpasswordDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userDto = await _accountService.FindByEmailAsync(forgotpasswordDto.Email);
                    if (userDto == null || !(await _accountService.IsEmailConfirmedAsync(userDto)))
                    {
                        ModelState.AddModelError("", _resourceForErrors["Forgot-NotRegisteredUser"]);
                        return Ok("ForgotPassword");
                    }
                    string code = await _accountService.GenerateResetTokenAsync(userDto);
                    string confirmationLink = Url.Action(
                        nameof(ResetPassword),
                        "Account",
                        new { userId = userDto.Id, code = HttpUtility.UrlEncode(code) },
                        protocol: HttpContext.Request.Scheme);
                    await _accountService.SendEmailReseting(confirmationLink, forgotpasswordDto);
                    return Ok("ForgotPasswordConfirmation");
                }
                return Ok("ForgotPassword");
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [HttpGet("resetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string userId, string code = null)
        {
            var userDto = await _accountService.FindByIdAsync(userId);
            if (userDto == null)
            {
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
            int totalTime = _accountService.GetTimeAfterReset(userDto);
            if (totalTime < 180)
            {
                if (string.IsNullOrWhiteSpace(code))
                {
                    return RedirectToAction("HandleError", "Error", new { code = 500 });
                }
                else
                {
                    return Ok("ResetPassword");
                }
            }
            else
            {
                //return Ok("ResetPasswordNotAllowed", userDto);
                return Ok("ResetPasswordNotAllowed");
            }
        }

        [HttpPost("resetPassword")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetpasswordDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok("ResetPassword");
                }
                var userDto = await _accountService.FindByEmailAsync(resetpasswordDto.Email);
                if (userDto == null)
                {
                    ModelState.AddModelError("", _resourceForErrors["Reset-NotRegisteredUser"]);
                    return Ok("ResetPassword");
                }
                var result = await _accountService.ResetPasswordAsync(userDto.Id, resetpasswordDto);
                if (result.Succeeded)
                {
                    await _accountService.CheckingForLocking(userDto);
                    return Ok("ResetPasswordConfirmation");
                }
                else
                {
                    ModelState.AddModelError("", _resourceForErrors["Reset-PasswordProblems"]);
                    return Ok("ResetPassword");
                }
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [HttpGet("changePassword")]
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changepasswordDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userDto = await _accountService.GetUserAsync(User);
                    if (userDto == null)
                    {
                        return RedirectToAction("Login");
                    }
                    var result = await _accountService.ChangePasswordAsync(userDto.Id, changepasswordDto);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", _resourceForErrors["Change-PasswordProblems"]);
                        return Ok("ChangePassword");
                    }
                    _accountService.RefreshSignInAsync(userDto);
                    return Ok("ChangePasswordConfirmation");
                }
                else
                {
                    return Ok("ChangePassword");
                }
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        
        [HttpPost("externalLogin")]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            string redirectUrl = Url.Action("ExternalLoginCallBack",
                "Account",
                new { ReturnUrl = returnUrl });
            AuthenticationProperties properties = _accountService.GetAuthProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        [HttpGet("externalLoginCallBack")]
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
                    ModelState.AddModelError("", _resourceForErrors["Error-ExternalLoginProvider"]);
                    //return View("Login", loginDto);
                    return Ok("Login");
                }
                var info = await _accountService.GetInfoAsync();
                if (info == null)
                {
                    ModelState.AddModelError(string.Empty, _resourceForErrors["Error-ExternalLoginInfo"]);
                    //return View("Login", loginDto);
                    return Ok("Login");
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
                    return Ok("Error");
                }
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }
    }
}