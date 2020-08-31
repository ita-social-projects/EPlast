using EPlast.BLL.DTO.ActiveMembership;
using EPlast.BLL.Interfaces.ActiveMembership;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActiveMembershipController : ControllerBase
    {
        private readonly IPlastDegreeService _plastDegreeService;
        private readonly IAccessLevelService _accessLevelService;
        public ActiveMembershipController(IPlastDegreeService plastDegreeService, IAccessLevelService accessLevelService)
        {
            _plastDegreeService = plastDegreeService;
            _accessLevelService = accessLevelService;
        }

        [HttpGet("dergee")]
        public async Task<IActionResult> GetAllDergees()
        {
            return Ok(await _plastDegreeService.GetDergeesAsync());
        }

        [HttpGet("accessLevel/{userId}")]
        public async Task<IActionResult> GetAccessLevel(string userId)
        {
            return Ok(await _accessLevelService.GetUserAccessLevelsAsync(userId));
        }

        [HttpGet("dergee/{userId}")]
        public async Task<IActionResult> GetUserDegrees(string userId)
        {
            return Ok(await _plastDegreeService.GetUserPlastDegreesAsync(userId));
        }

        [HttpPost("dergee")]
        public async Task<IActionResult> AddPlastDegreeForUser(UserPlastDegreePostDTO userPlastDegreePostDTO)
        {
            if (await _plastDegreeService.AddPlastDegreeForUserAsync(userPlastDegreePostDTO))
            {
                return Created("GetAllDergees", userPlastDegreePostDTO.PlastDegreeId);
            }

            return BadRequest();
        }

        [HttpDelete("dergee/{userId}/{plastDegreeId}")]
        public async Task<IActionResult> DeletePlastDegreeForUser(string userId, int plastDegreeId)
        {
            if (await _plastDegreeService.DeletePlastDegreeForUserAsync(userId, plastDegreeId))
            {
                return NoContent();
            }

            return BadRequest();
        }
        [HttpPut("degree/setAsCurrent/{userId}/{plastDegreeId}")]
        public async Task<IActionResult> SetPlastDegreeAsCurrent(string userId, int plastDegreeId)
        {
            if(await _plastDegreeService.SetPlastDegreeForUserAsCurrentAsync(userId, plastDegreeId))
            {
                return NoContent();
            }

            return BadRequest();
        }
        [HttpPut("degree/endDate")]
        public async Task<IActionResult> AddEndDatePlastDegreeForUser(UserPlastDegreePutDTO userPlastDegreePutDTO)
        {
            if (await _plastDegreeService.AddEndDateForUserPlastDegreeAsync(userPlastDegreePutDTO))
            {
                return NoContent();
            }

            return BadRequest();
        }

    }
}