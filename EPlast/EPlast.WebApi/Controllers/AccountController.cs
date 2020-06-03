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
using EPlast.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        /*private readonly IAccountService _accountService;
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

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            try
            {
                LoginViewModel loginViewModel = new LoginViewModel
                {
                    ReturnUrl = returnUrl,
                    ExternalLogins = (await _accountService.GetAuthSchemesAsync()).ToList()
                };
                return Ok(loginViewModel);
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [HttpPost("/signin")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginVM, string returnUrl)
        {
            try
            {
                loginVM.ReturnUrl = returnUrl;
                loginVM.ExternalLogins = (await _accountService.GetAuthSchemesAsync()).ToList();

                if (ModelState.IsValid)
                {
                    var user = await _accountService.FindByEmailAsync(loginVM.Email);
                    if (user == null)
                    {
                        ModelState.AddModelError("", _resourceForErrors["Login-NotRegistered"]);
                        return Ok(loginVM);
                    }
                    else
                    {
                        if (!await _accountService.IsEmailConfirmedAsync(user))
                        {
                            ModelState.AddModelError("", _resourceForErrors["Login-NotConfirmed"]);
                            return Ok(loginVM);
                        }
                    }
                    var result = await _accountService.SignInAsync(_mapper.Map<LoginDto>(loginVM));
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
                        return Ok(loginVM);
                    }
                }
                //return View("Login", loginVM);
                return Ok(loginVM);
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return Ok("Register");
        }*/

        [HttpGet("lala")]       //api/account/lala
        [AllowAnonymous]
        public ActionResult<IEnumerable<string>> TestValues()
        {
            return new string[] { "value1", "value2" };
        }

        /*[HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", _resourceForErrors["Register-InCorrectData"]);
                    return Ok("Register");
                }

                var registeredUser = await _accountService.FindByEmailAsync(registerVM.Email);
                if (registeredUser != null)
                {
                    ModelState.AddModelError("", _resourceForErrors["Register-RegisteredUser"]);
                    return Ok("Register");
                }
                else
                {
                    var result = await _accountService.CreateUserAsync(_mapper.Map<RegisterViewModel, RegisterDto>(registerVM));
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", _resourceForErrors["Register-InCorrectPassword"]);
                        return Ok("Register");
                    }
                    else
                    {
                        /*string code = await _accountService.AddRoleAndTokenAsync(_mapper.Map<RegisterViewModel, RegisterDto>(registerVM));
                        var userDto = await _accountService.FindByEmailAsync(registerVM.Email);
                        string confirmationLink = Url.Action(
                            nameof(),
                            "Account",
                            new { code = code, userId = userDto.Id },
                              protocol: HttpContext.Request.Scheme);
                        await _accountService.SendEmailRegistr(confirmationLink, userDto);*/
                        /*return Ok("AcceptingEmail");
                    }
                }
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }*/

    }
}