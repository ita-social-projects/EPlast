using EPlast.BLL.DTO.ActiveMembership;
using EPlast.BLL.Interfaces.ActiveMembership;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("dergees")]
        public async Task<IActionResult> GetAllDergees()
        {
            return Ok(await _plastDegreeService.GetDergeesAsync());
        }

        [HttpGet("accessLevel/{userId}")]
        public async Task<IActionResult> GetAccessLevel(string userId)
        {
            return Ok(await _accessLevelService.GetUserAccessLevelsAsync(userId));
        }

        [HttpGet("dergees/{userId}")]
        public async Task<IActionResult> GetUserDegrees(string userId)
        {
            return Ok(await _plastDegreeService.GetUserPlastDegreesAsync(userId));
        }

        [HttpPost("dergees")]
        public async Task<IActionResult> AddPlastDegreeForUser(UserPlastDegreePostDTO userPlastDegreePostDTO)
        {
            if (await _plastDegreeService.AddPlastDegreeForUserAsync(userPlastDegreePostDTO))
            {
                return Created("GetAllDergees", userPlastDegreePostDTO.PlastDegreeId);
            }

            return BadRequest();
        }
    }
}