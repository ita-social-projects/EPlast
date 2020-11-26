using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Services.Interfaces;
using EPlast.WebApi.Models.Approver;
using EPlast.WebApi.Models.User;
using EPlast.WebApi.Models.UserModels;
using EPlast.WebApi.Models.UserModels.UserProfileFields;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ILoggerService<UserController> _loggerService;
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
            ILoggerService<UserController> loggerService,
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

        /// <summary>
        /// Get a specify user
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>A user</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">User not found</response>
        [HttpGet("{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Get(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _loggerService.LogError("User id is null");
                return NotFound();
            }

            var user = await _userService.GetUserAsync(userId);
            if (user != null)
            {
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

            _loggerService.LogError($"User not found. UserId:{userId}");
            return NotFound();
        }

        /// <summary>
        /// Get a image
        /// </summary>
        /// <param name="imageName">The name of the image</param>
        /// <returns>Image in format base64</returns>
        /// <response code="200">Successful operation</response>
        [HttpGet("getImage/{imageName}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetImage(string imageName)
        {
            return Ok(await _userService.GetImageBase64Async(imageName));
        }

        /// <summary>
        /// Get specify model for edit user
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>A data of user for editing</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">User not found</response>
        [HttpGet("edit/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Edit(string userId)
        {
            if (userId == null)
            {
                _loggerService.LogError("User id is null");

                return NotFound();
            }
            var user = await _userService.GetUserAsync(userId);
            if (user != null)
            {
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
            _loggerService.LogError($"User not found. UserId:{userId}");
            return NotFound();
        }

        /// <summary>
        /// Edit a user
        /// </summary>
        /// <param name="model">Edit model</param>
        /// <response code="200">Successful operation</response>
        [HttpPut("editbase64")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> EditBase64([FromBody] EditUserViewModel model)
        {
            await _userService.UpdateAsyncForBase64(_mapper.Map<UserViewModel, UserDTO>(model.User),
                model.ImageBase64, model.EducationView.PlaceOfStudyID, model.EducationView.SpecialityID,
                model.WorkView.PlaceOfWorkID, model.WorkView.PositionID);
            _loggerService.LogInformation($"User was edited profile and saved in the database");

            return Ok();
        }

        /// <summary>
        /// Get approvers of selected user
        /// </summary>
        /// <param name="userId">The id of the user which can be approved</param>
        /// <param name="approverId">The id of the user which can approve</param>
        /// <returns>A specify data of user approvers for approve user or just review</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">User not found</response>
        [HttpGet("approvers/{userId}/{approverId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Approvers(string userId, string approverId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _loggerService.LogError("User id is null");
                return NotFound();
            }
            var user = await _userService.GetUserAsync(userId);
            var confirmedUsers = _userService.GetConfirmedUsers(user);
            var canApprove = await _userService.CanApproveAsync(confirmedUsers, userId, User);
            var time = await _userService.CheckOrAddPlastunRoleAsync(user.Id, user.RegistredOn);
            var clubApprover = _userService.GetClubAdminConfirmedUser(user);
            var cityApprover = _userService.GetCityAdminConfirmedUser(user);

            if (user != null)
            {
                var model = new UserApproversViewModel
                {
                    User = _mapper.Map<UserDTO, UserInfoViewModel>(user),
                    canApprove = canApprove,
                    TimeToJoinPlast = ((int)time.TotalDays),
                    ConfirmedUsers = _mapper.Map<IEnumerable<ConfirmedUserDTO>, IEnumerable<ConfirmedUserViewModel>>(confirmedUsers),
                    ClubApprover = _mapper.Map<ConfirmedUserDTO, ConfirmedUserViewModel>(clubApprover),
                    CityApprover = _mapper.Map<ConfirmedUserDTO, ConfirmedUserViewModel>(cityApprover),
                    IsUserHeadOfCity = await _userManagerService.IsInRoleAsync(User, "Голова Станиці"),
                    IsUserHeadOfClub = await _userManagerService.IsInRoleAsync(User, "Голова Куреня"),
                    IsUserHeadOfRegion = await _userManagerService.IsInRoleAsync(User, "Голова Округу"),
                    IsUserPlastun = await _userManagerService.IsInRoleAsync(user, "Пластун"),
                    CurrentUserId = approverId
                };
                foreach (var item in model.ConfirmedUsers)
                {
                    item.Approver.User.ImagePath = await _userService.GetImageBase64Async(item.Approver.User.ImagePath);
                }
                if (model.ClubApprover != null)
                {
                    model.ClubApprover.Approver.User.ImagePath = await _userService.GetImageBase64Async(model?.ClubApprover?.Approver?.User.ImagePath);
                }
                if (model.CityApprover != null)
                {
                    model.CityApprover.Approver.User.ImagePath = await _userService.GetImageBase64Async(model?.CityApprover?.Approver?.User.ImagePath);
                }
                return Ok(model);
            }

            _loggerService.LogError($"User not found. UserId:{userId}");
            return NotFound();
        }
        /// <summary>
        /// Approving user
        /// </summary>
        /// <param name="userId">The user ID which is confirmed</param>
        /// <param name="isClubAdmin">Confirm as an club admin</param>
        /// <param name="isCityAdmin">Confirm as an city admin</param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">User not found</response>
        [HttpPost("approveUser/{userId}/{isClubAdmin}/{isCityAdmin}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Пластун, Голова Куреня, Голова Станиці, Голова Округу, Admin")]
        public async Task<IActionResult> ApproveUser(string userId, bool isClubAdmin = false, bool isCityAdmin = false)
        {
            if (userId != null)
            {

                await _confirmedUserService.CreateAsync(User, userId, isClubAdmin, isCityAdmin);

                return Ok();
            }
            _loggerService.LogError("User id is null");

            return NotFound();
        }

        /// <summary>
        /// Delete approve
        /// </summary>
        /// <param name="confirmedId">Confirmation ID to be deleted</param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Confirmed id is 0</response>
        [HttpDelete("deleteApprove/{confirmedId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> ApproverDelete(int confirmedId)
        {
            if (confirmedId != 0)
            {
                await _confirmedUserService.DeleteAsync(confirmedId);
                _loggerService.LogInformation("Approve succesfuly deleted");

                return Ok();
            }
            _loggerService.LogError("Confirmed id is 0");
            return NotFound();
        }
    }
}
