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

namespace EPlast.WebApi.Controllers
{
    [ApiController, Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AnnualReportController : ControllerBase
    {
        private readonly IAnnualReportService _annualReportService;
        private readonly IUserManagerService _userManagerService;
        private readonly ILoggerService<AnnualReportController> _loggerService;
        private readonly IStringLocalizer<AnnualReportControllerMessage> _localizer;

        public AnnualReportController(IAnnualReportService annualReportService, IUserManagerService userManagerService, ILoggerService<AnnualReportController> loggerService,
            IStringLocalizer<AnnualReportControllerMessage> localizer)
        {
            _annualReportService = annualReportService;
            _userManagerService = userManagerService;
            _loggerService = loggerService;
            _localizer = localizer;
        }

        /// <summary>
        /// Method to get all reports that the user has access to
        /// </summary>
        /// <returns>List of annual reports</returns>
        /// <response code="200">Successful operation</response>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return StatusCode(StatusCodes.Status200OK, new { annualReports = await _annualReportService.GetAllAsync(User) });
        }

        /// <summary>
        /// Method to get all the information in the annual report
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
                return StatusCode(StatusCodes.Status200OK, new { annualreport = await _annualReportService.GetByIdAsync(User, id) });
            }
            catch (NullReferenceException)
            {
                _loggerService.LogError($"Annual report (id: {id}) not found");
                return StatusCode(StatusCodes.Status404NotFound, new { message = _localizer["NotFound"].Value });
            }
            catch (UnauthorizedAccessException)
            {
                _loggerService.LogError($"User (id: {await _userManagerService.GetUserIdAsync(User)}) hasn't access to annual report (id: {id})");
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
                    await _annualReportService.CreateAsync(User, annualReport);
                    _loggerService.LogInformation($"User (id: {await _userManagerService.GetUserIdAsync(User)}) created annual report for city (id: {annualReport.CityId})");
                    return StatusCode(StatusCodes.Status201Created, new { message = _localizer["Created"].Value });
                }
                catch (InvalidOperationException)
                {
                    _loggerService.LogError($"City (id: {annualReport.CityId}) has created annual report");
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = _localizer["HasReport"].Value });
                }
                catch (UnauthorizedAccessException)
                {
                    _loggerService.LogError($"User (id: {await _userManagerService.GetUserIdAsync(User)}) hasn't access to city (id: {annualReport.CityId})");
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
                    await _annualReportService.CreateAsync(User, annualReport);
                    _loggerService.LogInformation($"User (id: {await _userManagerService.GetUserIdAsync(User)}) edited annual report (id: {annualReport.ID})");
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
                    _loggerService.LogError($"User (id: {await _userManagerService.GetUserIdAsync(User)}) hasn't access to edit annual report (id: {annualReport.ID})");
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
        public async Task<IActionResult> Confirm(int id)
        {
            try
            {
                await _annualReportService.ConfirmAsync(User, id);
                _loggerService.LogInformation($"User (id: {await _userManagerService.GetUserIdAsync(User)}) confirmed annual report (id: {id})");
                return StatusCode(StatusCodes.Status200OK, new { message = _localizer["Confirmed"].Value });
            }
            catch (NullReferenceException)
            {
                _loggerService.LogError($"Annual report (id: {id}) not found");
                return StatusCode(StatusCodes.Status404NotFound, new { message = _localizer["NotFound"].Value });
            }
            catch (UnauthorizedAccessException)
            {
                _loggerService.LogError($"User (id: {await _userManagerService.GetUserIdAsync(User)}) hasn't access to confirm annual report (id: {id})");
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
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                await _annualReportService.CancelAsync(User, id);
                _loggerService.LogInformation($"User (id: {await _userManagerService.GetUserIdAsync(User)}) canceled annual report (id: {id})");
                return StatusCode(StatusCodes.Status200OK, new { message = _localizer["Canceled"].Value });
            }
            catch (NullReferenceException)
            {
                _loggerService.LogError($"Annual report (id: {id}) not found");
                return StatusCode(StatusCodes.Status404NotFound, new { message = _localizer["NotFound"].Value });
            }
            catch (UnauthorizedAccessException)
            {
                _loggerService.LogError($"User (id: {await _userManagerService.GetUserIdAsync(User)}) hasn't access to cancel annual report (id: {id})");
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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _annualReportService.DeleteAsync(User, id);
                _loggerService.LogInformation($"User (id: {await _userManagerService.GetUserIdAsync(User)}) deleted annual report (id: {id})");
                return StatusCode(StatusCodes.Status200OK, new { message = _localizer["Deleted"].Value });
            }
            catch (NullReferenceException)
            {
                _loggerService.LogError($"Annual report (id: {id}) not found");
                return StatusCode(StatusCodes.Status404NotFound, new { message = _localizer["NotFound"].Value });
            }
            catch (UnauthorizedAccessException)
            {
                _loggerService.LogError($"User (id: {await _userManagerService.GetUserIdAsync(User)}) hasn't access to delete annual report (id: {id})");
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
                if (await _annualReportService.CheckCreated(User, cityId))
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
    }
}