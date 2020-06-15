using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BussinessLayer.DTO.UserProfiles;
using EPlast.BussinessLayer.Interfaces;
using EPlast.BussinessLayer.Interfaces.UserProfiles;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.Controllers;
using EPlast.Resources;
using EPlast.ViewModels.UserInformation;
using EPlast.ViewModels.UserInformation.UserProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
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

        public UserController(IUserService userService,
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
        // GET api/userValues
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetAsync(string ID)
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [Authorize]
        [HttpGet("{userId}")]
        [Route("userProfile")]
        public async Task<ActionResult<string>> UserProfile(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    // _logger.Log(LogLevel.Error, "User id is null");
                    // return RedirectToAction("HandleError", "Error", new { code = 500 });
                    userId = await _userManagerService.GetUserIdAsync(User);
                }
                
                var user = await _userService.GetUserAsync(userId);
                var time = await _userService.CheckOrAddPlastunRoleAsync(_mapper.Map<UserDTO, UserViewModel>(user).Id, user.RegistredOn);
                var isUserPlastun = await _userManagerService.IsInRoleAsync(user, "Пластун");

                var model = new PersonalDataViewModel
                {
                    User = _mapper.Map<UserDTO, UserViewModel>(user),
                    TimeToJoinPlast = time,
                    IsUserPlastun = isUserPlastun
                };

                return Ok(model);
            }
            catch
            {
                _loggerService.LogError("Smth went wrong");
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [Authorize]
        [HttpGet("{userId}")]
        public async Task<IActionResult> Edit(string userId)
        {
            if (userId == null)
            {
                _loggerService.LogError("User id is null");
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }

            try
            {
                var user = await _userService.GetUserAsync(userId);

                var genders = (from item in await _genderService.GetAllAsync() select new SelectListItem { Text = item.Name, Value = item.ID.ToString() });

                var placeOfStudyUnique = _mapper.Map<IEnumerable<EducationDTO>, IEnumerable<EducationViewModel>>(await _educationService.GetAllGroupByPlaceAsync());
                var specialityUnique = _mapper.Map<IEnumerable<EducationDTO>, IEnumerable<EducationViewModel>>(await _educationService.GetAllGroupBySpecialityAsync());
                var placeOfWorkUnique = _mapper.Map<IEnumerable<WorkDTO>, IEnumerable<WorkViewModel>>(await _workService.GetAllGroupByPlaceAsync());
                var positionUnique = _mapper.Map<IEnumerable<WorkDTO>, IEnumerable<WorkViewModel>>(await _workService.GetAllGroupByPositionAsync());

                var educView = new EducationUserViewModel { PlaceOfStudyID = user.UserProfile.EducationId, SpecialityID = user.UserProfile.EducationId, PlaceOfStudyList = placeOfStudyUnique, SpecialityList = specialityUnique };
                var workView = new WorkUserViewModel { PlaceOfWorkID = user.UserProfile.WorkId, PositionID = user.UserProfile.WorkId, PlaceOfWorkList = placeOfWorkUnique, PositionList = positionUnique };
                var model = new EditUserViewModel()
                {
                    User = _mapper.Map<UserDTO, UserViewModel>(user),
                    Nationalities = _mapper.Map<IEnumerable<NationalityDTO>, IEnumerable<NationalityViewModel>>(await _nationalityService.GetAllAsync()),
                    Religions = _mapper.Map<IEnumerable<ReligionDTO>, IEnumerable<ReligionViewModel>>(await _religionService.GetAllAsync()),
                    EducationView = educView,
                    WorkView = workView,
                    Degrees = _mapper.Map<IEnumerable<DegreeDTO>, IEnumerable<DegreeViewModel>>(await _degreeService.GetAllAsync()),
                    Genders = genders
                };

                return Ok(model);
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: { e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model, IFormFile file)
        {
            try
            {
                await _userService.UpdateAsync(_mapper.Map<UserViewModel, UserDTO>(model.User), file, model.EducationView.PlaceOfStudyID, model.EducationView.SpecialityID, model.WorkView.PlaceOfWorkID, model.WorkView.PositionID);
                _loggerService.LogInformation($"User {model.User.Email} was edited profile and saved in the database");
                return RedirectToAction("UserProfile");
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: { e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }
        [Authorize]
        public async Task<IActionResult> ApproveUser(string userId, bool isClubAdmin = false, bool isCityAdmin = false)
        {
            if (userId != null)
            {
                await _confirmedUserService.CreateAsync(User, userId, isClubAdmin, isCityAdmin);
                return RedirectToAction("Approvers", "Account", new { userId = userId });
            }
            _loggerService.LogError("User id is null");
            return RedirectToAction("HandleError", "Error", new { code = 500 });
        }

        [Authorize]
        public async Task<IActionResult> ApproverDelete(int confirmedId, string userId)
        {
            await _confirmedUserService.DeleteAsync(confirmedId);
            _loggerService.LogInformation("Approve succesfuly deleted");
            return RedirectToAction("Approvers", "Account", new { userId = userId });
        }
        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
