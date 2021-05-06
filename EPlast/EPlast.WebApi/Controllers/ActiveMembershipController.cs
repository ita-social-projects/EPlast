using EPlast.BLL.DTO.ActiveMembership;
using EPlast.BLL.Interfaces.ActiveMembership;
using EPlast.BLL.Interfaces.Logging;
using EPlast.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Microsoft.AspNetCore.Http;
using EPlast.BLL.Interfaces.UserProfiles;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ActiveMembershipController : ControllerBase
    {
        private readonly IPlastDegreeService _plastDegreeService;
        private readonly IAccessLevelService _accessLevelService;
        private readonly IUserDatesService _userDatesService;
        private readonly ILoggerService<ActiveMembershipController> _loggerService;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;

        public ActiveMembershipController(IPlastDegreeService plastDegreeService,
            IAccessLevelService accessLevelService, IUserDatesService userDatesService,
            ILoggerService<ActiveMembershipController> loggerService,
            UserManager<User> userManager, IUserService userService)
        {
            _plastDegreeService = plastDegreeService;
            _accessLevelService = accessLevelService;
            _userDatesService = userDatesService;
            _loggerService = loggerService;
            _userManager = userManager;
            _userService = userService;
        }

        private async Task<bool> HasAccessAsync(string userId)
        {
            var currentUser = await _userService.GetUserAsync((await _userManager.GetUserAsync(User)).Id);
            var focusUser = await _userService.GetUserAsync(userId);
            var roles = await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User));
            var isUserAdmin = roles.Contains(Roles.Admin);
            var isUserHeadOfCity = roles.Contains(Roles.CityHead);
            var isUserHeadOfClub = roles.Contains(Roles.KurinHead);
            var isUserHeadOfRegion = roles.Contains(Roles.OkrugaHead);
            var isUserSameCity = currentUser.CityMembers.FirstOrDefault()?.CityId
                                     .Equals(focusUser.CityMembers.FirstOrDefault()?.CityId)
                                 == true;
            var isUserSameClub = currentUser.ClubMembers.FirstOrDefault()?.ClubId
                                     .Equals(focusUser.ClubMembers.FirstOrDefault()?.ClubId)
                                 == true;
            var isUserSameRegion = currentUser.RegionAdministrations.FirstOrDefault()?.RegionId
                                       .Equals(focusUser.RegionAdministrations.FirstOrDefault()?.RegionId) == true
                                   || currentUser.CityMembers.FirstOrDefault()?.City.RegionId
                                       .Equals(focusUser.CityMembers.FirstOrDefault()?.City.RegionId) == true;
            if (isUserAdmin || (isUserHeadOfClub && isUserSameClub) || (isUserHeadOfCity && isUserSameCity) ||
                (isUserHeadOfRegion && isUserSameRegion))
                return true;
            return false;
        }

        [HttpGet("degree")]
        public async Task<IActionResult> GetAllDergees()
        {
            var temp = await _plastDegreeService.GetDergeesAsync();
            return Ok(await _plastDegreeService.GetDergeesAsync());
        }

        [HttpGet("accessLevel/{userId}")]
        public async Task<IActionResult> GetAccessLevel(string userId)
        {
            return Ok(await _accessLevelService.GetUserAccessLevelsAsync(userId));
        }

        [HttpGet("degree/{userId}")]
        public async Task<IActionResult> GetUserDegrees(string userId)
        {
            return Ok(await _plastDegreeService.GetUserPlastDegreesAsync(userId));
        }

        [Authorize(Roles = Roles.Admin+","+Roles.RegionBoardHead+","+Roles.OkrugaHead+","+Roles.CityHead)]
        [HttpPost("degree")]
        public async Task<IActionResult> AddPlastDegreeForUser(UserPlastDegreePostDTO userPlastDegreePostDTO)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (!await HasAccessAsync(userPlastDegreePostDTO.UserId))
            {
                _loggerService.LogError($"User (id: {currentUser.Id}) hasn't access to add degree (id: {userPlastDegreePostDTO.UserId})");
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            if (await _plastDegreeService.AddPlastDegreeForUserAsync(userPlastDegreePostDTO) &&
                (((await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User))).Contains(Roles.CityHead) 
                 && new List<int>() { 1, 7 }.Contains(userPlastDegreePostDTO.PlastDegreeId)) || 
                 !(await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User))).Contains(Roles.CityHead)))
            {
                return Created("GetAllDergees", userPlastDegreePostDTO.PlastDegreeId);
            }

            return BadRequest();
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.RegionBoardHead + "," + Roles.OkrugaHead)]
        [HttpDelete("degree/{userId}/{plastDegreeId}")]
        public async Task<IActionResult> DeletePlastDegreeForUser(string userId, int plastDegreeId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (!await HasAccessAsync(userId))
            {
                _loggerService.LogError($"User (id: {currentUser.Id}) hasn't access to delete degree (id: {userId})");
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            if (await _plastDegreeService.DeletePlastDegreeForUserAsync(userId, plastDegreeId))
            {
                return NoContent();
            }

            return BadRequest();
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.RegionBoardHead + "," + Roles.OkrugaHead)]
        [HttpPut("degree/setAsCurrent/{userId}/{plastDegreeId}")]
        public async Task<IActionResult> SetPlastDegreeAsCurrent(string userId, int plastDegreeId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (!await HasAccessAsync(userId))
            {
                _loggerService.LogError($"User (id: {currentUser.Id}) hasn't access to set degree as current (id: {userId})");
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            if (await _plastDegreeService.SetPlastDegreeForUserAsCurrentAsync(userId, plastDegreeId))
            {
                return NoContent();
            }

            return BadRequest();
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.RegionBoardHead + "," + Roles.OkrugaHead)]
        [HttpPut("degree/endDate")]
        public async Task<IActionResult> AddEndDatePlastDegreeForUser(UserPlastDegreePutDTO userPlastDegreePutDTO)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (!await HasAccessAsync(userPlastDegreePutDTO.UserId))
            {
                _loggerService.LogError($"User (id: {currentUser.Id}) hasn't access to add end date degree (id: {userPlastDegreePutDTO.UserId})");
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            if (await _plastDegreeService.AddEndDateForUserPlastDegreeAsync(userPlastDegreePutDTO))
            {
                return NoContent();
            }

            return BadRequest();
        }
        
        [HttpGet("dates/{userId}")]
        public async Task<IActionResult> GetUserDates(string userId)
        {
            try
            {
                return Ok(await _userDatesService.GetUserMembershipDatesAsync(userId));
            }
            catch (InvalidOperationException)
            {
                return BadRequest(userId);
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.HeadsAndAdmin)]
        [HttpPost("dates")]
        public async Task<IActionResult> ChangeUserDates(UserMembershipDatesDTO userMembershipDatesDTO)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (!await HasAccessAsync(userMembershipDatesDTO.UserId))
            {
                _loggerService.LogError($"User (id: {currentUser.Id}) hasn't access to change dates (id: {userMembershipDatesDTO.UserId})");
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            if (await _userDatesService.ChangeUserMembershipDatesAsync(userMembershipDatesDTO))
            {
                return Ok(userMembershipDatesDTO);
            }

            return BadRequest();
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("dates/AddNew/{userId}")]
        public async Task<IActionResult> InitializeUserDates(string userId)
        {
            if (await _userDatesService.AddDateEntryAsync(userId))
            {
                return Ok(userId);
            }

            return BadRequest();
        }
    }
}