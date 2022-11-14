using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Region;
using EPlast.BLL.ExtensionMethods;
using EPlast.BLL.Interfaces.Cache;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Interfaces.Region;
using EPlast.BLL.Interfaces.RegionAdministrations;
using EPlast.BLL.Interfaces.UserAccess;
using EPlast.BLL.Queries.Region;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EPlast.WebApi.Controllers
{
    [ApiController, Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class RegionsController : ControllerBase
    {
        private readonly ICacheService _cache;
        private readonly IRegionAdministrationService _regionAdministrationService;
        private readonly IRegionAnnualReportService _regionAnnualReportService;
        private readonly IRegionService _regionService;
        private readonly UserManager<User> _userManager;
        private readonly IMediator _mediator;
        private readonly IUserAccessService _userAccessService;

        private const string ActiveRegionsCacheKey = "ActiveRegions";
        private const string ArchivedRegionsCacheKey = "ArchivedRegions";


        public RegionsController(ICacheService cache,
            IRegionService regionService,
            IRegionAdministrationService regionAdministrationService,
            IRegionAnnualReportService regionAnnualReportService,
            UserManager<User> userManager,
            IMediator mediator, IUserAccessService userAccessService)
        {
            _cache = cache;
            _regionService = regionService;
            _regionAdministrationService = regionAdministrationService;
            _regionAnnualReportService = regionAnnualReportService;
            _userManager = userManager;
            _mediator = mediator;
            _userAccessService = userAccessService;
        }

        [HttpPost("AddAdministrator")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndOkrugaHeadAndOkrugaHeadDeputy)]
        public async Task<IActionResult> AddAdministrator(RegionAdministrationDto admin)
        {
            await _regionAdministrationService.AddRegionAdministrator(admin);

            return Ok(admin);
        }

        [HttpPost("AddDocument")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndOkrugaHeadAndOkrugaHeadDeputy)]
        public async Task<IActionResult> AddDocument(RegionDocumentDto document)
        {
            try
            {
                await _regionService.AddDocumentAsync(document);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

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
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndGBAdmin)]
        public async Task<IActionResult> CreateRegion(RegionDto region)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _regionService.AddRegionAsync(region);
            await _cache.RemoveRecordsByPatternAsync(ActiveRegionsCacheKey);
            await _cache.RemoveRecordsByPatternAsync(ArchivedRegionsCacheKey);
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
                    await _regionAnnualReportService.CreateByNameAsync(await _userManager.GetUserAsync(User), id, year,
                        regionAnnualReportQuestions);
                return StatusCode(StatusCodes.Status201Created, new { message = "Річний звіт округи успішно створено!", report = annualreport });
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
        public async Task<IActionResult> EditAdministrator(RegionAdministrationDto admin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var regionAdministration = await _regionAdministrationService.GetRegionAdministrationByIdAsync(admin.ID);
            if (regionAdministration == null)
            {
                return NotFound();
            }

            var userAccess = await _userAccessService.GetUserRegionAdministrationAccessAsync(regionAdministration,
                await _userManager.GetUserAsync(User));
            if (userAccess["EditRegionHead"])
            {
                var updatedAdmin = await _regionAdministrationService.EditRegionAdministrator(admin);
                return Ok(updatedAdmin);
            }

            return StatusCode(StatusCodes.Status403Forbidden);
        }

        [HttpPut("EditRegion/{regId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndOkrugaHeadAndOkrugaHeadDeputy)]
        public async Task<IActionResult> EditRegion(int regId, RegionDto region)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _regionService.EditRegionAsync(regId, region);
            await _cache.RemoveRecordsByPatternAsync(ActiveRegionsCacheKey);
            await _cache.RemoveRecordsByPatternAsync(ArchivedRegionsCacheKey);
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
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndOkrugaHeadAndOkrugaHeadDeputy)]
        public async Task<IActionResult> GetAllRegionAnnualReports()
        {
            return StatusCode(StatusCodes.Status200OK,
                new { annualReports = await _regionAnnualReportService.GetAllAsync(await _userManager.GetUserAsync(User)) });
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
            return Ok(await _regionAnnualReportService.GetAllRegionsReportsAsync());
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
            User user = await _userManager.GetUserAsync(User);
            IList<string> userRoles = await _userManager.GetRolesAsync(user);

            bool isAdmin = userRoles.Contains(Roles.Admin) || userRoles.Contains(Roles.GoverningBodyAdmin);
            return base.Ok(await _regionAnnualReportService.GetAllRegionsReportsAsync(
                user,
                isAdmin,
                searchedData,
                page,
                pageSize,
                sortKey,
                auth
            ));
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
                    await _regionAnnualReportService.EditAsync(await _userManager.GetUserAsync(User), reportId, regionAnnualReportQuestions);
                    return StatusCode(StatusCodes.Status200OK, new { message = "Річний звіт округи змінено" });
                }
                catch (InvalidOperationException)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = "Виникла помилка при внесенні змін до річного звіту округи" });
                }
                catch (NullReferenceException)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { message = "Річний звіт округи не знайдено" });
                }
                catch (UnauthorizedAccessException)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new { message = "Користувач не має права редагувати річний звіт" });
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
            return Ok(await _regionAnnualReportService.GetRegionMembersInfoAsync(regionId, year, page, pageSize));
        }



        /// <summary>
        /// Method to confirm region annual report
        /// </summary>
        /// <param name="id">Region annual report identification number</param>
        /// <returns>Answer from backend</returns>
        /// <response code="200">Region annual report was successfully confirmed</response>
        [HttpPut("confirmReport/{id:int}")]
        [Authorize(Roles = Roles.AdminAndGBAdmin)]
        public async Task<IActionResult> Confirm(int id)
        {
            await _regionAnnualReportService.ConfirmAsync(id);
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
        [Authorize(Roles = Roles.AdminAndGBAdmin)]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                await _regionAnnualReportService.CancelAsync(id);
                return StatusCode(StatusCodes.Status200OK, new { message = "Річний звіт округи скасовано" });
            }
            catch (NullReferenceException)
            {
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
        [Authorize(Roles = Roles.AdminAndGBAdmin)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _regionAnnualReportService.DeleteAsync(id);
                return StatusCode(StatusCodes.Status200OK, new { message = "Річний звіт округи видалено" });
            }
            catch (NullReferenceException)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { message = "Річний звіт округи не знайдено" });
            }
        }

        [HttpGet("FileBase64/{fileName}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminPlastMemberAndSupporter)]
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

        /// <summary>
        /// Method to get all region followers
        /// </summary>
        /// <param name="regionId">Region identification number Data</param>
        /// <returns>Region followers</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User hasn't access to region followers page</response>
        /// <response code="404">The region followers page does not exist</response>
        [HttpGet("Followers/{regionId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetFollowers(int regionId)
        {
            var followers = await _regionService.GetFollowersAsync(regionId);
            return Ok(followers);
        }

        /// <summary>
        /// Get a specific follower of the region
        /// </summary>
        /// <param name="followerId">The id of the follower</param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Follower not found</response>
        [HttpGet("GetFollower/{followerId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndOkrugaHeadAndOkrugaHeadDeputy)]
        public async Task<IActionResult> GetFollower(int followerId)
        {
            var follower = await _regionService.GetFollowerAsync(followerId);

            if (follower == null)
            {
                return NotFound();
            }

            var user = await  _userManager.GetUserAsync(User);
            var userAccess = await _userAccessService.GetUserRegionAccessAsync(follower.RegionId, user.Id, user);
            if (userAccess["EditRegion"])
            {
                return Ok(follower);
            }

            return StatusCode(StatusCodes.Status403Forbidden);
        }

        /// <summary>
        /// Create a new follower
        /// </summary>
        /// <param name="follower">An information about a new follower</param>
        /// <response code="200">Successful operation</response>
        /// <response code="400">Wrong input</response>
        [HttpPost("CreateFollower")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CreateFollower(RegionFollowerDto follower)
        {
            int id = await _regionService.CreateFollowerAsync(follower);

            return Ok(id);
        }

        /// <summary>
        /// Remove a specific follower from the region
        /// </summary>
        /// <param name="followerId">The id of the follower</param>
        [HttpDelete("RemoveFollower/{followerId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndOkrugaHeadAndOkrugaHeadDeputy)]
        public async Task<IActionResult> RemoveFollower(int followerId)
        {
            var follower = await _regionService.GetFollowerAsync(followerId);

            if (follower == null)
            {
                return NotFound();
            }
            var user = await  _userManager.GetUserAsync(User);
            var userAccess = await _userAccessService.GetUserRegionAccessAsync(follower.RegionId, user.Id, user);
            if (userAccess["EditRegion"])
            {
                await _regionService.RemoveFollowerAsync(followerId);
                return Ok();
            }

            return StatusCode(StatusCodes.Status403Forbidden);

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
                if (region == null || region.Status == BLL.DTO.RegionsStatusTypeDto.RegionBoard)
                {
                    return NotFound();
                }

                return Ok(region);
            }
            catch (Exception)
            {
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
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminPlastMemberAndSupporter)]
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
        /// Get all active regions using redis cache
        /// </summary>
        /// <returns>List of active regions</returns>
        [HttpGet("Profiles/Active/{page}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetActiveRegions(int page, int pageSize, string regionName = null)
        {
            string cacheKey = $"{ActiveRegionsCacheKey}_{page}_{pageSize}_{regionName}";
            Tuple<IEnumerable<RegionObjectsDto>, int> cache;

            try
            {
                cache = await _cache.GetRecordByKeyAsync<Tuple<IEnumerable<RegionObjectsDto>, int>>(cacheKey);
            }
            catch
            {
                cache = null;
            }

            if (cache is null)
            {
                bool isArchive = true;
                var query = new GetAllRegionsByPageAndIsArchiveQuery(page, pageSize, regionName, isArchive);
                cache = await _mediator.Send(query);
                if (!string.IsNullOrEmpty(regionName))
                {
                    TimeSpan expireTime = TimeSpan.FromMinutes(5);
                    await _cache.SetCacheRecordAsync(cacheKey, cache, expireTime);
                }
            }
            return StatusCode(StatusCodes.Status200OK, new
            {
                page,
                pageSize,
                regions = cache.Item1,
                total = cache.Item2,
                canCreate = User.IsInRole(Roles.Admin) || User.IsInRole(Roles.GoverningBodyAdmin)
            });
        }

        /// <summary>
        /// Get all not active regions using redis cache
        /// </summary>
        /// <returns>List of not active regions</returns>
        [HttpGet("Profiles/NotActive/{page}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetNotActiveRegions(int page, int pageSize, string regionName)
        {
            string cacheKey = $"{ArchivedRegionsCacheKey}_{page}_{pageSize}_{regionName}";
            Tuple<IEnumerable<RegionObjectsDto>, int> cache;

            try
            {
                cache = await _cache.GetRecordByKeyAsync<Tuple<IEnumerable<RegionObjectsDto>, int>>(cacheKey);
            }
            catch
            {
                cache = null;
            }

            if (cache is null)
            {
                bool isArchive = false;
                var query = new GetAllRegionsByPageAndIsArchiveQuery(page, pageSize, regionName, isArchive);
                cache = await _mediator.Send(query);
                if (!string.IsNullOrEmpty(regionName))
                {
                    TimeSpan expireTime = TimeSpan.FromMinutes(5);
                    await _cache.SetCacheRecordAsync(cacheKey, cache, expireTime);
                }
            }
            return StatusCode(StatusCodes.Status200OK, new { regions = cache.Item1, total = cache.Item2 });
        }

        /// <summary>
        /// Get all regions
        /// </summary>
        /// <returns>List of regions</returns>
        [HttpGet("Regions")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRegions(UkraineOblasts oblast = UkraineOblasts.NotSpecified)
        {
            var regions = await _regionService.GetRegions(oblast);
            return Ok(regions);
        }

        /// <summary>
        /// Get active regions names
        /// </summary>
        /// <returns>List of regions names</returns>
        [HttpGet("RegionsNames")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult GetActiveRegionsNames()
        {
            var regions = _regionService.GetActiveRegionsNames();
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
            return Ok(new { regions = await _regionAnnualReportService.GetAllRegionsIdAndName(await _userManager.GetUserAsync(User)) });
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
            try
            {
                return Ok(await _regionAnnualReportService.GetReportByIdAsync(await _userManager.GetUserAsync(User), id, year));
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

        [HttpGet("GetUserAdministrations/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetUserAdministrations(string userId)
        {
            var secretaries = await _regionAdministrationService.GetUserAdministrations(userId);
            return Ok(secretaries);
        }

        [HttpGet("GetUserPreviousAdministrations/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetUserPrevAdministrations(string userId)
        {
            var secretaries = await _regionAdministrationService.GetUserPreviousAdministrations(userId);
            return Ok(secretaries);
        }

        [HttpGet("Profiles")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Index()
        {
            var regions = await _regionService.GetAllRegionsAsync();
            return Ok(regions);
        }

        [HttpGet("Profiles/Active")]
        [AllowAnonymous]
        public async Task<IActionResult> ActiveRegions()
        {
            var regions = await _regionService.GetAllActiveRegionsAsync();
            return Ok(regions);
        }

        [HttpGet("Profiles/NotActive")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> NotActiveRegions()
        {
            var regions = await _regionService.GetAllNotActiveRegionsAsync();
            return Ok(regions);
        }

        [HttpPut("RedirectCities/{prevRegId}/{nextRegId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> RedirectCities(int prevRegId, int nextRegId)
        {
            await _regionService.RedirectMembers(prevRegId, nextRegId);

            return Ok();
        }

        [HttpPut("ArchiveRegion/{Id}")]
        [Authorize(Roles = Roles.AdminAndGBAdmin)]
        public async Task<IActionResult> ArchiveRegion(int Id)
        {
            await _regionService.ArchiveRegionAsync(Id);
            await _cache.RemoveRecordsByPatternAsync(ActiveRegionsCacheKey);
            await _cache.RemoveRecordsByPatternAsync(ArchivedRegionsCacheKey);
            return Ok();
        }

        [HttpDelete("RemoveAdministration/{administratorId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndOkrugaHeadAndOkrugaHeadDeputy)]
        public async Task<IActionResult> RemoveAdministration(int administratorId)
        {
            var regionAdministration = await _regionAdministrationService.GetRegionAdministrationByIdAsync(administratorId);
            if (regionAdministration == null)
            {
                return NotFound();
            }

            var userAccess = await _userAccessService.GetUserRegionAdministrationAccessAsync(regionAdministration,
                await _userManager.GetUserAsync(User));
            if (userAccess["RemoveRegionHead"])
            {
                await _regionAdministrationService.DeleteAdminByIdAsync(administratorId);
                return Ok();
            }

            return StatusCode(StatusCodes.Status403Forbidden);
        }

        [HttpDelete("RemoveRegion/{Id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndGBAdmin)]
        public async Task<IActionResult> RemoveRegion(int Id)
        {
            var admins = await _regionAdministrationService.GetAdministrationAsync(Id);
            foreach (var admin in admins)
            {
                await _regionAdministrationService.DeleteAdminByIdAsync(admin.ID);
            }
            await _regionService.DeleteRegionByIdAsync(Id);
            await _cache.RemoveRecordsByPatternAsync(ActiveRegionsCacheKey);
            await _cache.RemoveRecordsByPatternAsync(ArchivedRegionsCacheKey);
            return Ok();
        }

        [HttpDelete("RemoveDocument/{documentId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndOkrugaHeadAndOkrugaHeadDeputy)]
        public async Task<IActionResult> RemoveDocument(int documentId)
        {
            await _regionService.DeleteFileAsync(documentId);

            return Ok();
        }

        [HttpGet("RegionUsers/{regionId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetRegionUsers(int regionId)
        {
            var regionUsers = await _regionService.GetRegionUsersAsync(regionId);

            return Ok(regionUsers);
        }

        [HttpPut("UnArchiveRegion/{Id}")]
        [Authorize(Roles = Roles.AdminAndGBAdmin)]
        public async Task<IActionResult> UnArchiveRegion(int Id)
        {
            await _regionService.UnArchiveRegionAsync(Id);
            await _cache.RemoveRecordsByPatternAsync(ActiveRegionsCacheKey);
            await _cache.RemoveRecordsByPatternAsync(ArchivedRegionsCacheKey);
            return Ok();
        }

        /// <summary>
        /// Checks if theres already a Region with such name
        /// </summary>
        /// <param name="name">Name which checking</param>
        /// <returns>True if exist</returns>
        /// <returns>False if doesn't exist</returns>
        /// <response code="200">Check was successfull</response>
        [HttpGet("CheckIfRegionNameExists/{name}")]
        [Authorize(Roles = Roles.AdminAndGBAdmin)]
        public async Task<IActionResult> CheckIfRegionNameExists(string name)
        {
            bool result = await _regionService.CheckIfRegionNameExistsAsync(name);
            return Ok(result);
        }
    }
}