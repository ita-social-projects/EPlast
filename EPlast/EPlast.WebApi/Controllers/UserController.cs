using EPlast.BusinessLogicLayer.Interfaces.UserProfiles;
using EPlast.BusinessLogicLayer.Services.Interfaces;
using EPlast.WebApi.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BusinessLogicLayer.Interfaces.Logging;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
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

        public UserController(IUserService userService,
            INationalityService nationalityService,
            IEducationService educationService,
            IReligionService religionService,
            IWorkService workService,
            IGenderService genderService,
            IDegreeService degreeService,
            IConfirmedUsersService confirmedUserService,
            IUserManagerService userManagerService,
            ILoggerService<AccountController> loggerService)
        {
            _userService = userService;
            _nationalityService = nationalityService;
            _religionService = religionService;
            _degreeService = degreeService;
            _workService = workService;
            _educationService = educationService;
            _genderService = genderService;
            _confirmedUserService = confirmedUserService;
            _userManagerService = userManagerService;
            _loggerService = loggerService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(string userId)
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
                var time = await _userService.CheckOrAddPlastunRoleAsync(user.Id, user.RegistredOn);
                var isUserPlastun = await _userManagerService.IsInRoleAsync(user, "Пластун");

                var model = new PersonalDataViewModel
                {
                    User = user,
                    TimeToJoinPlast = time,
                    IsUserPlastun = isUserPlastun
                };

                return Ok(model);
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Smth went wrong: {e.Message}");
                return BadRequest();
            }
        }

        //[Authorize]
        [HttpGet("edit/{userId}")]
        public async Task<IActionResult> Edit(string userId)
        {
            if (userId == null)
            {
                _loggerService.LogError("User id is null");
                return BadRequest();
            }

            try
            {
                var user = await _userService.GetUserAsync(userId);

                var genders = (from item in await _genderService.GetAllAsync() select new SelectListItem { Text = item.Name, Value = item.ID.ToString() });

                var placeOfStudyUnique = await _educationService.GetAllGroupByPlaceAsync();
                var specialityUnique = await _educationService.GetAllGroupBySpecialityAsync();
                var placeOfWorkUnique = await _workService.GetAllGroupByPlaceAsync();
                var positionUnique = await _workService.GetAllGroupByPositionAsync();

                var educView = new UserEducationViewModel { PlaceOfStudyID = user.UserProfile.EducationId, SpecialityID = user.UserProfile.EducationId, PlaceOfStudyList = placeOfStudyUnique, SpecialityList = specialityUnique };
                var workView = new UserWorkViewModel { PlaceOfWorkID = user.UserProfile.WorkId, PositionID = user.UserProfile.WorkId, PlaceOfWorkList = placeOfWorkUnique, PositionList = positionUnique };
                var model = new EditUserViewModel()
                {
                    User = user,
                    Nationalities = await _nationalityService.GetAllAsync(),
                    Religions = await _religionService.GetAllAsync(),
                    EducationView = educView,
                    WorkView = workView,
                    Degrees = await _degreeService.GetAllAsync(),
                    Genders = genders
                };

                return Ok(model);
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: { e.Message}");
                return BadRequest();
            }
        }
        [HttpPut("editbase64")]
        public async Task<IActionResult> EditBase64(EditUserViewModel model, string base64)
        {
            try
            {
                await _userService.UpdateAsync(model.User, base64, model.EducationView.PlaceOfStudyID, model.EducationView.SpecialityID, model.WorkView.PlaceOfWorkID, model.WorkView.PositionID);
                _loggerService.LogInformation($"User  was edited profile and saved in the database");

                return Ok();
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: { e.Message}");
                return BadRequest();
            }
        }
        // [Authorize]
        [HttpPut("edit")]
        public async Task<IActionResult> Edit(EditUserViewModel model, IFormFile file)
        {
            try
            {
                await _userService.UpdateAsync(model.User, file, model.EducationView.PlaceOfStudyID, model.EducationView.SpecialityID, model.WorkView.PlaceOfWorkID, model.WorkView.PositionID);
                _loggerService.LogInformation($"User {model.User.Email} was edited profile and saved in the database");

                return Ok();
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: { e.Message}");
                return BadRequest();
            }
        }
        // [Authorize]
        [HttpPost("approveUser/{userId}/{isClubAdmin}/{isCityAdmin}")]
        public async Task<IActionResult> ApproveUser(string userId, bool isClubAdmin = false, bool isCityAdmin = false)
        {
            try
            {
                if (userId != null)
                {
                    await _confirmedUserService.CreateAsync(User, userId, isClubAdmin, isCityAdmin);
                    return Ok();
                }
                throw new ArgumentException("User id is null");
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: { e.Message}");
                return BadRequest();
            }
        }

        //   [Authorize]
        [HttpDelete("deleteApprove/{confirmedId}")]
        public async Task<IActionResult> ApproverDelete(int confirmedId)
        {
            try
            {
                await _confirmedUserService.DeleteAsync(confirmedId);
                _loggerService.LogInformation("Approve succesfuly deleted");

                return Ok();
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: { e.Message}");
                return BadRequest();
            }

        }
    }
}
