using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Services.Interfaces;
using EPlast.WebApi.Models.UserModels;
using EPlast.WebApi.Models.UserModels.UserProfileFields;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        private readonly IMapper _mapper;

        public UserController(IUserService userService,
            INationalityService nationalityService,
            IEducationService educationService,
            IReligionService religionService,
            IWorkService workService,
            IGenderService genderService,
            IDegreeService degreeService,
            IConfirmedUsersService confirmedUserService,
            IUserManagerService userManagerService,
            ILoggerService<AccountController> loggerService,
            IMapper mapper)
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
            _mapper = mapper;
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
                    User = _mapper.Map<UserDTO, UserViewModel>(user),
                    TimeToJoinPlast = ((int)time.TotalDays),
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

        [HttpGet("getImage/{imageName}")]
        public async Task<string> GetImage(string imageName)
        {
            return await _userService.GetImageBase64Async(imageName);
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
            var user = await _userService.GetUserAsync(userId);
            var genders = _mapper.Map<IEnumerable<GenderDTO>, IEnumerable<GenderViewModel>>(await _genderService.GetAllAsync());

            var placeOfStudyUnique = _mapper.Map<IEnumerable<EducationDTO>, IEnumerable<EducationViewModel>>(await _educationService.GetAllGroupByPlaceAsync());
            var specialityUnique = _mapper.Map<IEnumerable<EducationDTO>, IEnumerable<EducationViewModel>>(await _educationService.GetAllGroupBySpecialityAsync());
            var placeOfWorkUnique = _mapper.Map<IEnumerable<WorkDTO>, IEnumerable<WorkViewModel>>(await _workService.GetAllGroupByPlaceAsync());
            var positionUnique = _mapper.Map<IEnumerable<WorkDTO>, IEnumerable<WorkViewModel>>(await _workService.GetAllGroupByPositionAsync());

            var educView = new UserEducationViewModel { PlaceOfStudyID = user.UserProfile.EducationId, SpecialityID = user.UserProfile.EducationId, PlaceOfStudyList = placeOfStudyUnique, SpecialityList = specialityUnique };
            var workView = new UserWorkViewModel { PlaceOfWorkID = user.UserProfile.WorkId, PositionID = user.UserProfile.WorkId, PlaceOfWorkList = placeOfWorkUnique, PositionList = positionUnique };
            var model = new EditUserViewModel()
            {
                User = _mapper.Map<UserDTO, UserViewModel>(user),
                Nationalities = _mapper.Map<IEnumerable<NationalityDTO>, IEnumerable<NationalityViewModel>>(await _nationalityService.GetAllAsync()),
                Religions = _mapper.Map<IEnumerable<ReligionDTO>, IEnumerable<ReligionViewModel>>(await _religionService.GetAllAsync()),
                EducationView = educView,
                WorkView = workView,
                Degrees = _mapper.Map<IEnumerable<DegreeDTO>, IEnumerable<DegreeViewModel>>(await _degreeService.GetAllAsync()),
                Genders = genders,
            };
            return Ok(model);
        }
        [HttpPut("editbase64")]
        public async Task<IActionResult> EditBase64([FromBody] EditUserViewModel model)
        {
            try
            {
                
                await _userService.UpdateAsyncForBase64(_mapper.Map<UserViewModel, UserDTO>(model.User), model.ImageBase64, model.EducationView.PlaceOfStudyID, model.EducationView.SpecialityID, model.WorkView.PlaceOfWorkID, model.WorkView.PositionID);
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
                await _userService.UpdateAsync(_mapper.Map<UserViewModel, UserDTO>(model.User), file, model.EducationView.PlaceOfStudyID, model.EducationView.SpecialityID, model.WorkView.PlaceOfWorkID, model.WorkView.PositionID);
                _loggerService.LogInformation($"User {model.User.FirstName}{model.User.LastName} was edited profile and saved in the database");

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
