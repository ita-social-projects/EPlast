using System.Threading.Tasks;
using System.Web;
using EPlast.BLL.DTO.Account;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.ActiveMembership;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Interfaces.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog.Extensions.Logging;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthEmailService _authEmailServices;
        private readonly IAuthService _authService;
        private readonly IResources _resources;
        private readonly IUserDatesService _userDatesService;
        private readonly ILoggerService<AuthController> _logger;
        private readonly IEmailSendingService _emailSendingService;
        private const int TotalMinutesInOneDay = 1440;

        public AuthController(
            IAuthService authService,
            IUserDatesService userDatesService,
            IResources resources,
            IAuthEmailService authEmailServices,
            ILoggerService<AuthController> logger,
            IEmailSendingService emailSendingService
        )
        {
            _authService = authService;
            _userDatesService = userDatesService;
            _resources = resources;
            _authEmailServices = authEmailServices;
            _logger = logger;
            _emailSendingService = emailSendingService;
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
        public async Task<IActionResult> ConfirmingEmailAsync(string userId, string token)
        {
            _logger.LogInformation($"Executing method ConfirmingEmailAsync for user {userId} and token: {token}");
            _logger.LogInformation($"Executing method FindByIdAsync in ConfirmingEmailAsync for user {userId}");
            var userDto = await _authService.FindByIdAsync(userId);
            if (userDto == null)
            {
                _logger.LogInformation($"Method FindByIdAsync in ConfirmingEmailAsync can not find user {userId}");
                return BadRequest();
            }
            _logger.LogInformation($"Executing method GetTimeAfterRegister in ConfirmingEmailAsync for user {userId}");
            int totalTime = _authService.GetTimeAfterRegister(userDto);
            _logger.LogInformation($"Method GetTimeAfterRegister in ConfirmingEmailAsync for user {userId} returned time = {totalTime}");
            if (totalTime < TotalMinutesInOneDay)
            {
                _logger.LogInformation($"Total time {totalTime} is less when 1440 for user {userId} in method ConfirmingEmailAsync");
                if (string.IsNullOrWhiteSpace(userId) && string.IsNullOrWhiteSpace(token))
                {
                    _logger.LogInformation($"User {userId} and {token} are null or empty in method ConfirmingEmailAsync");
                    return BadRequest();
                }
                _logger.LogInformation($"Executing method ConfirmEmailAsync in ConfirmingEmailAsync for user {userId}");
                var result = await _authEmailServices.ConfirmEmailAsync(userDto.Id, HttpUtility.UrlDecode(token));

                if (result.Succeeded)
                {
                    _logger.LogInformation($"Method ConfirmEmailAsync is successfully executed in ConfirmingEmailAsync for user {userId}");
                    string signInUrl = ConfigSettingLayoutRenderer.DefaultConfiguration.GetSection("URLs")["SignIn"];
                    _logger.LogInformation($"Executing method SendEmailGreetingAsync in ConfirmingEmailAsync for user {userId}");
                    var greetingSendResult = await _authEmailServices.SendEmailGreetingAsync(userDto.Email);
                    if (greetingSendResult)
                    {
                        _logger.LogInformation($"Method SendEmailGreetingAsync is successfully executed in ConfirmingEmailAsync for user {userId}");
                        return Redirect(signInUrl);
                    }
                    else
                    {
                        _logger.LogError($"Method SendEmailGreetingAsync is executed with errors in ConfirmingEmailAsync for user {userId}");
                        return BadRequest();
                    }
                }
                else
                {
                    _logger.LogError($"Method ConfirmEmailAsync is executed with errors in ConfirmingEmailAsync for user {userId}");
                    return BadRequest();
                }
            }
            else
            {
                _logger.LogInformation($"Total time {totalTime} in ConfirmingEmailAsync is greater when 1440 -> register not allowed");
                return Ok(_resources.ResourceForErrors["ConfirmedEmailNotAllowed"]);
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
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(_resources.ResourceForErrors["Register-InCorrectData"]);
            }
            var registeredUser = await _authService.FindByEmailAsync(registerDto.Email);
            if (registeredUser != null && registeredUser.EmailConfirmed)
            {
                return BadRequest(_resources.ResourceForErrors["Register-RegisteredUserExists"]);
            }
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
        /// Method for resending email after SMTPServer error
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <returns>Answer from backend for resending email method</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Problems with resending email</response>
        [HttpPost("resendEmailForRegistering/{userId}")]
        //[AllowAnonymous]
        public async Task<IActionResult> ResendEmailForRegistering(string userId)
        {
            var userDto = await _authService.FindByIdAsync(userId);
            if (userDto == null)
            {
                return BadRequest();
            }
            await _authEmailServices.SendEmailRegistrAsync(userDto.Email);
            return Ok(_resources.ResourceForErrors["EmailForRegistering-Resended"]);
        }

        [HttpPost("feedback")]
        public async Task<IActionResult> Feedback(FeedbackDto feedbackDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _emailSendingService.SendEmailAsync(
                "eplastdmnstrtr@gmail.com",
                "Питання користувачів",
                $@"
                Ім'я: {feedbackDto.Name}<br/>
                Пошта: {feedbackDto.Email}<br/>
                Номер телефону: {feedbackDto.PhoneNumber}<br/>
                Коментар: {feedbackDto.FeedbackBody}<br/>
                ",
                "EPlast"
            );

            return NoContent();
        }
    }
}
