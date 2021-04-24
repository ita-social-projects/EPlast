using EPlast.BLL.Interfaces.GoverningBodies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EPlast.BLL.DTO;
using EPlast.BLL.Interfaces.Logging;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoverningBodiesController : ControllerBase
    {
        private readonly IGoverningBodiesService _governingBodiesService;
        private readonly ILoggerService<GoverningBodiesController> _logger;
        private readonly UserManager<User> _userManager;


        public GoverningBodiesController(IGoverningBodiesService service,
            ILoggerService<GoverningBodiesController> logger,
            UserManager<User> userManager)
        {
            _governingBodiesService = service;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetGoverningBodies()
        {
            return Ok(await _governingBodiesService.GetGoverningBodiesListAsync());
        }

        [HttpPost("CreateGoverningBody")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Create(GoverningBodyDTO governingBodyDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            governingBodyDTO.Id = await _governingBodiesService.CreateAsync(governingBodyDTO);

            _logger.LogInformation($"Governing body {{{governingBodyDTO.GoverningBodyName}}} was created.");

            return Ok(governingBodyDTO.Id);
        }

        [HttpPut("EditGoverningBody/{governingBodyId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Edit(GoverningBodyDTO governingBody)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _governingBodiesService.EditAsync(governingBody);
            _logger.LogInformation($"Governing body {{{governingBody.GoverningBodyName}}} was edited.");

            return Ok();
        }

        [HttpGet("LogoBase64/{logoName}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetPhotoBase64(string logoName)
        {
            if (logoName == null)
            {
                return BadRequest(logoName);
            }
            
            return Ok(await _governingBodiesService.GetLogoBase64Async(logoName));
        }

        [HttpGet("Profile/{governingBodyId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetProfile(int governingBodyId)
        {
            var governingBodyProgileDto = await _governingBodiesService.GetProfileAsync(governingBodyId, await _userManager.GetUserAsync(User));
            if (governingBodyProgileDto == null)
            {
                return NotFound();
            }

            return Ok(governingBodyProgileDto);
        }

        [HttpDelete("RemoveGoverningBody/{governingBodyId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Remove(int governingBodyId)
        {
            await _governingBodiesService.RemoveAsync(governingBodyId);
            _logger.LogInformation($"GoverningBody with id {{{governingBodyId}}} was deleted.");

            return Ok();
        }

        [HttpGet("GetUserAccesses/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetUserAccess(string userId)
        {
            return Ok(await _governingBodiesService.GetUserAccessAsync(userId));
        }
    }
}
