using AutoMapper;
using EPlast.BLL.DTO.Account;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.Jwt;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Services.Interfaces;
using EPlast.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
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
        private readonly IJwtService _jwtService;
        private readonly IHomeService _homeService;

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
            IJwtService jwtService,
            IHomeService homeService)
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
            _jwtService = jwtService;
            _homeService = homeService;
        }

        /// <summary>
        /// Method for logining in system
        /// </summary>
        /// <param name="loginDto">Login model(dto)</param>
        /// <returns>Answer from backend for login method</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Problems with logining</response>
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
                    if (result.Succeeded)
                    {
                        var generatedToken = _jwtService.GenerateJWTToken(user);
                        return Ok(new { token = generatedToken });
                    }
                    else
                    {
                        return BadRequest(_resourceForErrors["Login-InCorrectPassword"]);
                    }
                }
                return Ok(_resourceForErrors["ModelIsNotValid"]);
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return BadRequest();
            }
        }

        /// <summary>
        /// Method for registering in system
        /// </summary>
        /// <param name="registerDto">Register model(dto)</param>
        /// <returns>Answer from backend for register method</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Problems with registration</response>
        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]RegisterDto registerDto)
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
                        string token = await _accountService.AddRoleAndTokenAsync(registerDto);
                        var userDto = await _accountService.FindByEmailAsync(registerDto.Email);
                        string confirmationLink = Url.Action(
                            nameof(ConfirmingEmail),
                            "Account",
                            new { token = token, userId = userDto.Id },
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

        /// <summary>
        /// Method for confirming email in system
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <param name="token">Token for confirming email</param>
        /// <returns>Answer from backend for confirming email method</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Problems with confirming email</response>
        [HttpGet("confirmingEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmingEmail(string userId, string token) //+
        {
            var userDto = await _accountService.FindByIdAsync(userId);
            if (userDto == null)
            {
                return BadRequest();
            }
            int totalTime = _accountService.GetTimeAfterRegistr(userDto);
            if (totalTime < 180)
            {
                if (string.IsNullOrWhiteSpace(userId) && string.IsNullOrWhiteSpace(token))
                {
                    return BadRequest();
                }
                var result = await _accountService.ConfirmEmailAsync(userDto.Id, token);
           
                if (result.Succeeded) 
                {
                    return Redirect("https://plastua.azurewebsites.net/");
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return Ok(_resourceForErrors["ConfirmedEmailNotAllowed"]);  
            }
        }

        /// <summary>
        /// Method for resending email in system
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <returns>Answer from backend for resending email method</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Problems with resending email</response>
        [HttpGet("resendEmailForRegistering")]
        [AllowAnonymous]
        public async Task<IActionResult> ResendEmailForRegistering(string userId)
        {
            var userDto = await _accountService.FindByIdAsync(userId);
            if (userDto == null)
            {
                return BadRequest();
            }
            string token = await _accountService.GenerateConfToken(userDto);
            var confirmationLink = Url.Action(
                nameof(ConfirmingEmail),
                "Account",
                new { token = token, userId = userDto.Id },
                protocol: HttpContext.Request.Scheme);
            await _accountService.SendEmailRegistr(confirmationLink, userDto);
            
            return Ok("ResendEmailConfirmation");
        }

        [HttpGet("logout")] //+
        //[ValidateAntiForgeryToken]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult Logout()
        {
            _accountService.SignOutAsync();
            return Ok();
        }

        /// <summary>
        /// Method for forgotting password in system
        /// </summary>
        /// <param name="forgotpasswordDto">Forgot Password model(dto)</param>
        /// <returns>Answer from backend for forgoting password email method</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Problems with forgotting password</response>
        [HttpPost("forgotPassword")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotpasswordDto)
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
                    string token = await _accountService.GenerateResetTokenAsync(userDto);
                    string confirmationLink = Url.Action(
                        nameof(ResetPassword),
                        "Account",
                        new { userId = userDto.Id, token = HttpUtility.UrlEncode(token) },
                        protocol: HttpContext.Request.Scheme);
                    await _accountService.SendEmailReseting(confirmationLink, forgotpasswordDto);
                    return Ok(_resourceForErrors["ForgotPasswordConfirmation"]);
                }
                return BadRequest(_resourceForErrors["ModelIsNotValid"]);
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return BadRequest();
            }
        }

        /// <summary>
        /// Method for resetting password in system
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <param name="token">Token for reseting password</param>
        /// <returns>Answer from backend for resetting password method</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Problems with resetting password</response>
        [HttpGet("resetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string userId, string token = null)
        {
            var userDto = await _accountService.FindByIdAsync(userId);
            if (userDto == null)
            {
                return BadRequest();
            }
            int totalTime = _accountService.GetTimeAfterReset(userDto);
            if (totalTime < 180)
            {
                if (string.IsNullOrWhiteSpace(token))
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
                return Ok(_resourceForErrors["ResetPasswordNotAllowed"]);
            }
        }

        /// <summary>
        /// Method for resetting password in system
        /// </summary>
        /// <param name="resetpasswordDto">ResetPassword model(dto)</param>
        /// <returns>Answer from backend for resetting password method</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Problems with resetting password</response>
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

        /// <summary>
        /// Method for resetting password in system
        /// </summary>
        /// <param name="changepasswordDto">ResetPassword model(dto)</param>
        /// <returns>Answer from backend for resetting password method</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Problems with resetting password</response>
        [HttpPost("changePassword")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changepasswordDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userDto = await _accountService.GetUserAsync(User); 
                    if (userDto == null)
                    {
                        return BadRequest(); 
                    }
                    var result = await _accountService.ChangePasswordAsync(userDto.Id, changepasswordDto);
                    if (!result.Succeeded)
                    {
                        return BadRequest(_resourceForErrors["Change-PasswordProblems"]);
                    }
                    _accountService.RefreshSignInAsync(userDto); //тут
                    return Ok(_resourceForErrors["ChangePasswordConfirmation"]);
                }
                else
                {
                    return BadRequest(_resourceForErrors["ModelIsNotValid"]);
                }
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return BadRequest();
            }
        }

        [HttpPost("sendQuestion")]
        public async Task<IActionResult> SendContacts([FromBody]ContactsDto contactsDto)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Дані введені неправильно");
                return BadRequest(_resourceForErrors["ModelIsNotValid"]);
            }
            await _homeService.SendEmailAdmin(contactsDto);

            return Ok(_resourceForErrors["Feedback-Sended"]);
        }
    }
}