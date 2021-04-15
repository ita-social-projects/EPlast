using System.Threading.Tasks;
using EPlast.BLL.Interfaces.RegionBoard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsBoardController: ControllerBase
    {
        private readonly IRegionsBoardService _regionsBoardService;

        public RegionsBoardController(IRegionsBoardService service)
        {
            _regionsBoardService = service;
        }

        [HttpGet("GetUserAccesses/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetUserAccess(string userId)
        {
            return Ok(await _regionsBoardService.GetUserAccessAsync(userId));
        }
    }
}
