using EPlast.BLL.DTO.AnnualReport;
using EPlast.BLL.ExtensionMethods;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Services.Interfaces;
using EPlast.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces.Club;
using EPlast.DataAccess.Entities;
using EPlast.WebApi.Models.Club;
using Microsoft.AspNetCore.Identity;

namespace EPlast.WebApi.Controllers
{
    [ApiController, Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.HeadsAndAdmin)]
    public class AnnualReportController : ControllerBase
    {
        private readonly IAnnualReportService _annualReportService;
        private readonly ILoggerService<AnnualReportController> _loggerService;
        private readonly IStringLocalizer<AnnualReportControllerMessage> _localizer;
        private readonly UserManager<User> _userManager;
        private readonly IClubAnnualReportService _clubAnnualReportService;
        private readonly IMapper _mapper;

        public AnnualReportController(IAnnualReportService annualReportService, 
            ILoggerService<AnnualReportController> loggerService,
            IStringLocalizer<AnnualReportControllerMessage> localizer, 
            UserManager<User> userManager, 
            IClubAnnualReportService clubAnnualReportService, 
            IMapper mapper)
        {
            _annualReportService = annualReportService;
            _loggerService = loggerService;
            _localizer = localizer;
            _userManager = userManager;
            _clubAnnualReportService = clubAnnualReportService;
            _mapper = mapper;
        }

        /// <summary>
        /// Method to get all reports that the user has access to
        /// </summary>
        /// <returns>List of annual reports</returns>
        /// <response code="200">Successful operation</response>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return StatusCode(StatusCodes.Status200OK, new { annualReports = await _annualReportService.GetAllAsync(await _userManager.GetUserAsync(User)) });
        }

        /// <summary>
        /// Method to get annual report
        /// </summary>
        /// <param name="id">Annual report identification number</param>
        /// <returns>Annual report</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User hasn't access to annual report</response>
        /// <response code="404">The annual report does not exist</response>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, new { annualReport = await _annualReportService.GetByIdAsync(await _userManager.GetUserAsync(User), id) });
            }
            catch (NullReferenceException)
            {
                _loggerService.LogError($"Annual report (id: {id}) not found");
                return StatusCode(StatusCodes.Status404NotFound, new { message = _localizer["NotFound"].Value });
            }
            catch (UnauthorizedAccessException)
            {
                _loggerService.LogError($"User (id: {(await _userManager.GetUserAsync(User)).Id}) hasn't access to annual report (id: {id})");
                return StatusCode(StatusCodes.Status403Forbidden, new { message = _localizer["NoAccess"].Value });
            }
        }

        /// <summary>
        /// Method to get all searched reports that the user has access to
        /// </summary>
        /// <param name="searchedData">Searched Data</param>
        /// <param name="page">current page on pagination</param>
        /// <param name="pageSize">number of records per page</param>
        /// <param name="sortKey">Key for sorting</param>
        /// <param name="auth">Whether to select reports of that user is author</param>
        /// <returns>List of AnnualReportTableObject</returns>
        /// <response code="200">Successful operation</response>
        [HttpGet("Cities")]
        public async Task<IActionResult> Get(string searchedData, int page, int pageSize, int sortKey, bool auth)
        {
            var user = await _userManager.GetUserAsync(User);
            try
            {
                return StatusCode(StatusCodes.Status200OK, new { annualReports = 
                    await _annualReportService.GetAllAsync(user, 
                        (await _userManager.GetRolesAsync(user)).Contains(Roles.Admin), 
                        searchedData, page, pageSize, sortKey, auth) });
            }
            catch (NullReferenceException)
            {
                _loggerService.LogError($"Annual reports not found");
                return StatusCode(StatusCodes.Status404NotFound, new { message = _localizer["NotFound"].Value });
            }
            catch (UnauthorizedAccessException)
            {
                _loggerService.LogError($"User (id: {(await _userManager.GetUserAsync(User)).Id}) hasn't access to annual reports");
                return StatusCode(StatusCodes.Status403Forbidden, new { message = _localizer["NoAccess"].Value });
            }
        }

        /// <summary>
        /// Get all members of a specific city
        /// </summary>
        /// <param name="cityId">The id of the city</param>
        /// <returns>CityDTO</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">City not found</response>
        [HttpGet("Members/{cityId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetMembers(int cityId)
        {
            var cityDto = await _annualReportService.GetCityMembersAsync(cityId);
            if (cityDto == null)
            {
                return NotFound();
            }

            return Ok(new { cityDto.CityMembers, cityDto.Name });
        }

        /// <summary>
        /// Method to get data to annual report edit form
        /// </summary>
        /// <param name="id">Annual report identification number</param>
        /// <returns>Annual report</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User hasn't access to annual report</response>
        /// <response code="404">The annual report does not exist</response>
        [HttpGet("EditCityAnnualReportForm/{id:int}")]
        public async Task<IActionResult> GetEditForm(int id)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, new { annualReport = await _annualReportService.GetEditFormByIdAsync(await _userManager.GetUserAsync(User), id) });
            }
            catch (NullReferenceException)
            {
                _loggerService.LogError($"Annual report (id: {id}) not found");
                return StatusCode(StatusCodes.Status404NotFound, new { message = _localizer["NotFound"].Value });
            }
            catch (UnauthorizedAccessException)
            {
                _loggerService.LogError($"User (id: {(await _userManager.GetUserAsync(User)).Id}) hasn't access to annual report (id: {id})");
                return StatusCode(StatusCodes.Status403Forbidden, new { message = _localizer["NoAccess"].Value });
            }
        }

        /// <summary>
        /// Method to create new annual report for city
        /// </summary>
        /// <param name="annualReport">Annual report model</param>
        /// <returns>Answer from backend</returns>
        /// <response code="201">Annual report was successfully created</response>
        /// <response code="400">City has created annual report</response>
        /// <response code="403">User hasn't access to city</response>
        /// <response code="404">The city does not exist</response>
        /// <response code="404">Annual report model is not valid</response>
        [HttpPost]
        public async Task<IActionResult> Create(AnnualReportDTO annualReport)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _annualReportService.CreateAsync(await _userManager.GetUserAsync(User), annualReport);
                    _loggerService.LogInformation($"User (id: {(await _userManager.GetUserAsync(User)).Id}) created annual report for city (id: {annualReport.CityId})");
                    return StatusCode(StatusCodes.Status201Created, new { message = _localizer["Created"].Value });
                }
                catch (InvalidOperationException)
                {
                    _loggerService.LogError($"City (id: {annualReport.CityId}) has created annual report");
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = _localizer["HasReport"].Value });
                }
                catch (UnauthorizedAccessException)
                {
                    _loggerService.LogError($"User (id: {(await _userManager.GetUserAsync(User)).Id}) hasn't access to city (id: {annualReport.CityId})");
                    return StatusCode(StatusCodes.Status403Forbidden, new { message = _localizer["CityNoAccess"].Value });
                }
                catch (NullReferenceException)
                {
                    _loggerService.LogError($"City (id: {annualReport.CityId}) not found");
                    return StatusCode(StatusCodes.Status404NotFound, new { message = _localizer["CityNotFound"].Value });
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Method to edit annual report
        /// </summary>
        /// <param name="annualReport"></param>
        /// <returns>Answer from backend</returns>
        /// <response code="200">Annual report was successfully edited</response>
        /// <response code="400">Annual report can not be edited</response>
        /// <response code="403">User hasn't access to annual report</response>
        /// <response code="404">The annual report does not exist</response>
        /// <response code="404">Annual report model is not valid</response>
        [HttpPut]
        public async Task<IActionResult> Edit(AnnualReportDTO annualReport)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _annualReportService.EditAsync(await _userManager.GetUserAsync(User), annualReport);
                    _loggerService.LogInformation($"User (id: {(await _userManager.GetUserAsync(User)).Id}) edited annual report (id: {annualReport.ID})");
                    return StatusCode(StatusCodes.Status200OK, new { message = _localizer["Edited"].Value });
                }
                catch (InvalidOperationException)
                {
                    _loggerService.LogError($"Annual report (id: {annualReport.ID}) can not be edited");
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = _localizer["FailedEdit"].Value });
                }
                catch (NullReferenceException)
                {
                    _loggerService.LogError($"Annual report (id: {annualReport.ID}) not found");
                    return StatusCode(StatusCodes.Status404NotFound, new { message = _localizer["NotFound"].Value });
                }
                catch (UnauthorizedAccessException)
                {
                    _loggerService.LogError($"User (id: {(await _userManager.GetUserAsync(User)).Id}) hasn't access to edit annual report (id: {annualReport.ID})");
                    return StatusCode(StatusCodes.Status403Forbidden, new { message = _localizer["NoAccess"].Value });
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Method to confirm annual report
        /// </summary>
        /// <param name="id">Annual report identification number</param>
        /// <returns>Answer from backend</returns>
        /// <response code="200">Annual report was successfully confirmed</response>
        /// <response code="403">User hasn't access to annual report</response>
        /// <response code="404">The annual report does not exist</response>
        [HttpPut("confirm/{id:int}")]
        [Authorize(Roles = Roles.AdminAndOkrugaHead)]
        public async Task<IActionResult> Confirm(int id)
        {
            try
            {
                await _annualReportService.ConfirmAsync(await _userManager.GetUserAsync(User), id);
                _loggerService.LogInformation($"User (id: {(await _userManager.GetUserAsync(User)).Id}) confirmed annual report (id: {id})");
                return StatusCode(StatusCodes.Status200OK, new { message = _localizer["Confirmed"].Value });
            }
            catch (NullReferenceException)
            {
                _loggerService.LogError($"Annual report (id: {id}) not found");
                return StatusCode(StatusCodes.Status404NotFound, new { message = _localizer["NotFound"].Value });
            }
            catch (UnauthorizedAccessException)
            {
                _loggerService.LogError($"User (id: {(await _userManager.GetUserAsync(User)).Id}) hasn't access to confirm annual report (id: {id})");
                return StatusCode(StatusCodes.Status403Forbidden, new { message = _localizer["NoAccess"].Value });
            }
        }

        /// <summary>
        /// Method to cancel annual report
        /// </summary>
        /// <param name="id">Annual report identification number</param>
        /// <returns>Answer from backend</returns>
        /// <response code="200">Annual report was successfully canceled</response>
        /// <response code="403">User hasn't access to annual report</response>
        /// <response code="404">The annual report does not exist</response>
        [HttpPut("cancel/{id:int}")]
        [Authorize(Roles = Roles.AdminAndOkrugaHead)]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                await _annualReportService.CancelAsync(await _userManager.GetUserAsync(User), id);
                _loggerService.LogInformation($"User (id: {(await _userManager.GetUserAsync(User)).Id}) canceled annual report (id: {id})");
                return StatusCode(StatusCodes.Status200OK, new { message = _localizer["Canceled"].Value });
            }
            catch (NullReferenceException)
            {
                _loggerService.LogError($"Annual report (id: {id}) not found");
                return StatusCode(StatusCodes.Status404NotFound, new { message = _localizer["NotFound"].Value });
            }
            catch (UnauthorizedAccessException)
            {
                _loggerService.LogError($"User (id: {(await _userManager.GetUserAsync(User)).Id}) hasn't access to cancel annual report (id: {id})");
                return StatusCode(StatusCodes.Status403Forbidden, new { message = _localizer["NoAccess"].Value });
            }
        }

        /// <summary>
        /// Method to delete annual report
        /// </summary>
        /// <param name="id">Annual report identification number</param>
        /// <returns>Answer from backend</returns>
        /// <response code="200">Annual report was successfully deleted</response>
        /// <response code="403">User hasn't access to annual report</response>
        /// <response code="404">The annual report does not exist</response>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = Roles.AdminAndOkrugaHead)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _annualReportService.DeleteAsync(await _userManager.GetUserAsync(User), id);
                _loggerService.LogInformation($"User (id: {(await _userManager.GetUserAsync(User)).Id}) deleted annual report (id: {id})");
                return StatusCode(StatusCodes.Status200OK, new { message = _localizer["Deleted"].Value });
            }
            catch (NullReferenceException)
            {
                _loggerService.LogError($"Annual report (id: {id}) not found");
                return StatusCode(StatusCodes.Status404NotFound, new { message = _localizer["NotFound"].Value });
            }
            catch (UnauthorizedAccessException)
            {
                _loggerService.LogError($"User (id: {(await _userManager.GetUserAsync(User)).Id}) hasn't access to delete annual report (id: {id})");
                return StatusCode(StatusCodes.Status403Forbidden, new { message = _localizer["NoAccess"].Value });
            }
        }

        /// <summary>
        /// Method to get information whether city has created annual report
        /// </summary>
        /// <param name="cityId">City identification number</param>
        /// <returns>Answer from backend</returns>
        /// <response code="200">Successful operation</response>
        /// /// <response code="403">User hasn't access to city</response>
        /// <response code="404">The city not exist</response>
        [HttpGet("checkCreated/{cityId:int}")]
        public async Task<IActionResult> CheckCreated(int cityId)
        {
            try
            {
                if (await _annualReportService.CheckCreated(await _userManager.GetUserAsync(User), cityId))
                {
                    return StatusCode(StatusCodes.Status200OK, new { hasCreated = true, message = _localizer["HasReport"].Value });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new { hasCreated = false });
                }
            }
            catch (NullReferenceException)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { message = _localizer["CityNotFound"].Value });
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = _localizer["CityNoAccess"].Value });
            }
        }

        /// <summary>
        /// Method to get annual report statuses
        /// </summary>
        /// <returns>List of enum values</returns>
        /// <response code="200">Successful operation</response>
        [HttpGet("getStatuses")]
        public IActionResult GetStatuses()
        {
            var statuses = new List<string>();
            foreach (var enumValue in Enum.GetValues(typeof(AnnualReportStatusDTO)).Cast<AnnualReportStatusDTO>())
            {
                statuses.Add(enumValue.GetDescription());
            }
            return StatusCode(StatusCodes.Status200OK, new { statuses });
        }

        /// <summary>
        /// Method to get information whether club has created annual report
        /// </summary>
        /// <param name="clubId">Club identification number</param>
        /// <returns>Answer from backend</returns>
        /// <response code="200">Successful operation</response>
        /// /// <response code="403">User hasn't access to club</response>
        /// <response code="404">The club not exist</response>
        [HttpGet("checkCreatedClubReport/{clubId:int}")]
        public async Task<IActionResult> CheckCreatedClubAnnualReport(int clubId)
        {
            try
            {
                if (await _clubAnnualReportService.CheckCreated(await _userManager.GetUserAsync(User), clubId))
                {
                    return StatusCode(StatusCodes.Status200OK, new { hasCreated = true, message = _localizer["ClubHasReport"].Value });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new { hasCreated = false });
                }
            }
            catch (NullReferenceException)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { message = _localizer["ClubNotFound"].Value });
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = _localizer["ClubNoAccess"].Value });
            }
        }

        /// <summary>
        /// Method to get all club reports that the user has access to
        /// </summary>
        /// <param name="searchedData">Searched Data</param>
        /// <param name="page">current page on pagination</param>
        /// <param name="pageSize">number of records per page</param>
        /// <param name="sortKey">Key for sorting</param>
        /// <param name="auth">Whether to select reports of that user is author</param>
        /// <returns>List of ClubAnnualReportTableObject</returns>
        /// <response code="200">Successful operation</response>

        [HttpGet("~/api/Club/ClubAnnualReports")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.HeadsAndAdmin)]
        public async Task<IActionResult> GetAllClubAnnualReports(string searchedData, int page, int pageSize, int sortKey, bool auth)
        {
            var user = await _userManager.GetUserAsync(User);
            try
            {
                return StatusCode(StatusCodes.Status200OK, new
                {
                    clubAnnualReports = await _clubAnnualReportService.GetAllAsync(user,
                        (await _userManager.GetRolesAsync(user)).Contains(Roles.Admin), searchedData, page, pageSize, sortKey, auth)
                });
            }
            catch (NullReferenceException)
            {
                _loggerService.LogError($"Annual reports not found");
                return StatusCode(StatusCodes.Status404NotFound, new { message = _localizer["NotFound"].Value });
            }
            catch (UnauthorizedAccessException)
            {
                _loggerService.LogError($"User (id: {(await _userManager.GetUserAsync(User)).Id}) hasn't access to annual reports");
                return StatusCode(StatusCodes.Status403Forbidden, new { message = _localizer["NoAccess"].Value });
            }
        }

        /// <summary>
        /// Method to get all club reports that the user has access to
        /// </summary>
        /// <returns>List of annual reports</returns>
        /// <response code="200">Successful operation</response>

        [HttpGet("~/api/Club/GetAllClubAnnualReports")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.HeadsAndAdmin)]
        public async Task<IActionResult> GetAllClubAnnualReports()
        {
            return StatusCode(StatusCodes.Status200OK, new { clubAnnualReports = await _clubAnnualReportService.GetAllAsync(await _userManager.GetUserAsync(User)) });
        }

        /// <summary>
        /// Method to get club annual report
        /// </summary>
        /// <param name="id">Club annual report identification number</param>
        /// <returns>Annual report</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User hasn't access to annual report</response>
        /// <response code="404">The club annual report does not exist</response>

        [HttpGet("~/api/Club/GetClubAnnualReportById/{id:int}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.HeadsAndAdmin)]
        public async Task<IActionResult> GetClubAnnualReportById(int id)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, new { annualreport = await _clubAnnualReportService.GetByIdAsync(await _userManager.GetUserAsync(User), id) });
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

        [HttpPost("~/api/Club/CreateClubAnnualReport")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.HeadsAndAdmin)]
        public async Task<IActionResult> CreateClubAnnualReport([FromBody] ClubAnnualReportViewModel annualReport)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var clubAnnualReport = _mapper.Map<ClubAnnualReportViewModel, ClubAnnualReportDTO>(annualReport);
                    await _clubAnnualReportService.CreateAsync(await _userManager.GetUserAsync(User), clubAnnualReport);
                }
                catch (InvalidOperationException)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                catch (NullReferenceException)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

            }
            else
            {
                return BadRequest(ModelState);
            }

            return StatusCode(StatusCodes.Status201Created);
        }


        /// <summary>
        /// Method to confirm annual report
        /// </summary>
        /// <param name="id">Annual report identification number</param>
        /// <returns>Answer from backend</returns>
        /// <response code="200">Annual report was successfully confirmed</response>
        /// <response code="403">User hasn't access to annual report</response>
        /// <response code="404">The annual report does not exist</response>
        [HttpPut("~/api/Club/confirmClubAnnualReport/{id:int}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.HeadsAndAdmin)]
        public async Task<IActionResult> ConfirmClubAnnualReport(int id)
        {
            try
            {
                await _clubAnnualReportService.ConfirmAsync(await _userManager.GetUserAsync(User), id);
                return Ok();
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
        /// Method to cancel club annual report
        /// </summary>
        /// <param name="id">Annual report identification number</param>
        /// <returns>Answer from backend</returns>
        /// <response code="200">Annual report was successfully canceled</response>
        /// <response code="403">User hasn't access to annual report</response>
        /// <response code="404">The annual report does not exist</response>
        [HttpPut("~/api/Club/cancelClubAnnualReport/{id:int}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.HeadsAndAdmin)]
        public async Task<IActionResult> CancelClubAnnualReport(int id)
        {
            try
            {
                await _clubAnnualReportService.CancelAsync(await _userManager.GetUserAsync(User), id);
                return Ok();
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
        /// Method to delete annual report
        /// </summary>
        /// <param name="id">Club annual report identification number</param>
        /// <returns>Answer from backend</returns>
        /// <response code="200">Annual report was successfully deleted</response>
        /// <response code="403">User hasn't access to annual report</response>
        /// <response code="404">The annual report does not exist</response>
        [HttpDelete("~/api/Club/deleteClubAnnualReport/{id:int}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.HeadsAndAdmin)]
        public async Task<IActionResult> DeleteClubAnnualReport(int id)
        {
            try
            {
                await _clubAnnualReportService.DeleteClubReportAsync(await _userManager.GetUserAsync(User), id);
                return Ok();
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
        /// Method to edit club annual report
        /// </summary>
        /// <param name="clubAnnualReport"></param>
        /// <returns>Answer from backend</returns>
        /// <response code="200">Club annual report was successfully edited</response>
        /// <response code="403">User hasn't access to club annual report</response>
        /// <response code="404">The club annual report does not exist</response>
        /// <response code="404">Annual report model is not valid</response>
        [HttpPut("~/api/Club/editClubAnnualReport")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.HeadsAndAdmin)]
        public async Task<IActionResult> EditClubAnnualReport(ClubAnnualReportDTO clubAnnualReport)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _clubAnnualReportService.EditClubReportAsync(await _userManager.GetUserAsync(User), clubAnnualReport);
                    return Ok();
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
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}
