﻿using EPlast.BLL.DTO.Region;
using EPlast.BLL.ExtensionMethods;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Interfaces.Region;
using EPlast.DataAccess.Entities;
using EPlast.WebApi.Extensions;
using EPlast.WebApi.Models.Region;
using EPlast.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [ApiController, Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class RegionsController : ControllerBase
    {
        private readonly IDistributedCache _cache;
        private readonly ILoggerService<CitiesController> _logger;
        private readonly IRegionAdministrationService _regionAdministrationService;
        private readonly IRegionAnnualReportService _RegionAnnualReportService;
        private readonly IRegionService _regionService;
        private readonly UserManager<User> _userManager;

        public RegionsController(ILoggerService<CitiesController> logger,
            IRegionService regionService,
            IRegionAdministrationService regionAdministrationService,
            IRegionAnnualReportService RegionAnnualReportService,
            UserManager<User> userManager,
            IDistributedCache cache)
        {
            _logger = logger;
            _regionService = regionService;
            _regionAdministrationService = regionAdministrationService;
            _RegionAnnualReportService = RegionAnnualReportService;
            _userManager = userManager;
            _cache = cache;
        }

        [HttpPost("AddAdministrator")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndOkrugaHeadAndOkrugaHeadDeputy)]
        public async Task<IActionResult> AddAdministrator(RegionAdministrationDTO admin)
        {
            await _regionAdministrationService.AddRegionAdministrator(admin);

            return NoContent();
        }

        [HttpPost("AddDocument")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndOkrugaHeadAndOkrugaHeadDeputy)]
        public async Task<IActionResult> AddDocument(RegionDocumentDTO document)
        {
            await _regionService.AddDocumentAsync(document);
            _logger.LogInformation($"Document with id {{{document.ID}}} was added.");

            return Ok(document);
        }

        [HttpPost("AddFollower/{regionId}/{cityId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddFollower(int regionId, int cityId)
        {
            await _regionService.AddFollowerAsync(regionId, cityId);
            return Ok();
        }

        [HttpPost("AddRegion")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndOkrugaHeadAndOkrugaHeadDeputy)]
        public async Task<IActionResult> CreateRegion(RegionDTO region)
        {
            await _regionService.AddRegionAsync(region);

            return Ok();
        }

        /// <summary>
        /// Method to create region annual report
        /// </summary>
        /// <param name="id">Region annual report identification number</param>
        /// <param name="year">Region annual report year</param>
        /// <param name="regionAnnualReportQuestions">Region annual report questions</param>
        /// <returns>Annual report</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User hasn't access to annual report</response>
        /// <response code="404">The region annual report does not exist</response>
        [HttpPost("CreateRegionAnnualReportById/{id}/{year}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndOkrugaHeadAndOkrugaHeadDeputy)]
        public async Task<IActionResult> CreateRegionAnnualReportById(int id, int year,
            [FromBody] RegionAnnualReportQuestions regionAnnualReportQuestions)
        {
            try
            {
                var annualreport =
                    await _RegionAnnualReportService.CreateByNameAsync(await _userManager.GetUserAsync(User), id, year,
                        regionAnnualReportQuestions);
                return StatusCode(StatusCodes.Status201Created, new { message = "Річний звіт округи успішно створено!", report= annualreport });
            }
            catch (NullReferenceException)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            catch (InvalidOperationException)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        [HttpPost("EditAdministrator")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndOkrugaHeadAndOkrugaHeadDeputy)]
        public async Task<IActionResult> EditAdministrator(RegionAdministrationDTO admin)
        {
            if (admin != null)
            {
                await _regionAdministrationService.EditRegionAdministrator(admin);
                _logger.LogInformation($"Successful edit Admin: {admin.UserId}");
                return NoContent();
            }
            _logger.LogError("Admin is null");

            return NotFound();
        }

        [HttpPut("EditRegion/{regId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndOkrugaHeadAndOkrugaHeadDeputy)]
        public async Task<IActionResult> EditRegion(int regId, RegionDTO region)
        {
            await _regionService.EditRegionAsync(regId, region);

            return Ok();
        }

        [HttpGet("GetAdminTypeId/{name}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<int> GetAdminTypeId(string name)
        {
            var typeId = await _regionAdministrationService.GetAdminType(name);
            return typeId;
        }

        [HttpGet("GetAdminTypes")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAdminTypes()
        {
            var types = await _regionAdministrationService.GetAllAdminTypes();
            return Ok(types);
        }

        /// <summary>
        /// Method to get all region reports that the user has access to
        /// </summary>
        /// <returns>List of annual reports</returns>
        /// <response code="200">Successful operation</response>
        [HttpGet("GetAllRegionAnnualReports")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.HeadsAndHeadDeputiesAndAdmin)]
        public async Task<IActionResult> GetAllRegionAnnualReports()
        {
            return StatusCode(StatusCodes.Status200OK,
                new { annualReports = await _RegionAnnualReportService.GetAllAsync(await _userManager.GetUserAsync(User)) });
        }

        /// <summary>
        /// Method to get all region annual reports
        /// </summary>
        /// <returns>Annual reports</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User hasn't access to annual report</response>
        /// <response code="404">The region annual report does not exist</response>
        [HttpGet("GetAllRegionsReports")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.HeadsAndHeadDeputiesAndAdmin)]
        public async Task<IActionResult> GetAllRegionsReportsAsync()
        {
            return Ok(await _RegionAnnualReportService.GetAllRegionsReportsAsync());
        }

        /// <summary>
        /// Method to get all region annual reports
        /// </summary>
        /// <param name="searchedData">Searched Data</param>
        /// <param name="page">current page on pagination</param>
        /// <param name="pageSize">number of records per page</param>
        /// <param name="sortKey">Key for sorting</param>
        /// <param name="auth">Whether to select reports of that user is author</param>
        /// <returns>RegionAnnualReportTableObject</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User hasn't access to annual report</response>
        /// <response code="404">The region annual report does not exist</response>
        [HttpGet("RegionsAnnualReports")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.HeadsAndHeadDeputiesAndAdmin)]
        public async Task<IActionResult> GetAllRegionsReportsAsync(string searchedData, int page, int pageSize, int sortKey, bool auth)
        {
            var user = await _userManager.GetUserAsync(User);
            return Ok(await _RegionAnnualReportService.GetAllRegionsReportsAsync(user,
                (await _userManager.GetRolesAsync(user)).Contains(Roles.Admin), searchedData, page, pageSize, sortKey, auth));
        }

        /// <summary>
        /// Method to edit region annual report
        /// </summary>
        /// <param name="regionAnnualReportQuestions">Region annual report questions</param>
        /// <param name="reportId">Region annual report identification number</param>
        /// <returns>Answer from backend</returns>
        /// <response code="200">Region annual report was successfully edited</response>
        /// <response code="400">Region annual report can not be edited</response>
        /// <response code="403">User hasn't access to region annual report</response>
        /// <response code="404">Region annual report does not exist</response>
        /// <response code="404">Region annual report model is not valid</response>
        [HttpPut("editReport/{reportId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndOkrugaHeadAndOkrugaHeadDeputy)]
        public async Task<IActionResult> EditRegionReport(int reportId,
            [FromBody] RegionAnnualReportQuestions regionAnnualReportQuestions)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _RegionAnnualReportService.EditAsync(reportId, regionAnnualReportQuestions);
                    _logger.LogInformation($"User (id: {(await _userManager.GetUserAsync(User)).Id}) edited annual report (id: {reportId})");
                    return StatusCode(StatusCodes.Status200OK, new { message = "Річний звіт округи змінено" });
                }
                catch (InvalidOperationException)
                {
                    _logger.LogError($"Annual report (id: {reportId}) can not be edited");
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = "Виникла помилка при внесенні змін до річного звіту округи" });
                }
                catch (NullReferenceException)
                {
                    _logger.LogError($"Annual report (id: {reportId}) not found");
                    return StatusCode(StatusCodes.Status404NotFound, new { message = "Річний звіт округи не знайдено" });
                }
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Method to get region members info
        /// </summary>
        /// <param name="page">current page on pagination</param>
        /// <param name="pageSize">number of records per page</param>
        /// <param name="regionId">Region identification number</param>
        /// <param name="year">Year of region members info</param>
        /// <returns>RegionMembersInfoTableObject</returns>
        /// <response code="200">Successful operation</response>
        [HttpGet("MembersInfo/{regionId:int}/{year:int}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminCityHeadOkrugaHeadCityHeadDeputyOkrugaHeadDeputy)]
        public async Task<IActionResult> GetRegionMembersInfo(int regionId, int year, int page, int pageSize)
        {
            return Ok(await _RegionAnnualReportService.GetRegionMembersInfoAsync(regionId, year, page, pageSize));
        }



        /// <summary>
        /// Method to confirm region annual report
        /// </summary>
        /// <param name="id">Region annual report identification number</param>
        /// <returns>Answer from backend</returns>
        /// <response code="200">Region annual report was successfully confirmed</response>
        [HttpPut("confirmReport/{id:int}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Confirm(int id)
        {
            await _RegionAnnualReportService.ConfirmAsync(id);
            _logger.LogInformation($"User (id: {(await _userManager.GetUserAsync(User)).Id}) confirmed annual report (id: {id})");
            return StatusCode(StatusCodes.Status200OK, new { message = "Річний звіт округи підтверджено" });
        }

        /// <summary>
        /// Method to cancel region annual report
        /// </summary>
        /// <param name="id">Region annual report identification number</param>
        /// <returns>Answer from backend</returns>
        /// <response code="200">Region annual report was successfully confirmed</response>
        /// <response code="404">Region annual report does not exist</response>
        [HttpPut("cancel/{id:int}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                await _RegionAnnualReportService.CancelAsync(id);
                _logger.LogInformation($"User (id: {(await _userManager.GetUserAsync(User)).Id}) canceled annual report (id: {id})");
                return StatusCode(StatusCodes.Status200OK, new { message = "Річний звіт округи скасовано" });
            }
            catch (NullReferenceException)
            {
                _logger.LogError($"Annual report (id: {id}) not found");
                return StatusCode(StatusCodes.Status404NotFound, new { message = "Річний звіт округи не знайдено" });
            }
        }

        /// <summary>
        /// Method to delete region annual report
        /// </summary>
        /// <param name="id">Region annual report identification number</param>
        /// <returns>Answer from backend</returns>
        /// <response code="200">Region annual report was successfully confirmed</response>
        /// <response code="404">Region annual report does not exist</response>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _RegionAnnualReportService.DeleteAsync(id);
                _logger.LogInformation($"User (id: {(await _userManager.GetUserAsync(User)).Id}) deleted annual report (id: {id})");
                return StatusCode(StatusCodes.Status200OK, new { message = "Річний звіт округи видалено" });
            }
            catch (NullReferenceException)
            {
                _logger.LogError($"Annual report (id: {id}) not found");
                return StatusCode(StatusCodes.Status404NotFound, new { message = "Річний звіт округи не знайдено" });
            }
        }

        [HttpGet("FileBase64/{fileName}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetFileBase64(string fileName)
        {
            var fileBase64 = await _regionService.DownloadFileAsync(fileName);
            return Ok(fileBase64);
        }

        [HttpGet("GetMembers/{regionId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetMembers(int regionId)
        {
            var members = await _regionService.GetMembersAsync(regionId);
            return Ok(members);
        }

        [HttpGet("LogoBase64")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetPhotoBase64(string logoName)
        {
            var logoBase64 = await _regionService.GetLogoBase64(logoName);

            return Ok(logoBase64);
        }

        [HttpGet("Profile/{regionId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetProfile(int regionId)
        {
            try
            {
                var region = await _regionService.GetRegionProfileByIdAsync(regionId, await _userManager.GetUserAsync(User));
                if (region == null || region.Status == BLL.DTO.RegionsStatusTypeDTO.RegionBoard)
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

        [HttpGet("GetAdministration/{regionId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetRegionAdmins(int regionId)
        {
            var Admins = await _regionAdministrationService.GetAdministrationAsync(regionId);

            return Ok(Admins);
        }

        [HttpGet("getDocs/{regionId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetRegionDocs(int regionId)
        {
            var secretaries = await _regionService.GetRegionDocsAsync(regionId);
            return Ok(secretaries);
        }

        [HttpGet("GetHead/{regionId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetRegionHead(int regionId)
        {
            var Head = await _regionAdministrationService.GetHead(regionId);

            return Ok(Head);
        }

        [HttpGet("GetHeadDeputy/{regionId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetRegionHeadDeputy(int regionId)
        {
            var HeadDeputy = await _regionAdministrationService.GetHeadDeputy(regionId);

            return Ok(HeadDeputy);
        }

        /// <summary>
        /// Get all regions using redis cache
        /// </summary>
        /// <returns>List of regions</returns>
        [HttpGet("Profiles/{page}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetRegions(int page, int pageSize, string regionName)
        {
            string recordKey = "Regions_" + DateTime.Now.ToString("yyyyMMdd_hhmm");
            IEnumerable<RegionDTO> regions = await _cache.GetRecordAsync<IEnumerable<RegionDTO>>(recordKey);

            if (regions is null)
            {
                regions = await _regionService.GetAllRegionsAsync();
                await _cache.SetRecordAsync(recordKey, regions);
            }
            var regionsViewModel = new RegionsViewModel(page, pageSize, regions, regionName, User.IsInRole(Roles.Admin));

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

        /// <summary>
        /// Get id and name from all regions that the user has access to
        /// </summary>
        /// <returns>Tuple (int, string)</returns>
        [HttpGet("RegionOptions")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetRegionsNameThatUserHasAccessTo()
        {
            return Ok(new { regions = await _RegionAnnualReportService.GetAllRegionsIdAndName(await _userManager.GetUserAsync(User)) });
        }

        /// <summary>
        /// Method to get regions board
        /// </summary>
        /// <returns>region "Крайовий Провід Пласту"</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User hasn't access to region</response>
        [HttpGet("regionsBoard")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetRegionsBoardAsync()
        {
            var desc = EnumExtensions.GetDescription(RegionsStatusType.RegionBoard);
            var user = await _userManager.GetUserAsync(User);
            var res = await _regionService.GetRegionByNameAsync(desc, user);
            return Ok(res);
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
        public async Task<IActionResult> GetReportByIdAsync(int id, int year)
        {
            return Ok(await _RegionAnnualReportService.GetReportByIdAsync(id, year));
        }

        [HttpGet("GetUserAdministrations/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetUserAdministrations(string userId)
        {
            var secretaries = await _regionAdministrationService.GetUsersAdministrations(userId);
            return Ok(secretaries);
        }

        [HttpGet("GetUserPreviousAdministrations/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetUserPrevAdministrations(string userId)
        {
            var secretaries = await _regionAdministrationService.GetUsersPreviousAdministrations(userId);
            return Ok(secretaries);
        }

        [HttpGet("Profiles")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Index()
        {
            var regions = await _regionService.GetAllRegionsAsync();
            return Ok(regions);
        }

        [HttpPut("RedirectCities/{prevRegId}/{nextRegId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> RedirectCities(int prevRegId, int nextRegId)
        {
            await _regionService.RedirectMembers(prevRegId, nextRegId);

            return Ok();
        }

        [HttpDelete("RemoveAdministration/{Id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndOkrugaHeadAndOkrugaHeadDeputy)]
        public async Task<IActionResult> Remove(int Id)
        {
            await _regionAdministrationService.DeleteAdminByIdAsync(Id);
            return Ok();
        }

        [HttpDelete("RemoveRegion/{Id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndOkrugaHeadAndOkrugaHeadDeputy)]
        public async Task<IActionResult> RemoveRegion(int Id)
        {
            var admins = await _regionAdministrationService.GetAdministrationAsync(Id);
            foreach (var admin in admins)
            {
                await _regionAdministrationService.DeleteAdminByIdAsync(admin.ID);
            }
            await _regionService.DeleteRegionByIdAsync(Id);
            return Ok();
        }

        [HttpDelete("RemoveDocument/{documentId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndOkrugaHeadAndOkrugaHeadDeputy)]
        public async Task<IActionResult> RemoveDocument(int documentId)
        {
            await _regionService.DeleteFileAsync(documentId);
            _logger.LogInformation($"Document with id {{{documentId}}} was deleted.");

            return Ok();
        }
        [HttpGet("RegionUsers/{regionId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetRegionUsers(int regionId)
        {
            var regionUsers = await _regionService.GetRegionUsersAsync(regionId);

            return Ok(regionUsers);
        }
    }
}
