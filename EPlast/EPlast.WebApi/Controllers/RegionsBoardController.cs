using EPlast.BLL.Interfaces.Region;
using EPlast.BLL.Interfaces.RegionBoard;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsBoardController: ControllerBase
    {
        private readonly IRegionsBoardService _regionsBoardService;
        private readonly IRegionService _regionService;

        public RegionsBoardController(IRegionsBoardService regionsBoardService, IRegionService regionService)
        {
            _regionsBoardService = regionsBoardService;
            _regionService = regionService;
        }

        [HttpGet("GetUserAccesses/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetUserAccess(string userId)
        {
            return Ok(await _regionsBoardService.GetUserAccessAsync(userId));
        }

        [HttpGet("getDocs/{regionId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetRegionDocs(int regionId)
        {
            var secretaries = await _regionService.GetRegionDocsAsync(regionId);
            return Ok(secretaries);
        }
    }
}
