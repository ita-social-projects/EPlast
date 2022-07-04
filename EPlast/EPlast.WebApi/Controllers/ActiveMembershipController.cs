using System;
using System.Threading.Tasks;
using EPlast.BLL.DTO.ActiveMembership;
using EPlast.BLL.Interfaces.ActiveMembership;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        private readonly ILoggerService _loggerService;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;

        public ActiveMembershipController(IPlastDegreeService plastDegreeService,
            IAccessLevelService accessLevelService, IUserDatesService userDatesService,
            ILoggerService loggerService,
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
            var isUserRegionReferentUPS = roles.Contains(Roles.OkrugaReferentUPS);
            var isUserRegionReferentUSP = roles.Contains(Roles.OkrugaReferentUSP);
            var isUserRegionReferentOfActiveMemebership = roles.Contains(Roles.OkrugaReferentOfActiveMembership);
            var isUserCityReferentUPS=roles.Contains(Roles.CityReferentUPS);
            var isUserCityReferentUSP = roles.Contains(Roles.CityReferentUSP);
            var isUserCityReferentOfActiveMemebership = roles.Contains(Roles.CityReferentOfActiveMembership);
            if (isUserAdmin
                || (isUserHeadOfClub && _userService.IsUserSameClub(currentUser, focusUser))
                || (isUserHeadDeputyOfClub && _userService.IsUserSameClub(currentUser, focusUser))
                || (isUserHeadOfCity && _userService.IsUserSameCity(currentUser, focusUser))
                || (isUserHeadDeputyOfCity && _userService.IsUserSameCity(currentUser, focusUser))
                || (isUserHeadOfRegion && _userService.IsUserSameRegion(currentUser, focusUser))
                || (isUserHeadDeputyOfRegion && _userService.IsUserSameRegion(currentUser, focusUser))
                || (isUserRegionReferentUPS && _userService.IsUserSameCity(currentUser, focusUser))
                || (isUserRegionReferentUSP && _userService.IsUserSameCity(currentUser, focusUser))
                || (isUserRegionReferentOfActiveMemebership && _userService.IsUserSameCity(currentUser, focusUser))
                || (isUserCityReferentUPS && _userService.IsUserSameCity(currentUser, focusUser))
                || (isUserCityReferentUSP && _userService.IsUserSameCity(currentUser, focusUser))
                || (isUserCityReferentOfActiveMemebership && _userService.IsUserSameCity(currentUser, focusUser))) 
                return true;
            _loggerService.LogError($"No access.");
            return false;
        }

        [HttpGet("degree")]
        public async Task<IActionResult> GetAllDergees()
        {
            return Ok(await _plastDegreeService.GetDegreesAsync());
        }

        [HttpGet("accessLevel/{userId}")]
        public async Task<IActionResult> GetAccessLevel(string userId)
        {
            return Ok(await _accessLevelService.GetUserAccessLevelsAsync(userId));
        }

        [HttpGet("degree/{userId}")]
        public async Task<IActionResult> GetUserDegree(string userId)
        {
            return Ok(await _plastDegreeService.GetUserPlastDegreeAsync(userId));
        }

        [Authorize(Roles = Roles.CanEditCity)]
        [HttpPost("degree")]
        public async Task<IActionResult> AddPlastDegreeForUser(UserPlastDegreePostDto userPlastDegreePostDTO)
        {
            if (!await HasAccessAsync(userPlastDegreePostDTO.UserId))
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            var roles = await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User));
            if(roles.Contains(Roles.Admin) || roles.Contains(Roles.OkrugaHead) || roles.Contains(Roles.OkrugaHeadDeputy))
            {
                await _plastDegreeService.AddPlastDegreeForUserAsync(userPlastDegreePostDTO);
                return Created("GetAllDegrees", userPlastDegreePostDTO.PlastDegreeId);
            }

            await _plastDegreeService.AddPlastDegreeForUserAsync(userPlastDegreePostDTO);
            return Created("GetAllDegrees", userPlastDegreePostDTO.PlastDegreeId);
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
        public async Task<IActionResult> ChangeUserEntryandOathDatesAsync(EntryAndOathDatesDto entryAndOathDatesDTO)
        {
            var focusUser = await _userManager.FindByIdAsync(entryAndOathDatesDTO.UserId);
            var roles = await _userManager.GetRolesAsync(focusUser);
            //If user is registered or former member, then we can not change his dates!
            if(roles.Contains(Roles.RegisteredUser) || roles.Count == 0 || !await HasAccessAsync(entryAndOathDatesDTO.UserId))
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            if (await _userDatesService.ChangeUserEntryAndOathDateAsync(entryAndOathDatesDTO))
            {
                return Ok(entryAndOathDatesDTO);
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