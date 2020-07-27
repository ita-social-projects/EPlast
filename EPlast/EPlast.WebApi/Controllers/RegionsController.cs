using AutoMapper;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Interfaces.Region;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly ILoggerService<CitiesController> _logger;
        private readonly IRegionService _regionService;
        private readonly IRegionAdministrationService _regionAdministrationService;

        public RegionsController(ILoggerService<CitiesController> logger,
            IRegionService regionService,
            IRegionAdministrationService regionAdministrationService)
        {
            _logger = logger;
            _regionService = regionService;
            _regionAdministrationService = regionAdministrationService;
        }

        [HttpGet("Profiles")]
        public async Task<IActionResult> Index()
        {
            var regions = await _regionService.GetAllRegionsAsync();

            return Ok(regions);
        }

        [HttpGet("Profile/{regionId}")]
        public async Task<IActionResult> GetProfile(int regionId)
        {
            try
            {
                var region = await _regionService.GetRegionProfileByIdAsync(regionId);
                if (region == null)
                {
                    return NotFound();
                }

                return Ok(region);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        [HttpPost("AddAdministrator/{regionId}")]
        public async Task<IActionResult> AddAdministrator(RegionAdministrationDTO admin)
        {
            try
            {
                await _regionAdministrationService.AddAdministratorAsync(admin);
                _logger.LogInformation($"User {{{admin.UserId}}} became admin for region {{{admin.CityId}}}" +
                    $" with role {{{admin.AdminType.AdminTypeName}}}.");

                return Ok();
            }
            catch(Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }
    }
}
