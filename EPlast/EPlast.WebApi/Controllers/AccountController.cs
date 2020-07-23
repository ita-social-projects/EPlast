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
        private readonly IJwtService _JwtService;
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
            IJwtService JwtService,
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
            _JwtService = JwtService;
            _homeService = homeService;
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
                    if (result.Succeeded)
                    {
                        var generatedToken = _JwtService.GenerateJWTToken(user);
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
        [Authorize]
        public async Task<IActionResult> ChangePassword()
        {
            var userDto = await _accountService.GetUserAsync(User);
            var result = userDto.SocialNetworking;
            if (result != true)
            {
                return Ok("changePassword");
            }
            else
            {
                return Ok(_resourceForErrors["ChangePasswordNotAllowed"]);
            }
        }

        [HttpPost("changePassword")]
        [Authorize]  //коли зайшов через реакт він не авторизований
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