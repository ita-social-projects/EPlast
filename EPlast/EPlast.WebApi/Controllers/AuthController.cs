using AutoMapper;
using EPlast.BLL.DTO.Account;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.Jwt;
using EPlast.BLL.Interfaces.Logging;
using EPlast.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using System.Web;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly ILoggerService<AuthController> _loggerService;
        private readonly IStringLocalizer<AuthenticationErrors> _resourceForErrors;
        private readonly IJwtService _jwtService;
        private readonly IHomeService _homeService;

        public AuthController(IAuthService authService,
            IMapper mapper,
            ILoggerService<AuthController> loggerService,
            IStringLocalizer<AuthenticationErrors> resourceForErrors,
            IJwtService jwtService,
            IHomeService homeService)
        {
            _authService = authService;
            _mapper = mapper;
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
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _authService.FindByEmailAsync(loginDto.Email);
                if (user == null)
                {
                    return BadRequest(_resourceForErrors["Login-NotRegistered"]);
                }
                else
                {
                    if (!await _authService.IsEmailConfirmedAsync(user))
                    {
                        return BadRequest(_resourceForErrors["Login-NotConfirmed"]);
                    }
                }
                var result = await _authService.SignInAsync(loginDto);
                if (result.IsLockedOut)
                {
                    return BadRequest(_resourceForErrors["Account-Locked"]);
                }
                if (result.Succeeded)
                {
                    var generatedToken = await _jwtService.GenerateJWTTokenAsync(user);
                    return Ok(new { token = generatedToken });
                }
                else
                {
                    return BadRequest(_resourceForErrors["Login-InCorrectPassword"]);
                }
            }
            return Ok(_resourceForErrors["ModelIsNotValid"]);
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
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(_resourceForErrors["Register-InCorrectData"]);
            }
            var registeredUser = await _authService.FindByEmailAsync(registerDto.Email);
            if (registeredUser != null)
            {
                return BadRequest(_resourceForErrors["Register-RegisteredUser"]);
            }
            else
            {
                var result = await _authService.CreateUserAsync(registerDto);
                if (!result.Succeeded)
                {
                    return BadRequest(_resourceForErrors["Register-InCorrectPassword"]);
                }
                else
                {
                    string token = await _authService.AddRoleAndTokenAsync(registerDto);
                    var userDto = await _authService.FindByEmailAsync(registerDto.Email);
                    string confirmationLink = Url.Action(
                        nameof(ConfirmingEmail),
                        "Auth",
                        new { token = token, userId = userDto.Id },
                          protocol: HttpContext.Request.Scheme);
                    await _authService.SendEmailRegistr(confirmationLink, userDto);

                    return Ok(_resourceForErrors["Confirm-Registration"]);
                }
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
            var userDto = await _authService.FindByIdAsync(userId);
            if (userDto == null)
            {
                return BadRequest();
            }
            int totalTime = _authService.GetTimeAfterRegistr(userDto);
            if (totalTime < 180)
            {
                if (string.IsNullOrWhiteSpace(userId) && string.IsNullOrWhiteSpace(token))
                {
                    return BadRequest();
                }
                var result = await _authService.ConfirmEmailAsync(userDto.Id, token);

                if (result.Succeeded)
                {
                    return Redirect("https://eplastua.azurewebsites.net/");
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
            var userDto = await _authService.FindByIdAsync(userId);
            if (userDto == null)
            {
                return BadRequest();
            }
            string token = await _authService.GenerateConfToken(userDto);
            var confirmationLink = Url.Action(
                nameof(ConfirmingEmail),
                "Auth",
                new { token = token, userId = userDto.Id },
                protocol: HttpContext.Request.Scheme);
            await _authService.SendEmailRegistr(confirmationLink, userDto);

            return Ok("ResendEmailConfirmation");
        }

        [HttpGet("logout")] //+
        //[ValidateAntiForgeryToken]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult Logout()
        {
            _authService.SignOutAsync();
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
            if (ModelState.IsValid)
            {
                var userDto = await _authService.FindByEmailAsync(forgotpasswordDto.Email);
                if (userDto == null || !(await _authService.IsEmailConfirmedAsync(userDto)))
                {
                    return BadRequest(_resourceForErrors["Forgot-NotRegisteredUser"]);
                }
                string token = await _authService.GenerateResetTokenAsync(userDto);
                var confirmationLink = string.Format("https://eplastua.azurewebsites.net?token={0}", HttpUtility.UrlEncode(token));
                await _authService.SendEmailReseting(confirmationLink, forgotpasswordDto);
                return Ok(_resourceForErrors["ForgotPasswordConfirmation"]);
            }
            return BadRequest(_resourceForErrors["ModelIsNotValid"]);
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
        public async Task<IActionResult> ResetPassword([FromQuery(Name = "userId")] string userId, [FromQuery(Name = "token")] string token)
        {
            var userDto = await _authService.FindByIdAsync(userId);
            var model = new ResetPasswordDto { Code = token, Email = userDto.Email };
            if (userDto == null)
            {
                return BadRequest();
            }
            int totalTime = _authService.GetTimeAfterReset(userDto);
            if (totalTime < 180)
            {
                if (string.IsNullOrWhiteSpace(token))
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(model);
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
            if (!ModelState.IsValid)
            {
                return BadRequest(_resourceForErrors["ModelIsNotValid"]);
            }
            var userDto = await _authService.FindByEmailAsync(resetpasswordDto.Email);
            if (userDto == null)
            {
                return BadRequest(_resourceForErrors["Reset-NotRegisteredUser"]);
            }
            var result = await _authService.ResetPasswordAsync(userDto.Id, resetpasswordDto);
            if (result.Succeeded)
            {
                await _authService.CheckingForLocking(userDto);
                return Ok(_resourceForErrors["ResetPasswordConfirmation"]);
            }
            else
            {
                return BadRequest(_resourceForErrors["Reset-PasswordProblems"]);
            }
        }

        /// <summary>
        /// Method for changing password in system
        /// </summary>
        /// <param name="changepasswordDto">ChangePassword model(dto)</param>
        /// <returns>Answer from backend for changing password method</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Problems with changing password</response>
        [HttpPost("changePassword")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changepasswordDto)
        {
            if (ModelState.IsValid)
            {
                var userDto = await _authService.GetUserAsync(User);
                if (userDto == null)
                {
                    return BadRequest();
                }
                var result = await _authService.ChangePasswordAsync(userDto.Id, changepasswordDto);
                if (!result.Succeeded)
                {
                    return BadRequest(_resourceForErrors["Change-PasswordProblems"]);
                }
                _authService.RefreshSignInAsync(userDto); //тут
                return Ok(_resourceForErrors["ChangePasswordConfirmation"]);
            }
            else
            {
                return BadRequest(_resourceForErrors["ModelIsNotValid"]);
            }
        }

        /// <summary>
        /// Method for sending question to admin in system
        /// </summary>
        /// <param name="contactsDto">Contacts model(dto)</param>
        /// <returns>Answer from backend sending question to admin in system</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Problems with sending question</response>
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