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
            var isUserHeadDeputyOfCity = roles.Contains(Roles.CityHeadDeputy);
            var isUserHeadOfClub = roles.Contains(Roles.KurinHead);
            var isUserHeadDeputyOfClub = roles.Contains(Roles.KurinHeadDeputy);
            var isUserHeadOfRegion = roles.Contains(Roles.OkrugaHead);
            var isUserHeadDeputyOfRegion = roles.Contains(Roles.OkrugaHeadDeputy);
            if (isUserAdmin 
                || (isUserHeadOfClub && _userService.IsUserSameClub(currentUser, focusUser)) 
                || (isUserHeadDeputyOfClub && _userService.IsUserSameClub(currentUser, focusUser)) 
                || (isUserHeadOfCity && _userService.IsUserSameCity(currentUser, focusUser)) 
                || (isUserHeadDeputyOfCity && _userService.IsUserSameCity(currentUser, focusUser)) 
                || (isUserHeadOfRegion && _userService.IsUserSameRegion(currentUser, focusUser))
                || (isUserHeadDeputyOfRegion && _userService.IsUserSameRegion(currentUser, focusUser)))
                return true;
            _loggerService.LogError($"No access.");
            return false;
        }

        [HttpGet("degree")]
        public async Task<IActionResult> GetAllDergees()
        {
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

        [Authorize(Roles = Roles.AdminRegionBoardHeadOkrugaCityHeadAndDeputy)]
        [HttpPost("degree")]
        public async Task<IActionResult> AddPlastDegreeForUser(UserPlastDegreePostDTO userPlastDegreePostDTO)
        {
            if (!await HasAccessAsync(userPlastDegreePostDTO.UserId))
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            if (await _plastDegreeService.AddPlastDegreeForUserAsync(userPlastDegreePostDTO) &&
                (((await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User))).Contains(Roles.CityHead)
                  && new List<int>() { 1, 7 }.Contains(userPlastDegreePostDTO.PlastDegreeId)) ||
                 !(await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User))).Contains(Roles.CityHead)))
            {
                return Created("GetAllDegrees", userPlastDegreePostDTO.PlastDegreeId);
            }
            if (await _plastDegreeService.AddPlastDegreeForUserAsync(userPlastDegreePostDTO) &&
                (((await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User))).Contains(Roles.CityHeadDeputy)
                  && new List<int>() { 1, 7 }.Contains(userPlastDegreePostDTO.PlastDegreeId)) ||
                 !(await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User))).Contains(Roles.CityHeadDeputy)))
            {
                return Created("GetAllDegrees", userPlastDegreePostDTO.PlastDegreeId);
            }

            return BadRequest();
        }

        [Authorize(Roles = Roles.AdminRegionBoardHeadOkrugaHeadAndDeputy)]
        [HttpDelete("degree/{userId}/{plastDegreeId}")]
        public async Task<IActionResult> DeletePlastDegreeForUser(string userId, int plastDegreeId)
        {
            if (!await HasAccessAsync(userId))
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            if (await _plastDegreeService.DeletePlastDegreeForUserAsync(userId, plastDegreeId))
            {
                return NoContent();
            }

            return BadRequest();
        }

        [Authorize(Roles = Roles.AdminRegionBoardHeadOkrugaHeadAndDeputy)]
        [HttpPut("degree/setAsCurrent/{userId}/{plastDegreeId}")]
        public async Task<IActionResult> SetPlastDegreeAsCurrent(string userId, int plastDegreeId)
        {
            if (!await HasAccessAsync(userId))
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            if (await _plastDegreeService.SetPlastDegreeForUserAsCurrentAsync(userId, plastDegreeId))
            {
                return NoContent();
            }

            return BadRequest();
        }

        [Authorize(Roles = Roles.AdminRegionBoardHeadOkrugaHeadAndDeputy)]
        [HttpPut("degree/endDate")]
        public async Task<IActionResult> AddEndDatePlastDegreeForUser(UserPlastDegreePutDTO userPlastDegreePutDTO)
        {
            if (!await HasAccessAsync(userPlastDegreePutDTO.UserId))
            {
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

        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.HeadsAndHeadDeputiesAndAdmin)]
        [HttpPost("dates")]
        public async Task<IActionResult> ChangeUserDates(UserMembershipDatesDTO userMembershipDatesDTO)
        {
            if (!await HasAccessAsync(userMembershipDatesDTO.UserId))
            {
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