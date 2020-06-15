using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BussinessLayer.DTO.Account;
using EPlast.BussinessLayer.Interfaces;
using EPlast.BussinessLayer.Interfaces.UserProfiles;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

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
                LoginDTO loginDto = new LoginDTO
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
        public async Task<IActionResult> Login(LoginDTO loginDto, string returnUrl)
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
        public async Task<IActionResult> Register(RegisterDTO registerDto)
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

        [HttpGet]
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

        [HttpGet]
        public IActionResult ConfirmedEmail()
        {
            return Ok("ConfirmedEmail");
        }
    }
}