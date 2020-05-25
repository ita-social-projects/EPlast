using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using AutoMapper;
using EPlast.BussinessLayer.DTO.Account;
using EPlast.BussinessLayer.Interfaces;
using EPlast.ViewModels;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.BussinessLayer.DTO;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.ViewModels.UserInformation.UserProfile;
using Microsoft.EntityFrameworkCore;
using EPlast.ViewModels.UserInformation;
using EPlast.BussinessLayer.Services;
using Microsoft.Extensions.Logging;

namespace EPlast.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly INationalityService _nationalityService;
        private readonly IEducationService _educationService;
        private readonly IReligionService _religionService;
        private readonly IWorkService _workService;
        private readonly IGenderService _genderService;
        private readonly IDegreeService _degreeService;
        private readonly IUserManagerService _userManagerService;
        private readonly IConfirmedUsersService _confirmedUserService;
        private readonly ILoggerService<AccountController> _loggerService;

        public AccountController(IRepositoryWrapper repoWrapper,
            IUserService userService,
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
            IAccountService accountService)
        {
            _accountService = accountService;
            _repoWrapper = repoWrapper;
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
                return View(loginViewModel);
            }
            catch (Exception e)
            {
                //_logger.LogError("Exception: {0}", e.Message); 
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [HttpPost]
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
                    if(user == null)
                    {
                        ModelState.AddModelError("", "Ви не зареєстровані в системі, або не підтвердили свою електронну пошту");
                        return View(loginVM);
                    }
                    else
                    {
                        if (!await _accountService.IsEmailConfirmedAsync(user))
                        {
                            ModelState.AddModelError("", "Ваш акаунт не підтверджений, будь ласка увійдіть та зробіть підтвердження");
                            return View(loginVM);
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
                        ModelState.AddModelError("", "Ви ввели неправильний пароль, спробуйте ще раз");
                        return View(loginVM);
                    }
                }
                return View("Login", loginVM);
            }
            catch (Exception e)
            {
                //_logger.LogError("Exception: {0}", e.Message);
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel registerVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Дані введені неправильно");
                    return View("Register");
                }

                var registeredUser = await _accountService.FindByEmailAsync(registerVM.Email);
                if (registeredUser != null)
                {
                    ModelState.AddModelError("", "Користувач з введеною електронною поштою вже зареєстрований в системі, " +
                        "можливо він не підтвердив свою реєстрацію");
                    return View("Register");
                }
                else
                {
                    var result = await _accountService.CreateUserAsync(_mapper.Map<RegisterViewModel, RegisterDto>(registerVM));
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Пароль має містити цифри та літери, мінімальна довжина повинна складати 8");
                        return View("Register");
                    }
                    else
                    {
                        string code = await _accountService.AddRoleAndTokenAsync(_mapper.Map<RegisterViewModel, RegisterDto>(registerVM));
                        var userDto = await _accountService.FindByEmailAsync(registerVM.Email);
                        string confirmationLink = Url.Action(
                            nameof(ConfirmingEmail),
                            "Account",
                            new { code = code, userId = userDto.Id },
                              protocol: HttpContext.Request.Scheme);
                        await _accountService.SendEmailRegistr(confirmationLink, userDto);
                        return View("AcceptingEmail");
                    }
                }
            }
            catch (Exception e)
            {
                //_logger.LogError("Exception: {0}", e.Message);
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }
        
        [HttpGet]
        public IActionResult ConfirmedEmail()
        {
            return View("ConfirmedEmail");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ResendEmailForRegistering(string userId)
        {
            var userDto = await _accountService.FindByIdAsync(userId);
            if (userDto == null)
            {
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
            string code = await _accountService.GenerateConfToken(userDto);  //чекати
            var confirmationLink = Url.Action(
                nameof(ConfirmingEmail),
                "Account",
                new { code = code, userId = userDto.Id },
                protocol: HttpContext.Request.Scheme);

            await _accountService.SendEmailRegistr(confirmationLink, userDto);
            return View("ResendEmailConfirmation");
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
                    return RedirectToAction("ConfirmedEmail", "Account");
                }
                else
                {
                    return RedirectToAction("HandleError", "Error", new { code = 500 });
                }
            }
            else
            {
                return View("ConfirmEmailNotAllowed", userDto);
            }
        }

        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccountLocked()
        {
            return View("AccountLocked");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Logout()
        {
            _accountService.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View("ForgotPassword");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotpasswordVM)  
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userDto = await _accountService.FindByEmailAsync(forgotpasswordVM.Email);
                    if (userDto == null || !(await _accountService.IsEmailConfirmedAsync(userDto)))
                    {
                        ModelState.AddModelError("", "Користувача із заданою електронною поштою немає в системі або він не підтвердив свою реєстрацію");
                        return View("ForgotPassword");
                    }
                    string code = await _accountService.GenerateResetTokenAsync(userDto);
                    string confirmationLink = Url.Action(
                        nameof(ResetPassword),
                        "Account",
                        new { userId = userDto.Id, code = HttpUtility.UrlEncode(code) },
                        protocol: HttpContext.Request.Scheme);
                    await _accountService.SendEmailReseting(confirmationLink, _mapper.Map<ForgotPasswordDto>(forgotpasswordVM));
                    return View("ForgotPasswordConfirmation");
                }
                return View("ForgotPassword");
            }
            catch (Exception e)
            {
                //_logger.LogError("Exception: {0}", e.Message);
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [HttpGet]        
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
                    return View("ResetPassword");
                }
            }
            else
            {
                return View("ResetPasswordNotAllowed", userDto);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetpasswordVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("ResetPassword");
                }
                var userDto = await _accountService.FindByEmailAsync(resetpasswordVM.Email);
                if (userDto == null)
                {
                    ModelState.AddModelError("", "Користувача із заданою електронною поштою немає в системі або він не підтвердив свою реєстрацію");
                    return View("ResetPassword");
                }
                var result = await _accountService.ResetPasswordAsync(userDto.Id, _mapper.Map<ResetPasswordDto>(resetpasswordVM));
                if (result.Succeeded)
                {
                    await _accountService.CheckingForLocking(userDto);
                    return View("ResetPasswordConfirmation");
                }
                else
                {
                    ModelState.AddModelError("", "Проблеми зі скидуванням пароля або введений новий пароль повинен вміщати 8символів, включаючи літери та цифри");
                    return View("ResetPassword");
                }
            }
            catch (Exception e)
            {
                //_logger.LogError("Exception: {0}", e.Message);
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ChangePassword()
        {
            var userDto = await _accountService.GetUserAsync(User);
            var result = userDto.SocialNetworking;
            if (result != true)
            {
                return View("ChangePassword");
            }
            else
            {
                return View("ChangePasswordNotAllowed");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changepasswordModel)
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
                    var result = await _accountService.ChangePasswordAsync(userDto.Id, _mapper.Map<ChangePasswordDto>(changepasswordModel));
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Проблема зі зміною пароля, можливо неправильно введений старий пароль");
                        return View("ChangePassword");
                    }
                    _accountService.RefreshSignInAsync(userDto);
                    return View("ChangePasswordConfirmation");
                }
                else
                {
                    return View("ChangePassword");
                }
            }
            catch (Exception e)
            {
                //_logger.LogError("Exception: {0}", e.Message);
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            string redirectUrl = Url.Action("ExternalLoginCallBack", 
                "Account",
                new { ReturnUrl = returnUrl });
            AuthenticationProperties properties = _accountService.GetAuthProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallBack(string returnUrl = null, string remoteError = null)
        {
            try
            {
                returnUrl = returnUrl ?? Url.Content("~/Account/UserProfile");
                LoginViewModel loginViewModel = new LoginViewModel
                {
                    ReturnUrl = returnUrl,
                    ExternalLogins = (await _accountService.GetAuthSchemesAsync()).ToList()
                };

                if (remoteError != null)
                {
                    ModelState.AddModelError(string.Empty, $"Error from external provider : {remoteError}");
                    return View("Login", loginViewModel);
                }
                var info = await _accountService.GetInfoAsync(); 
                if (info == null)
                {
                    ModelState.AddModelError(string.Empty, "Error loading external login information");
                    return View("Login", loginViewModel);
                }

                var signInResult = await _accountService.GetSignInResultAsync(info);
                if (signInResult.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    var email = info.Principal.FindFirstValue(ClaimTypes.Email);
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
                    return View("Error");
                }
            }
            catch (Exception e)
            {
                //_logger.LogError("Exception: {0}", e.Message);
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UserProfile(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    // _logger.Log(LogLevel.Error, "User id is null");
                    // return RedirectToAction("HandleError", "Error", new { code = 500 });
                    userId = _userManagerService.GetUserId(User);
                }

                var user = _userService.GetUser(userId);
                var time = _userService.CheckOrAddPlastunRole(_mapper.Map<UserDTO, UserViewModel>(user).Id, user.RegistredOn);
                var isUserPlastun = await _userManagerService.IsInRole(user, "Пластун");

                var model = new PersonalDataViewModel
                {
                    User = _mapper.Map<UserDTO, UserViewModel>(user),
                    TimeToJoinPlast = time.Result,
                    IsUserPlastun = isUserPlastun
                };

                return View(model);
            }
            catch
            {
               _loggerService.LogError("Smth went wrong");
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        /*[HttpGet]
        public IActionResult Approvers(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    _loggerService.LogError("User id is null");
                    return RedirectToAction("HandleError", "Error", new { code = 500 });
                }

                var user = _userService.GetUser(userId);
                var _confUsers = _userService.GetConfirmedUsers(user);
                var canApprove = _userService.CanApprove(_confUsers, userId, User);
                var time = await _userService.CheckOrAddPlastunRole(user.Id, user.RegistredOn);
                var clubApprover = _userService.GetClubAdminConfirmedUser(user);
                var cityApprover = _userService.GetCityAdminConfirmedUser(user);

                if (user != null)
                {
                    var model = new UserApproversViewModel
                    {
                        User = _mapper.Map<UserDTO, UserViewModel>(user),
                        canApprove = canApprove,
                        TimeToJoinPlast = time,
                        ConfirmedUsers = _mapper.Map<IEnumerable<ConfirmedUserDTO>, IEnumerable<ConfirmedUserViewModel>>(_confUsers),
                        ClubApprover = _mapper.Map<ConfirmedUserDTO, ConfirmedUserViewModel>(clubApprover),
                        CityApprover = _mapper.Map<ConfirmedUserDTO, ConfirmedUserViewModel>(cityApprover),
                        IsUserHeadOfCity = await _userManagerService.IsInRole(user, "Голова Станиці"),
                        IsUserHeadOfClub = await _userManagerService.IsInRole(user, "Голова Куреня"),
                        IsUserHeadOfRegion = await _userManagerService.IsInRole(user, "Голова Округу"),
                        IsUserPlastun = await _userManagerService.IsInRole(user, "Пластун"),
                        CurrentUserId= _userManagerService.GetUserId(User)
                    };

                    return View(model);
                }
                _loggerService.LogError( $"Can`t find this user:{userId}, or smth else");
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
            catch
            {
                _loggerService.LogError("Smth went wrong");
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }       

        public IActionResult ApproveUser(string userId, bool _isClubAdmin = false, bool _isCityAdmin = false)
        {
            if (userId != null)
            {
                _confirmedUserService.Create(User, userId, _isClubAdmin, _isCityAdmin);
                return RedirectToAction("Approvers", "Account", new { userId = userId });
            }
            _loggerService.LogError("User id is null");
            return RedirectToAction("HandleError", "Error", new { code = 500 });
        }

        [Authorize]
        public IActionResult ApproverDelete(int confirmedId, string userId)
        {
            _confirmedUserService.Delete(confirmedId);
            _loggerService.LogInformation("Approve succesfuly deleted");
            return RedirectToAction("UserProfile", "Account", new { userId = userId });
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(string userId)
        {
            if(userId == null)
            {
                _loggerService.LogError("User id is null");
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }

            try
            {
                var user = _userService.GetUser(userId);

                var genders = (from item in _genderService.GetAll() select new SelectListItem { Text = item.Name, Value = item.ID.ToString() });
                                                                                            
                var placeOfStudyUnique = _mapper.Map<IEnumerable<EducationDTO>, IEnumerable<EducationViewModel>>(_educationService.GetAllGroupByPlace());
                var specialityUnique = _mapper.Map<IEnumerable<EducationDTO>, IEnumerable<EducationViewModel>>(_educationService.GetAllGroupBySpeciality());
                var placeOfWorkUnique = _mapper.Map<IEnumerable<WorkDTO>, IEnumerable<WorkViewModel>>(_workService.GetAllGroupByPlace()); 
                var positionUnique = _mapper.Map<IEnumerable<WorkDTO>, IEnumerable<WorkViewModel>>(_workService.GetAllGroupByPosition());

                var educView = new EducationUserViewModel {PlaceOfStudyID=user.UserProfile.EducationId, SpecialityID = user.UserProfile.EducationId, PlaceOfStudyList = placeOfStudyUnique, SpecialityList = specialityUnique };
                var workView = new WorkUserViewModel { PlaceOfWorkID = user.UserProfile.WorkId, PositionID = user.UserProfile.WorkId, PlaceOfWorkList = placeOfWorkUnique, PositionList = positionUnique };
                var model = new EditUserViewModel()
                {
                    User = _mapper.Map<UserDTO, UserViewModel>(user),
                    Nationalities = _mapper.Map<IEnumerable<NationalityDTO>, IEnumerable<NationalityViewModel>>(_nationalityService.GetAll()),
                    Religions = _mapper.Map<IEnumerable<ReligionDTO>, IEnumerable<ReligionViewModel>>(_religionService.GetAll()),
                    EducationView = educView,
                    WorkView = workView,
                    Degrees = _mapper.Map<IEnumerable<DegreeDTO>, IEnumerable<DegreeViewModel>>(_degreeService.GetAll()),
                    Genders= genders
                };

                return View(model);
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: { e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(EditUserViewModel model, IFormFile file)
        {
            try
            {
                _userService.Update(_mapper.Map<UserViewModel,UserDTO>(model.User), file, model.EducationView.PlaceOfStudyID, model.EducationView.SpecialityID, model.WorkView.PlaceOfWorkID, model.WorkView.PositionID);
                _loggerService.LogInformation($"User {model.User.Email} was edited profile and saved in the database");
                return RedirectToAction("UserProfile");
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: { e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }
        public async Task<IActionResult> Positions(string userId)
        {
            try
            {
                var user = _userService.GetUser(userId);
                var result = new PositionUserViewModel
                {
                    User = _mapper.Map<UserDTO, UserViewModel>(user),
                    TimeToJoinPlast = await _userService.CheckOrAddPlastunRole(userId, user.RegistredOn),
                    IsUserPlastun = await _userManagerService.IsInRole(user, "Пластун")
                };
                return View(result);
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: { e.Message}");
                return RedirectToAction("HandleError", "Error");
            }
        }*/
    }
}