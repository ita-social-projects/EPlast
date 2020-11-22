using EPlast.BLL.DTO.Region;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Interfaces.Region;
using EPlast.WebApi.Models.Region;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly IRegionAnnualReportService _RegionAnnualReportService;

        public RegionsController(ILoggerService<CitiesController> logger,
            IRegionService regionService,
            IRegionAnnualReportService RegionAnnualReportService)
        {
            _logger = logger;
            _regionService = regionService;
            _RegionAnnualReportService = RegionAnnualReportService;

        }

        [HttpGet("Profiles")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Index()
        {
            var regions = await _regionService.GetAllRegionsAsync();

            return Ok(regions);
        }

        [HttpPost("AddRegion")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Голова Округу")]
        public async Task<IActionResult> CreateRegion(RegionDTO region)
        {
            await _regionService.AddRegionAsync(region);

            return Ok();
        }

        [HttpPut("EditRegion/{regId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Голова Округу")]
        public async Task<IActionResult> EditRegion(int regId, RegionDTO region)
        {
            await _regionService.EditRegionAsync(regId, region);

            return Ok();
        }


        [HttpPut("RedirectCities/{prevRegId}/{nextRegId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> RedirectCities(int prevRegId, int nextRegId)
        {
            await _regionService.RedirectMembers(prevRegId, nextRegId);

            return Ok();
        }


        [HttpGet("LogoBase64")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetPhotoBase64(string logoName)
        {
            var logoBase64 = await _regionService.GetLogoBase64(logoName);

            return Ok(logoBase64);
        }



        [HttpGet("GetAdministration/{regionId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetRegionAdmins(int regionId)
        {
            var Admins = await _regionService.GetAdministrationAsync(regionId);

            return Ok(Admins);
        }



        [HttpGet("GetHead/{regionId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetRegionHead(int regionId)
        {
            var Head = await _regionService.GetHead(regionId);

            return Ok(Head);
        }




        [HttpGet("Profile/{regionId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetProfile(int regionId)
        {
            try
            {
                var region = await _regionService.GetRegionProfileByIdAsync(regionId, User);
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

        [HttpPost("AddAdministrator")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Голова Округу")]
        public async Task<IActionResult> AddAdministrator(RegionAdministrationDTO admin)
        {
            await _regionService.AddRegionAdministrator(admin);

            return NoContent();
        }

        [HttpPost("EditAdministrator")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Голова Округу")]
        public async Task<IActionResult> EditAdministrator(RegionAdministrationDTO admin)
        {
            if(admin != null)
            {
                await _regionService.EditRegionAdministrator(admin);
                _logger.LogInformation($"Successful edit admin: {admin.UserId}");
                return NoContent();
            }
            _logger.LogError("Admin is null");

            return NotFound();
        }


        [HttpGet("Profiles/{page}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetRegions(int page, int pageSize, string regionName)
        {
            var regions = await _regionService.GetAllRegionsAsync();
            var regionsViewModel = new RegionsViewModel(page, pageSize, regions, regionName, User.IsInRole("Admin"));

            return Ok(regionsViewModel);
        }

        /// <summary>
        /// Get all regions
        /// </summary>
        /// <returns>List of regions</returns>
        [HttpGet("Regions")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetRegions()
        {
            var regions = await _regionService.GetRegions();
            return Ok(regions);
        }



        [HttpDelete("RemoveAdministration/{Id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Голова Округу")]
        public async Task<IActionResult> Remove(int Id)
        {
            await _regionService.DeleteAdminByIdAsync(Id);
            return Ok();
        }


        [HttpPost("AddDocument")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Голова Округу")]
        public async Task<IActionResult> AddDocument(RegionDocumentDTO document)
        {
            await _regionService.AddDocumentAsync(document);
            _logger.LogInformation($"Document with id {{{document.ID}}} was added.");

            return Ok(document);
        }


        [HttpDelete("RemoveRegion/{Id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Голова Округу")]
        public async Task<IActionResult> RemoveAdmin(int Id)
        {
            await _regionService.DeleteRegionByIdAsync(Id);
            return Ok();
        }

        [HttpGet("GetUserAdministrations/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetUserAdministrations(string userId)
        {
            var secretaries = await _regionService.GetUsersAdministrations(userId);
            return Ok(secretaries);

        }

        [HttpGet("GetUserPreviousAdministrations/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetUserPrevAdministrations(string userId)
        {
            var secretaries = await _regionService.GetUsersPreviousAdministrations(userId);
            return Ok(secretaries);

        }


        [HttpDelete("RemoveDocument/{documentId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Голова Округу")]
        public async Task<IActionResult> RemoveDocument(int documentId)
        {
            await _regionService.DeleteFileAsync(documentId);
            _logger.LogInformation($"Document with id {{{documentId}}} was deleted.");

            return Ok();
        }




        [HttpGet("FileBase64/{fileName}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetFileBase64(string fileName)
        {
            var fileBase64 = await _regionService.DownloadFileAsync(fileName);

            return Ok(fileBase64);
        }



        [HttpGet("getDocs/{regionId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetRegionDocs(int regionId)
        {
            var secretaries = await _regionService.GetRegionDocsAsync(regionId);
            return Ok(secretaries);

        }


        [HttpPost("AddFollower/{regionId}/{cityId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddFollower(int regionId, int cityId)
        {
            await _regionService.AddFollowerAsync(regionId, cityId);
            return Ok();
        }


        [HttpGet("GetMembers/{regionId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetMembers(int regionId)
        {
            var members = await _regionService.GetMembersAsync(regionId);
            return Ok(members);
        }


        [HttpGet("GetAdminTypes")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAdminTypes()
        {
            var types = await _regionService.GetAllAdminTypes();
            return Ok(types);
        }


        [HttpGet("GetAdminTypeId/{name}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<int> GetAdminTypeId(string name)
        {
            var typeId = await _regionService.GetAdminType(name);
            return typeId;
        }

        /// <summary>
        /// Method to get all region reports that the user has access to
        /// </summary>
        /// <returns>List of annual reports</returns>
        /// <response code="200">Successful operation</response>
        [HttpGet("GetAllRegionAnnualReports")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Голова Округу")]
        public async Task<IActionResult> GetAllRegionAnnualReports()
        {
            return StatusCode(StatusCodes.Status200OK,
                new { annualReports = await _RegionAnnualReportService.GetAllAsync(User) });
        }

        /// <summary>
        /// Method to create region annual report
        /// </summary>
        /// <param name="id">Region annual report identification number</param>
        /// <param name="year">Region annual report year</param>
        /// <returns>Annual report</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User hasn't access to annual report</response>
        /// <response code="404">The region annual report does not exist</response>
        [HttpPost("CreateRegionAnnualReportById/{id}/{year}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Голова Округу")]
        public async Task<IActionResult> CreateRegionAnnualReportById(int id, int year)
        {
            try
            {
                var annualreport = await _RegionAnnualReportService.CreateByNameAsync(User, id, year);
                return StatusCode(StatusCodes.Status200OK, annualreport);
            }
            catch (NullReferenceException)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
        }

        /// <summary>
        /// Method to get region annual report by id
        /// </summary>
        /// <param name="id">Region annual report identification number</param>
        /// <param name="year">Region annual report year</param>
        /// <returns>Annual report</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User hasn't access to annual report</response>
        /// <response code="404">The region annual report does not exist</response>
        [HttpGet("GetReportById/{id}/{year}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetReportByIdAsync(int id, int year)
        {
            return Ok(await _RegionAnnualReportService.GetReportByIdAsync(id, year));
        }

        /// <summary>
        /// Method to get all region annual reports
        /// </summary>
        /// <returns>Annual reports</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User hasn't access to annual report</response>
        /// <response code="404">The region annual report does not exist</response>
        [HttpGet("GetAllRegionsReports")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAllRegionsReportsAsync()
        {
            return Ok(await _RegionAnnualReportService.GetAllRegionsReportsAsync());
        }

    }
}
