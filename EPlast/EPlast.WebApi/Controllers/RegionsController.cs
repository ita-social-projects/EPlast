using AutoMapper;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Interfaces.Region;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
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

        [HttpPost("AddRegion")]
        public async Task<IActionResult> CreateRegion(RegionDTO region)
        {

           await  _regionService.AddRegionAsync(region);

            return Ok();
        }

        [HttpPut("EditRegion/{regId}")]
        public async Task<IActionResult> EditRegion(int regId, RegionDTO region)
        {
            await _regionService.EditRegionAsync(regId, region);

            return Ok();
        }




        [HttpGet("GetAdministration/{regionId}")]
        public async Task<IActionResult> GetRegionAdmins(int regionId)
        {
            var Admins = await _regionService.GetAdministrationAsync(regionId);
            return Ok(Admins);
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


        [HttpDelete("RemoveRegion/{Id}")]
        public async Task<IActionResult> Remove(int Id)
        {
            await _regionService.DeleteRegionByIdAsync(Id);
            return Ok();
        }




        [HttpPost("AddFollower/{regionId}/{cityId}")]
        public async Task<IActionResult> AddFollower(int regionId, int cityId)
        {
            await _regionService.AddFollowerAsync(regionId, cityId);
            return Ok();
        }


        [HttpGet("GetMembers/{regionId}")]
        public async Task<IActionResult> GetMembers(int regionId)
        {
          var members =   await _regionService.GetMembersAsync(regionId);
            return Ok(members);
        }


    }
}
