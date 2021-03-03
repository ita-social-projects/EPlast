using EPlast.BLL.DTO.ActiveMembership;
using EPlast.BLL.Interfaces.ActiveMembership;
using EPlast.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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

        public ActiveMembershipController(IPlastDegreeService plastDegreeService, IAccessLevelService accessLevelService, IUserDatesService userDatesService)
        {
            _plastDegreeService = plastDegreeService;
            _accessLevelService = accessLevelService;
            _userDatesService = userDatesService;
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

        [Authorize(Roles = Roles.degreeAssignRoles)]
        [HttpPost("degree")]
        public async Task<IActionResult> AddPlastDegreeForUser(UserPlastDegreePostDTO userPlastDegreePostDTO)
        {
            if (await _plastDegreeService.AddPlastDegreeForUserAsync(userPlastDegreePostDTO))
            {
                return Created("GetAllDergees", userPlastDegreePostDTO.PlastDegreeId);
            }

            return BadRequest();
        }

        [Authorize(Roles = Roles.degreeAssignRoles)]
        [HttpDelete("degree/{userId}/{plastDegreeId}")]
        public async Task<IActionResult> DeletePlastDegreeForUser(string userId, int plastDegreeId)
        {
            if (await _plastDegreeService.DeletePlastDegreeForUserAsync(userId, plastDegreeId))
            {
                return NoContent();
            }

            return BadRequest();
        }

        [Authorize(Roles = Roles.degreeAssignRoles)]
        [HttpPut("degree/setAsCurrent/{userId}/{plastDegreeId}")]
        public async Task<IActionResult> SetPlastDegreeAsCurrent(string userId, int plastDegreeId)
        {
            if(await _plastDegreeService.SetPlastDegreeForUserAsCurrentAsync(userId, plastDegreeId))
            {
                return NoContent();
            }

            return BadRequest();
        }

        [Authorize(Roles = Roles.degreeAssignRoles)]
        [HttpPut("degree/endDate")]
        public async Task<IActionResult> AddEndDatePlastDegreeForUser(UserPlastDegreePutDTO userPlastDegreePutDTO)
        {
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

        [Authorize(Roles = Roles.degreeAssignRoles)]
        [HttpPost("dates")]
        public async Task<IActionResult> ChangeUserDates(UserMembershipDatesDTO userMembershipDatesDTO)
        {
            if (await _userDatesService.ChangeUserMembershipDatesAsync(userMembershipDatesDTO))
            {
                return Ok(userMembershipDatesDTO);
            }

            return BadRequest();
        }

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