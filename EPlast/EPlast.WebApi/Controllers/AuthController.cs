using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using EPlast.BLL.DTO.Account;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.ActiveMembership;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.HostURL;
using EPlast.BLL.Interfaces.Logging;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace EPlast.WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public struct ConflictErrorObject
        {
            public ConflictErrorObject(string error, bool isEmailConfirmed) : this()
            {
                Error = error;
                IsEmailConfirmed = isEmailConfirmed;
            }
            public ConflictErrorObject(string error, bool isEmailConfirmed, DateTime registeredExpire) : this(error, isEmailConfirmed)
            {

                RegisteredExpire = registeredExpire;
            }

            public string Error { get; }
            public bool IsEmailConfirmed { get; }
            public DateTime? RegisteredExpire { get; }
        }

        private readonly IUserDatesService _userDatesService;
        private readonly IEmailSendingService _emailSendingService;
        private readonly UserManager<User> _userManager;
        private readonly IHostUrlService _hostURLService;
        private readonly IMapper _mapper;
        private readonly ICityParticipantsService _cityParticipantsService;
        private readonly ILoggerService<AuthController> _loggerService;

        public AuthController(
            IUserDatesService userDatesService,
            IEmailSendingService emailSendingService,
            IMapper mapper,
            ICityParticipantsService cityParticipantsService,
            UserManager<User> userManager,
            IHostUrlService hostURLService, ILoggerService<AuthController> loggerService)
        {
            _userDatesService = userDatesService;
            _emailSendingService = emailSendingService;
            _mapper = mapper;
            _cityParticipantsService = cityParticipantsService;
            _userManager = userManager;
            _hostURLService = hostURLService;
            _loggerService = loggerService;
        }

        /// <summary>
        /// Email confirmation
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="token">Email confirmation token</param>
        /// <returns>Action response</returns>
        /// <response code="204">Successful operation</response>
        /// <response code="400">UserId or Token were found invalid</response>
        /// <response code="404">User by specified id is not found</response>
        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail([Required, FromQuery] string userId, [Required, FromQuery] string token)
        {
            var decodedToken = HttpUtility.UrlDecode(token);

            if (!ModelState.IsValid)
            {
                _loggerService.LogWarning("Invalid ModelState");
                return Redirect(_hostURLService.GetSignInURL(error: 400));
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Redirect(_hostURLService.GetSignInURL(error: 404));
            }
            TimeSpan elapsedTimeFromRegistration = DateTime.Now - user.RegistredOn;
            if (elapsedTimeFromRegistration >= TimeSpan.FromHours(12))
            {
                // 410 GONE - User should be deleted, because 12hrs elapsed from registration and email is still is not confirmed
                await _userManager.DeleteAsync(user);
                return Redirect(_hostURLService.GetSignInURL(error: 410));
            }

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    _loggerService.LogWarning(error.Description);
                }
                return Redirect(_hostURLService.GetSignInURL(error: 400));
            }

            await _userDatesService.AddDateEntryAsync(user.Id);

            await _userManager.AddToRoleAsync(user, Roles.RegisteredUser);

            if (user.CityId != null)
            {
                await _cityParticipantsService.AddFollowerAsync((int)user.CityId, user.Id);
            }
            else if (user.RegionId != null)
            {
                await _cityParticipantsService.AddNotificationUserWithoutSelectedCity(user, (int)user.RegionId);
            }
            else
            {
                await _userManager.DeleteAsync(user);
                throw new ArgumentException("User had both RegionId and CityId set to null, which is an anomaly", nameof(user));
            }

            return Redirect(_hostURLService.SignInURL);
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="registerDto">Registration DTO</param>
        /// <returns>Action response</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="400">Registration DTO is invalid</response>
        /// <response code="409">User by specified email is already exist</response>
        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp(RegisterDto registerDto)
        {
            if (registerDto.GenderId != 1 && registerDto.GenderId != 2 && registerDto.GenderId != 7)
            {
                ModelState.AddModelError(nameof(RegisterDto.GenderId), "Unknown GenderId");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = await _userManager.FindByEmailAsync(registerDto.Email);
            if (user != null)
            {
                // Check if user's email is confirmed
                if (await _userManager.IsEmailConfirmedAsync(user))
                {
                    return Conflict(new ConflictErrorObject("User exists and email is confirmed", true));
                }

                TimeSpan elapsedTimeFromRegistration = DateTime.Now - user.RegistredOn;
                if (elapsedTimeFromRegistration < TimeSpan.FromHours(12))
                {
                    return Conflict(new ConflictErrorObject("User exists, but email is not yet confirmed", false, user.RegistredOn.AddHours(12)));
                }

                await _userManager.DeleteAsync(user);
            }

            user = _mapper.Map<User>(registerDto);
            user.RegistredOn = DateTime.Now;

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new { error = result.Errors });
            }

            try
            {
                await SendConfirmationEmail(user);
            }
            catch (Exception)
            {
                await _userManager.DeleteAsync(user);
                throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Resends email confirmation letter for the user by his email address
        /// </summary>
        /// <param name="userEmail">Email of the user</param>
        /// <returns>Action response</returns>
        /// <response code="204">Successful operation</response>
        /// <response code="404">User by specified email was not found</response>
        /// <response code="409">User's email is already confirmed</response>
        [HttpPost("resendConfirmationEmail")]
        public async Task<IActionResult> ResendConfirmationEmail([Required, EmailAddress] string userEmail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return NotFound();
            }

            if (await _userManager.IsEmailConfirmedAsync(user))
            {
                return BadRequest(new { error = "This email is already confirmed" });
            }

            await SendConfirmationEmail(user);

            return NoContent();
        }

        [HttpPost("feedback")]
        public async Task<IActionResult> Feedback(FeedbackDto feedbackDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var message = _emailSendingService.Compose(
                "eplastdmnstrtr@gmail.com",
                "Питання користувачів",
                $@"
                Ім'я: {feedbackDto.Name}<br/>
                Пошта: {feedbackDto.Email}<br/>
                Номер телефону: {feedbackDto.PhoneNumber}<br/>
                Коментар: {feedbackDto.FeedbackBody}<br/>
                "
            );

            await _emailSendingService.SendEmailAsync(message);

            return NoContent();
        }

        [NonAction]
        public async Task SendConfirmationEmail(User user)
        {
            var reciever = new MailboxAddress($"{user.FirstName} {user.LastName}", user.Email);

            var token = HttpUtility.UrlEncode(await _userManager.GenerateEmailConfirmationTokenAsync(user)); 

            string url = _hostURLService.GetConfirmEmailApiURL(user.Id, token);

            var message = _emailSendingService.Compose(
                reciever,
                "Підтвердження пошти",
                $"Перейдіть за посиланням, щоб підтвердити пошту: <br/><a href={url}>посилання</a>"
            );

            await _emailSendingService.SendEmailAsync(message);
        }
    }
}
