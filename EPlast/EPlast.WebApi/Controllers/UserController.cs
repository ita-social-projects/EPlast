using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Interfaces.UserAccess;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using EPlast.WebApi.Models.Approver;
using EPlast.WebApi.Models.User;
using EPlast.WebApi.Models.UserModels;
using EPlast.WebApi.Models.UserModels.UserProfileFields;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserManagerService _userManagerService;
        private readonly IConfirmedUsersService _confirmedUserService;
        private readonly IUserPersonalDataService _userPersonalDataService;
        private readonly ILoggerService<UserController> _loggerService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IUserAccessService _userAccessService;

        public UserController(IUserService userService,
            IUserPersonalDataService userPersonalDataService,
            IConfirmedUsersService confirmedUserService,
            IUserManagerService userManagerService,
            ILoggerService<UserController> loggerService,
            IMapper mapper, UserManager<User> userManager, IUserAccessService userAccessService)
        {
            _userService = userService;
            _userPersonalDataService = userPersonalDataService;
            _confirmedUserService = confirmedUserService;
            _userManagerService = userManagerService;
            _loggerService = loggerService;
            _mapper = mapper;
            _userManager = userManager;
            _userAccessService = userAccessService;
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

            var currentUserId = _userManager.GetUserId(User);
            var currentUser = await _userService.GetUserAsync(currentUserId);
            var user = await _userService.GetUserAsync(userId);
            if (user != null)
            {
                var isThisUser = currentUserId == userId;
                var time = _userService.CheckOrAddPlastunRole(user.Id, user.RegistredOn);
                var isUserPlastun = await _userManagerService.IsInRoleAsync(user, Roles.PlastMember)
                    || user.UserProfile.UpuDegreeID != 1
                    || !(await _userManagerService.IsInRoleAsync(user, Roles.Supporter)
                    && await _userService.IsApprovedCityMember(userId));

                if (await _userManagerService.IsInRoleAsync(currentUser, Roles.RegisteredUser) && !isThisUser)
                {
                    _loggerService.LogError($"User (id: {currentUserId}) hasn't access to profile (id: {userId})");
                    return StatusCode(StatusCodes.Status403Forbidden);
                }

                var model = new PersonalDataViewModel
                {
                    User = _mapper.Map<UserDto, UserViewModel>(user),
                    TimeToJoinPlast = (int)time.TotalDays,
                    IsUserPlastun = isUserPlastun,
                };

                return Ok(model);
            }

            _loggerService.LogError($"User not found. UserId:{userId}");
            return NotFound();
        }

        /// <summary>
        /// Adds or updates a comment for the specified user
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="comment">The text of the comment</param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">User not found</response>
        /// <response code="400">The incoming userId is not valid</response>
        [HttpPut("{userId}/comment")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Roles = Roles.AdminAndAdminsOfOkrugaAndKrayuAndCityAndKurin)]
        public async Task<IActionResult> PutComment(string userId, [FromBody] string comment)
        {
            if (string.IsNullOrEmpty(userId)) return BadRequest();

            var user = await _userManagerService.FindByIdAsync(userId);
            if (user == null) return NotFound();

            await _userService.UpdateUserComment(user, comment);
            return Ok();
        }

        /// <summary>
        /// Get a specify user profile
        /// </summary>
        /// <param name="focusUserId">The id of the focus user</param>
        /// <returns>A focus user profile</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Focus user not found</response>
        [HttpGet("UserProfile/{focusUserId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetUserProfile([Required]string focusUserId)
        {
            var focusUser = await _userService.GetUserAsync(focusUserId);
            if (focusUser == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);

            var currentUserAccess = await 
                _userAccessService.GetUserProfileAccessAsync(currentUser.Id, focusUserId, currentUser);

            var timeToJoinPlast = _userService.CheckOrAddPlastunRole(focusUser.Id, focusUser.RegistredOn);
            var isFocusUserPlastMember = await _userManagerService.IsInRoleAsync(focusUser, Roles.PlastMember);

            if (currentUserAccess["CanViewUserFullProfile"])
            {
                var viewModel = new PersonalDataViewModel
                {
                    User = _mapper.Map<UserDto, UserViewModel>(focusUser),
                    TimeToJoinPlast = (int)timeToJoinPlast.TotalDays,
                    IsUserPlastun = isFocusUserPlastMember
                };
                return Ok(viewModel);
            }
            else
            {
                var viewModel = new PersonalDataViewModel
                {
                    ShortUser = _mapper.Map<UserDto, UserShortViewModel>(focusUser),
                    TimeToJoinPlast = (int)timeToJoinPlast.TotalDays,
                    IsUserPlastun = isFocusUserPlastMember
                };
                return Ok(viewModel);
            }
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
            var currentUserId = _userManager.GetUserId(User);
            var currentUser = await _userService.GetUserAsync(currentUserId);
            var currentUserAccess = await _userAccessService.GetUserProfileAccessAsync(currentUserId, userId, _mapper.Map<UserDto, User>(currentUser));
            if (!currentUserAccess["CanEditUserProfile"])
            {
                _loggerService.LogError($"User (id: {currentUserId}) hasn't access to edit profile (id: {userId})");
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            if (user != null)
            {
                var genders = _mapper.Map<IEnumerable<GenderDto>, IEnumerable<GenderViewModel>>(await _userPersonalDataService.GetAllGendersAsync());
                var placeOfStudyUnique = _mapper.Map<IEnumerable<EducationDto>, IEnumerable<EducationViewModel>>(await _userPersonalDataService.GetAllEducationsGroupByPlaceAsync());
                var specialityUnique = _mapper.Map<IEnumerable<EducationDto>, IEnumerable<EducationViewModel>>(await _userPersonalDataService.GetAllEducationsGroupBySpecialityAsync());
                var placeOfWorkUnique = _mapper.Map<IEnumerable<WorkDto>, IEnumerable<WorkViewModel>>(await _userPersonalDataService.GetAllWorkGroupByPlaceAsync());
                var positionUnique = _mapper.Map<IEnumerable<WorkDto>, IEnumerable<WorkViewModel>>(await _userPersonalDataService.GetAllWorkGroupByPositionAsync());
                var upuDegrees = _mapper.Map<IEnumerable<UpuDegreeDto>, IEnumerable<UpuDegreeViewModel>>(await _userPersonalDataService.GetAllUpuDegreesAsync());

                var educView = new UserEducationViewModel { PlaceOfStudyID = user.UserProfile.EducationId, SpecialityID = user.UserProfile.EducationId, PlaceOfStudyList = placeOfStudyUnique, SpecialityList = specialityUnique };
                var workView = new UserWorkViewModel { PlaceOfWorkID = user.UserProfile.WorkId, PositionID = user.UserProfile.WorkId, PlaceOfWorkList = placeOfWorkUnique, PositionList = positionUnique };
                var model = new EditUserViewModel()
                {
                    User = _mapper.Map<UserDto, UserViewModel>(user),
                    Nationalities = _mapper.Map<IEnumerable<NationalityDto>, IEnumerable<NationalityViewModel>>(await _userPersonalDataService.GetAllNationalityAsync()),
                    Religions = _mapper.Map<IEnumerable<ReligionDto>, IEnumerable<ReligionViewModel>>(await _userPersonalDataService.GetAllReligionsAsync()),
                    EducationView = educView,
                    WorkView = workView,
                    Degrees = _mapper.Map<IEnumerable<DegreeDto>, IEnumerable<DegreeViewModel>>(await _userPersonalDataService.GetAllDegreesAsync()),
                    Genders = genders,
                    UpuDegrees = upuDegrees,
                };

                return Ok(model);
            }
            _loggerService.LogError($"User not found. UserId:{userId}");
            return NotFound();
        }

        [HttpPut("photo/{userid}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> EditProfilePhotoAsync(string userid, [FromBody] string imageBase64)
        {
            try
            {
                var currentUserId = _userManager.GetUserId(User);
                var currentUser = await _userService.GetUserAsync(currentUserId);
                var userToUpdate = await _userManagerService.FindByIdAsync(userid);
                var currentUserAccess = await _userAccessService.GetUserProfileAccessAsync(currentUserId, userToUpdate.Id, _mapper.Map<UserDto, User>(currentUser));

                if (currentUserAccess["CanEditUserPhoto"])
                {
                    await _userService.UpdatePhotoAsyncForBase64(userToUpdate, imageBase64);
                    _loggerService.LogInformation($"Photo of user {userid} was successfully updated");
                    return Ok("Photo successfully updated");
                }

                _loggerService.LogInformation($"User {currentUserId} cannot update profile photo of user {userid}");
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            catch (Exception ex)
            {
                _loggerService.LogError($"Cannot update user photo because: {ex.Message}");
                return BadRequest();
            }
        }

        /// <summary>
        /// Edit a user
        /// </summary>
        /// <param name="model">Edit model</param>
        /// <response code="200">Successful operation</response>
        /// <response code="400">Bad request</response>
        /// <response code="403">Access denied</response>
        [HttpPut("editbase64")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> EditBase64([FromBody] EditUserViewModel model)
        {
            try
            {
                var currentUserId = _userManager.GetUserId(User);
                var currentUser = await _userService.GetUserAsync(currentUserId);
                var currentUserAccess = await _userAccessService.GetUserProfileAccessAsync(currentUserId, model.User.ID, _mapper.Map<UserDto, User>(currentUser));

                if (currentUserAccess["CanEditUserProfile"])
                {
                    await _userService.UpdateAsyncForBase64(_mapper.Map<UserViewModel, UserDto>(model.User),
                        model.ImageBase64, model.EducationView.PlaceOfStudyID, model.EducationView.SpecialityID,
                        model.WorkView.PlaceOfWorkID, model.WorkView.PositionID);
                    _loggerService.LogInformation($"User profile {model.User.ID} was edited  and saved in the database");
                    return Ok("Profile successfully edited");
                }
                else
                {
                    _loggerService.LogInformation($"User {currentUserId} cannot edit user profile of user {model.User.ID}");
                    return StatusCode(StatusCodes.Status403Forbidden);
                }
            }
            catch (Exception ex)
            {
                _loggerService.LogInformation($"Cannot update user because: {ex.Message}");
                return BadRequest();
            }
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

            UserDto user;

            try
            {
                user = await _userService.GetUserAsync(userId);
            }
            catch (Exception)
            {
                _loggerService.LogError($"User not found. UserId:{userId}");
                return NotFound();
            }
            var currentUserId = _userManager.GetUserId(User);
            var currentUser = await _userService.GetUserAsync(currentUserId);
            var userRoles = (await _userManagerService.GetRolesAsync(user)).ToList();
            var confirmedUsers = _userService.GetConfirmedUsers(user);
            var canApprove = !userRoles.Any(r => r == Roles.RegisteredUser || r == Roles.FormerPlastMember);
            var canApprovePlastMember = canApprove && _userService.CanApprove(confirmedUsers, userId, currentUserId,
                    await _userManagerService.IsInRoleAsync(currentUser, new string[] { Roles.Admin, Roles.GoverningBodyAdmin }));
            var time = _userService.CheckOrAddPlastunRole(user.Id, user.RegistredOn);
            var clubApprover = _userService.GetClubAdminConfirmedUser(user);
            var cityApprover = _userService.GetCityAdminConfirmedUser(user);
            var userViewModel = _mapper.Map<UserDto, UserInfoViewModel>(user);
            var userApproverViewModel = _mapper.Map<UserDto, UserInfoViewModel>(currentUser);
            var isUserHeadOfClub =
                await _userManagerService.IsInRoleAsync(
                    _mapper.Map<User, UserDto>(await _userManager.GetUserAsync(User)), Roles.KurinHead);
            var isUserHeadDeputyOfClub =
                await _userManagerService.IsInRoleAsync(
                    _mapper.Map<User, UserDto>(await _userManager.GetUserAsync(User)), Roles.KurinHeadDeputy);
            var canApproveClubMember = ((clubApprover == null) && (userViewModel.ClubId == userApproverViewModel.ClubId
                                                                   && canApprove && userId != currentUserId
                                                                   && (isUserHeadOfClub ||
                                                                       isUserHeadDeputyOfClub))) ||
                                       await _userManagerService.IsInRoleAsync(currentUser, new string[] { Roles.Admin, Roles.GoverningBodyAdmin });
            var model = new UserApproversViewModel
            {
                User = userViewModel,
                CanApproveClubMember = canApproveClubMember,
                CanApprove = canApprove,
                CanApprovePlastMember = canApprovePlastMember,
                TimeToJoinPlast = ((int)time.TotalDays),
                ConfirmedUsers = _mapper.Map<IEnumerable<ConfirmedUserDto>, IEnumerable<ConfirmedUserViewModel>>(confirmedUsers),
                ClubApprover = _mapper.Map<ConfirmedUserDto, ConfirmedUserViewModel>(clubApprover),
                CityApprover = _mapper.Map<ConfirmedUserDto, ConfirmedUserViewModel>(cityApprover),
                IsUserHeadOfCity = await _userManagerService.IsInRoleAsync(_mapper.Map<User, UserDto>(await _userManager.GetUserAsync(User)), Roles.CityHead),
                IsUserHeadDeputyOfCity = await _userManagerService.IsInRoleAsync(_mapper.Map<User, UserDto>(await _userManager.GetUserAsync(User)), Roles.CityHeadDeputy),
                IsUserHeadOfClub = isUserHeadOfClub,
                IsUserHeadDeputyOfClub = isUserHeadDeputyOfClub,
                IsUserHeadOfRegion = await _userManagerService.IsInRoleAsync(_mapper.Map<User, UserDto>(await _userManager.GetUserAsync(User)), Roles.OkrugaHead),
                IsUserHeadDeputyOfRegion = await _userManagerService.IsInRoleAsync(_mapper.Map<User, UserDto>(await _userManager.GetUserAsync(User)), Roles.OkrugaHeadDeputy),
                IsUserPlastun = await _userManagerService.IsInRoleAsync(user, Roles.PlastMember)
                    || user.UserProfile.UpuDegreeID != 1
                    || !(await _userManagerService.IsInRoleAsync(user, Roles.Supporter)
                    && await _userService.IsApprovedCityMember(userId)),
                CurrentUserId = approverId
            };
            foreach (var item in model.ConfirmedUsers.Select(x => x.Approver.User))
            {
                item.ImagePath = await _userService.GetImageBase64Async(item.ImagePath);
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
        /// <summary>
        /// Approving user
        /// </summary>
        /// <param name="userId">The user ID which is confirmed</param>
        /// <param name="approveType">Approve type</param>
        /// <response code="200">Successful operation</response>
        /// <responce code="403">Access denied</responce>
        /// <response code="404">User not found</response>
        [HttpPost("approveUser/{userId}/{approveType}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.HeadsAndHeadDeputiesAndAdminAndPlastun)]
        public async Task<IActionResult> ApproveUser(string userId, ApproveType approveType)
        {
            if (userId != null)
            {
                var userRoles = await _userManagerService.GetRolesAsync(await _userService.GetUserAsync(userId));
                if (!userRoles.Any(r => r == Roles.RegisteredUser || r == Roles.FormerPlastMember))
                {
                    await _confirmedUserService.CreateAsync(await _userManager.GetUserAsync(User), userId, approveType);
                    return Ok();
                }
                _loggerService.LogError("Forbidden");
                return StatusCode(StatusCodes.Status403Forbidden);
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
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.HeadsAndHeadDeputiesAndAdminAndPlastun)]
        public async Task<IActionResult> ApproverDelete(int confirmedId)
        {
            if (confirmedId != 0)
            {
                await _confirmedUserService.DeleteAsync(await _userManager.GetUserAsync(User), confirmedId);
                _loggerService.LogInformation("Approve succesfuly deleted");
                return Ok();
            }
            _loggerService.LogError("Confirmed id is 0");
            return NotFound();
        }
    }
}
