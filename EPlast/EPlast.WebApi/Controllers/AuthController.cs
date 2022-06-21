#nullable enable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Account;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.ActiveMembership;
using EPlast.BLL.Interfaces.City;
using EPlast.DataAccess.Entities;
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
            public ConflictErrorObject(string error, bool isEmailConfirmed)
            {
                Error = error;
                IsEmailConfirmed = isEmailConfirmed;
            }

            public string Error { get; }
            public bool IsEmailConfirmed { get; }
        }

        private readonly IUserDatesService _userDatesService;
        private readonly IEmailSendingService _emailSendingService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ICityParticipantsService _cityParticipantsService;

        public AuthController(
            IUserDatesService userDatesService,
            IEmailSendingService emailSendingService,
            IMapper mapper,
            ICityParticipantsService cityParticipantsService,
            UserManager<User> userManager
        )
        {
            _userDatesService = userDatesService;
            _emailSendingService = emailSendingService;
            _mapper = mapper;
            _cityParticipantsService = cityParticipantsService;
            _userManager = userManager;
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
        [HttpPost("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail([Required] string userId, [Required] string token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            TimeSpan elapsedTimeFromRegistration = DateTime.Now - user.RegistredOn;
            if (elapsedTimeFromRegistration >= TimeSpan.FromHours(12))
            {
                // 410 GONE - User should be deleted, because 12hrs elapsed from registration and email is still is not confirmed
                await _userManager.DeleteAsync(user);
                return StatusCode(410);
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return BadRequest(new { error = result.Errors });
            }

            return NoContent();
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

            User? user = await _userManager.FindByEmailAsync(registerDto.Email);
            if (user != null)
            {
                // Check if user's email is confirmed
                if (await _userManager.IsEmailConfirmedAsync(user))
                {
                    return Conflict(new ConflictErrorObject("User exists and email is confirmed", true));
                }
                else
                {
                    return Conflict(new ConflictErrorObject("User exists, but email is not yet confirmed", false));
                }
            }

            user = _mapper.Map<User>(registerDto);

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

            await _userDatesService.AddDateEntryAsync(user.Id);

            if (registerDto.CityId != null)
            {
                await _cityParticipantsService.AddFollowerAsync((int)registerDto.CityId, user.Id);
            }
            else
            {
                // TODO
            }

            UserDTO userDto = _mapper.Map<UserDTO>(user);
            return Ok(userDto);
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

            User? user = await _userManager.FindByEmailAsync(userEmail);
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

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // 3000 is the port of the front-end
            var url = $"https://{Request?.Host.Host ?? "localhost"}:3000/confirmEmail?userId={user.Id}&token={token}";

            var message = _emailSendingService.Compose(
                reciever,
                "Підтвердження пошти",
                $"Перейдіть за посиланням, щоб підтвердити пошту: <br/>{url}"
            );

            await _emailSendingService.SendEmailAsync(message);
        }
    }
}
