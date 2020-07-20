using EPlast.BLL.DTO.AnnualReport;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Services.Interfaces;
using EPlast.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.ExtensionMethods;

namespace EPlast.WebApi.Controllers
{
    [ApiController, Route("api/[controller]")]
    [Authorize(Roles = "Admin, Голова Округу, Голова Станиці")]
    public class AnnualReportController : ControllerBase
    {
        private readonly IAnnualReportService _annualReportService;
        private readonly ILoggerService<AnnualReportController> _loggerService;
        private readonly IStringLocalizer<AnnualReportControllerMessage> _localizer;

        public AnnualReportController(IAnnualReportService annualReportService, ILoggerService<AnnualReportController> loggerService,
            IStringLocalizer<AnnualReportControllerMessage> localizer)
        {
            _annualReportService = annualReportService;
            _loggerService = loggerService;
            _localizer = localizer;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Голова Округу")]
        public async Task<IActionResult> Get()
        {
            return StatusCode(StatusCodes.Status200OK, new { annualReports = await _annualReportService.GetAllAsync(User) });
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin, Голова Округу")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, new { annualreport = await _annualReportService.GetByIdAsync(User, id) });
            }
            catch (NullReferenceException)
            {
                _loggerService.LogError(_localizer["NotFound"]);
                return StatusCode(StatusCodes.Status404NotFound, new { message = _localizer["NotFound"] });
            }
            catch (UnauthorizedAccessException)
            {
                _loggerService.LogError(_localizer["NoAccess"]);
                return StatusCode(StatusCodes.Status403Forbidden, new { message = _localizer["NoAccess"] });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(AnnualReportDTO annualReport)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _annualReportService.CreateAsync(User, annualReport);
                    return StatusCode(StatusCodes.Status201Created, new { message = _localizer["Created"] });
                }
                catch (InvalidOperationException)
                {
                    _loggerService.LogError(_localizer["HasReport"]);
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = _localizer["HasReport"] });
                }
                catch (UnauthorizedAccessException)
                {
                    _loggerService.LogError(_localizer["NoAccess"]);
                    return StatusCode(StatusCodes.Status403Forbidden, new { message = _localizer["NoAccess"] });
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin, Голова Округу")]
        public async Task<IActionResult> Edit(AnnualReportDTO annualReport)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _annualReportService.CreateAsync(User, annualReport);
                    return StatusCode(StatusCodes.Status200OK, new { message = _localizer["Edited"] });
                }
                catch (InvalidOperationException)
                {
                    _loggerService.LogError(_localizer["InvalidEdit"]);
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = _localizer["FailedEdit"] });
                }
                catch (NullReferenceException)
                {
                    _loggerService.LogError(_localizer["NotFound"]);
                    return StatusCode(StatusCodes.Status404NotFound, new { message = _localizer["NotFound"] });
                }
                catch (UnauthorizedAccessException)
                {
                    _loggerService.LogError(_localizer["NoAccess"]);
                    return StatusCode(StatusCodes.Status403Forbidden, new { message = _localizer["NoAccess"] });
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut("confirm/{id:int}")]
        [Authorize(Roles = "Admin, Голова Округу")]
        public async Task<IActionResult> Confirm(int id)
        {
            try
            {
                await _annualReportService.ConfirmAsync(User, id);
                return StatusCode(StatusCodes.Status200OK, new { message = _localizer["Confirmed"] });
            }
            catch (NullReferenceException)
            {
                _loggerService.LogError(_localizer["NotFound"]);
                return StatusCode(StatusCodes.Status404NotFound, new { message = _localizer["NotFound"] });
            }
            catch (UnauthorizedAccessException)
            {
                _loggerService.LogError(_localizer["NoAccess"]);
                return StatusCode(StatusCodes.Status403Forbidden, new { message = _localizer["NoAccess"] });
            }
        }

        [HttpPut("cancel/{id:int}")]
        [Authorize(Roles = "Admin, Голова Округу")]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                await _annualReportService.CancelAsync(User, id);
                return StatusCode(StatusCodes.Status200OK, new { message = _localizer["Canceled"] });
            }
            catch (NullReferenceException)
            {
                _loggerService.LogError(_localizer["NotFound"]);
                return StatusCode(StatusCodes.Status404NotFound, new { message = _localizer["NotFound"] });
            }
            catch (UnauthorizedAccessException)
            {
                _loggerService.LogError(_localizer["NoAccess"]);
                return StatusCode(StatusCodes.Status403Forbidden, new { message = _localizer["NoAccess"] });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin, Голова Округу")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _annualReportService.DeleteAsync(User, id);
                return StatusCode(StatusCodes.Status200OK, new { message = _localizer["Deleted"] });
            }
            catch (NullReferenceException)
            {
                _loggerService.LogError(_localizer["NotFound"]);
                return StatusCode(StatusCodes.Status404NotFound, new { message = _localizer["NotFound"] });
            }
            catch (UnauthorizedAccessException)
            {
                _loggerService.LogError(_localizer["NoAccess"]);
                return StatusCode(StatusCodes.Status403Forbidden, new { message = _localizer["NoAccess"] });
            }
        }

        [HttpGet("checkCreated/{cityId:int}")]
        public async Task<IActionResult> CheckCreated(int cityId)
        {
            try
            {
                if (await _annualReportService.CheckCreated(cityId))
                {
                    return StatusCode(StatusCodes.Status200OK, new { hasCreated = true, message = _localizer["HasReport"] });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new { hasCreated = false });
                }
            }
            catch (NullReferenceException)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
        }

        [HttpGet("getStatuses")]
        public IActionResult GetStatuses()
        {
            var statuses = new List<string>();
            foreach (Enum status in Enum.GetValues(typeof(AnnualReportStatusDTO)))
            {
                statuses.Add(status.GetDescription());
            }
            return StatusCode(StatusCodes.Status200OK, new { statuses });
        }
    }
}