using EPlast.BLL.Interfaces.UserAccess;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccessController : ControllerBase
    {
        private readonly IUserAccessService _userAccessService;
        private readonly UserManager<User> _userManager;

        public UserAccessController(IUserAccessService userAccessService, UserManager<User> userManager)
        {
            _userAccessService = userAccessService;
            _userManager = userManager;
        }

        [HttpGet("GetUserClubAccess/{clubId}/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetUserClubAccess(int clubId, string userId)
        {
            return Ok(await _userAccessService.GetUserClubAccessAsync(clubId, userId, await _userManager.GetUserAsync(User)));
        }

        [HttpGet("GetEventUserAccess/{userId}/{eventId?}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetEventUserAccess(string userId, int? eventId = null)
        {
            return Ok(await _userAccessService.GetUserEventAccessAsync(userId, await _userManager.GetUserAsync(User), eventId));
        }

        [HttpGet("GetUserCityAccess/{cityId}/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetUserCityAccess(int cityId, string userId)
        {
            return Ok(await _userAccessService.GetUserCityAccessAsync(cityId, userId, await _userManager.GetUserAsync(User)));
        }

        [HttpGet("GetUserRegionAccess/{regionId}/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetUserRegionAccess(int regionId, string userId)
        {
            return Ok(await _userAccessService.GetUserRegionAccessAsync(regionId, userId, await _userManager.GetUserAsync(User)));
        }

        [HttpGet("GetUserDistinctionAccess/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetUserDistinctionAccess(string userId)
        {
            return Ok(await _userAccessService.GetUserDistinctionAccessAsync(userId));
        }

        [HttpGet("GetUserAnnualReportAccess/{userId}/{reportType?}/{reportId?}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetUserAnnualReportAccess(string userId, ReportType? reportType = null, int? reportId = null)
        {
            return Ok(await _userAccessService.GetUserAnnualReportAccessAsync(userId, await _userManager.GetUserAsync(User), reportType, reportId));
        }

        [HttpGet("GetUserStatisticsAccess/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetUserStatisticsAccess(string userId)
        {
            return Ok(await _userAccessService.GetUserStatisticsAccessAsync(userId));
        }
    }
}
