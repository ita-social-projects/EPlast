using System;
using EPlast.BLL.DTO.Account;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.ActiveMembership;
using EPlast.BLL.Interfaces.Jwt;
using EPlast.BLL.Interfaces.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Web;
using EPlast.BLL.Interfaces.Resources;
using EPlast.BLL.Models;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using NLog.Extensions.Logging;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILoggerService<AuthController> _loggerService;
        private readonly IJwtService _jwtService;
        private readonly IUserDatesService _userDatesService;
        private readonly IResources _resources;
        private readonly IAuthEmailService _authEmailServices;

        public AuthController(
            IAuthService authService,
            ILoggerService<AuthController> loggerService,
            IJwtService jwtService,
            IUserDatesService userDatesService,
            IResources resources,
            IAuthEmailService authEmailServices)
        {
            _authService = authService;
            _loggerService = loggerService;
            _jwtService = jwtService;
            _userDatesService = userDatesService;
            _resources = resources;
            _authEmailServices = authEmailServices;
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
                    return BadRequest(_resources.ResourceForErrors["Login-NotRegistered"]);
                }
                else
                {
                    if (!await _authService.IsEmailConfirmedAsync(user))
                    {
                        return BadRequest(_resources.ResourceForErrors["Login-NotConfirmed"]);
                    }
                }
                var result = await _authService.SignInAsync(loginDto);
                if (result.IsLockedOut)
                {
                    return BadRequest(_resources.ResourceForErrors["Account-Locked"]);
                }
                if (result.Succeeded)
                {
                    var generatedToken = await _jwtService.GenerateJWTTokenAsync(user);
                    return Ok(new { token = generatedToken });
                }
                else
                {
                    return BadRequest(_resources.ResourceForErrors["Login-InCorrectPassword"]);
                }
            }
            return Ok(_resources.ResourceForErrors["ModelIsNotValid"]);
        }

        [HttpGet("googleClientId")]
        [AllowAnonymous]
        public IActionResult GetGoogleClientId()
        {
            return Ok(new { id = ConfigSettingLayoutRenderer.DefaultConfiguration.GetSection("GoogleAuthentication")["GoogleClientId"]});
        }

        [HttpGet("facebookAppId")]
        [AllowAnonymous]
        public IActionResult GetFacebookAppId()
        {
            return Ok(new { id = ConfigSettingLayoutRenderer.DefaultConfiguration.GetSection("FacebookAuthentication")["FacebookAppId"] });
        }

        [HttpPost("signin/google")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleLogin(string googleToken)
        {
            try
            {
                var user = await _authService.GetGoogleUserAsync(googleToken);
                if (user == null)
                {
                    return BadRequest();
                }
                await AddEntryMembershipDate(user.Id);
                var generatedToken = await _jwtService.GenerateJWTTokenAsync(user);

                return Ok(new {token = generatedToken});
            }
            catch (Exception exc)
            {
                _loggerService.LogError(exc.Message);
            }
            
            return BadRequest();
        }

        [HttpPost("signin/facebook")]
        [AllowAnonymous]
        public async Task<IActionResult> FacebookLogin([FromBody] FacebookUserInfo userInfo)
        {
            try
            {
                string newGender = _resources.ResourceForGender[userInfo.Gender].Value;
                userInfo.Gender = newGender;
                var user = await _authService.FacebookLoginAsync(userInfo);
                if (user == null)
                {
                    return BadRequest();
                }
                await AddEntryMembershipDate(user.Id);

                var generatedToken = await _jwtService.GenerateJWTTokenAsync(user);
                return Ok(new {token = generatedToken});
            }
            catch (Exception exc)
            {
                _loggerService.LogError(exc.Message);
            }

            return BadRequest();
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
                return BadRequest(_resources.ResourceForErrors["Register-InCorrectData"]);
            }
            var registeredUser = await _authService.FindByEmailAsync(registerDto.Email);
            if (registeredUser != null)
            {
                return BadRequest(_resources.ResourceForErrors["Register-RegisteredUser"]);
            }
            else
            {
                var result = await _authService.CreateUserAsync(registerDto);
                if (!result.Succeeded)
                {
                    return BadRequest(_resources.ResourceForErrors["Register-InCorrectPassword"]);
                }
                else
                {
                    if (!(await _authEmailServices.SendEmailRegistrAsync(registerDto.Email)))
                    {
                        return BadRequest(_resources.ResourceForErrors["Register-SMTPServerError"]);
                    }
                    var userDto = await _authService.FindByEmailAsync(registerDto.Email);
                    await _userDatesService.AddDateEntryAsync(userDto.Id);
                    return Ok(_resources.ResourceForErrors["Confirm-Registration"]);
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
        public async Task<IActionResult> ConfirmingEmail(string userId, string token)
        {
            var userDto = await _authService.FindByIdAsync(userId);
            if (userDto == null)
            {
                return BadRequest();
            }
            int totalTime = _authService.GetTimeAfterRegistr(userDto);
            if (totalTime < 1440)
            {
                if (string.IsNullOrWhiteSpace(userId) && string.IsNullOrWhiteSpace(token))
                {
                    return BadRequest();
                }
                var result = await _authService.ConfirmEmailAsync(userDto.Id, token);

                if (result.Succeeded)
                {
                    string signinurl = ConfigSettingLayoutRenderer.DefaultConfiguration.GetSection("URLs")["SignIn"];
                    string citiesurl = ConfigSettingLayoutRenderer.DefaultConfiguration.GetSection("URLs")["Сities"];
                    await _authEmailServices.SendEmailReminderAsync(citiesurl, userDto);
                    return Redirect(signinurl);
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return Ok(_resources.ResourceForErrors["ConfirmedEmailNotAllowed"]);
            }
        }

        /// <summary>
        /// Method for resending email after SMTPServer error
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <returns>Answer from backend for resending email method</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Problems with resending email</response>
        [HttpGet("resendEmailForRegistering/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> ResendEmailForRegistering(string userId)
        {
            var userDto = await _authService.FindByIdAsync(userId);
            if (userDto == null)
            {
                return BadRequest();
            }
            await _authEmailServices.SendEmailRegistrAsync(userDto.Email);
            return Ok("ResendEmailConfirmation");
        }

        [HttpGet("logout")]
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
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotpasswordDto)
        {
            if (ModelState.IsValid)
            {
                var userDto = await _authService.FindByEmailAsync(forgotpasswordDto.Email);
                if (userDto == null || !(await _authService.IsEmailConfirmedAsync(userDto)))
                {
                    return BadRequest(_resources.ResourceForErrors["Forgot-NotRegisteredUser"]);
                }
                string token = await _authService.GenerateResetTokenAsync(userDto);
                var confirmationLink = string.Format("https://eplast.westeurope.cloudapp.azure.com/resetPassword?token={0}", HttpUtility.UrlEncode(token));
                await _authEmailServices.SendEmailResetingAsync(confirmationLink, forgotpasswordDto);
                return Ok(_resources.ResourceForErrors["ForgotPasswordConfirmation"]);
            }
            return BadRequest(_resources.ResourceForErrors["ModelIsNotValid"]);
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
                return Ok(_resources.ResourceForErrors["ResetPasswordNotAllowed"]);
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
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetpasswordDto) //+
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(_resources.ResourceForErrors["ModelIsNotValid"]);
            }
            var userDto = await _authService.FindByEmailAsync(resetpasswordDto.Email);
            if (userDto == null)
            {
                return BadRequest(_resources.ResourceForErrors["Reset-NotRegisteredUser"]);
            }
            var result = await _authService.ResetPasswordAsync(userDto.Id, resetpasswordDto);
            if (result.Succeeded)
            {
                await _authService.CheckingForLocking(userDto);
                return Ok(_resources.ResourceForErrors["ResetPasswordConfirmation"]);
            }
            else
            {
                return BadRequest(_resources.ResourceForErrors["Reset-PasswordProblems"]);
            }
        }

        

        

        private async Task AddEntryMembershipDate(string userId)
        {
            if (!(await _userDatesService.UserHasMembership(userId)))
            {
                await _userDatesService.AddDateEntryAsync(userId);
            }
        }
    }
}