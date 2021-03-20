using EPlast.BLL.Interfaces.GoverningBodies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.Interfaces.Logging;
using EPlast.WebApi.Models.City;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoverningBodiesController : ControllerBase
    {
        private readonly IGoverningBodiesService _governingBodiesService;
        private readonly ILoggerService<GoverningBodiesController> _logger;
        private readonly IMapper _mapper;


        public GoverningBodiesController(IGoverningBodiesService service, ILoggerService<GoverningBodiesController> logger, IMapper mapper)
        {
            _governingBodiesService = service;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetGoverningBodies()
        {
            var governingBodies = await _governingBodiesService.GetGoverningBodiesListAsync();
            return Ok(governingBodies);
        }

        [HttpPost("CreateGoverningBody")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Create(GoverningBodyDTO governingBodyDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            governingBodyDTO.ID = await _governingBodiesService.CreateAsync(governingBodyDTO);

            _logger.LogInformation($"Governing body {{{governingBodyDTO.GoverningBodyName}}} was created.");

            return Ok(governingBodyDTO.ID);
        }

        [HttpGet("LogoBase64")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetPhotoBase64(string logoName)
        {
            var logoBase64 = await _governingBodiesService.GetLogoBase64(logoName);

            return Ok(logoBase64);
        }
    }
}
