﻿using EPlast.BLL.DTO.AnnualReport;
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
                _loggerService.LogError($"Annual report (id: {id}) not found");
                return StatusCode(StatusCodes.Status404NotFound, new { message = _localizer["NotFound"] });
            }
            catch (UnauthorizedAccessException)
            {
                _loggerService.LogError($"User (id: {await _userManagerService.GetUserIdAsync(User)}) hasn't access to annual report (id: {id})");
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
                    _loggerService.LogInformation($"User (id: {await _userManagerService.GetUserIdAsync(User)}) created annual report for city (id: {annualReport.CityId})");
                    return StatusCode(StatusCodes.Status201Created, new { message = _localizer["Created"] });
                }
                catch (InvalidOperationException)
                {
                    _loggerService.LogError($"City (id: {annualReport.CityId}) has created annual report");
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = _localizer["HasReport"] });
                }
                catch (UnauthorizedAccessException)
                {
                    _loggerService.LogError($"User (id: {await _userManagerService.GetUserIdAsync(User)}) hasn't access to city (id: {annualReport.CityId})");
                    return StatusCode(StatusCodes.Status403Forbidden, new { message = _localizer["NoAccess"] });
                }
                catch (NullReferenceException)
                {
                    _loggerService.LogError($"City (id: {annualReport.CityId}) not found");
                    return StatusCode(StatusCodes.Status404NotFound);
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
                    _loggerService.LogInformation($"User (id: {await _userManagerService.GetUserIdAsync(User)}) edited annual report (id: {annualReport.ID})");
                    return StatusCode(StatusCodes.Status200OK, new { message = _localizer["Edited"] });
                }
                catch (InvalidOperationException)
                {
                    _loggerService.LogError($"Annual report (id: {annualReport.ID}) can not be edited");
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = _localizer["FailedEdit"] });
                }
                catch (NullReferenceException)
                {
                    _loggerService.LogError($"Annual report (id: {annualReport.ID}) not found");
                    return StatusCode(StatusCodes.Status404NotFound, new { message = _localizer["NotFound"] });
                }
                catch (UnauthorizedAccessException)
                {
                    _loggerService.LogError($"User (id: {await _userManagerService.GetUserIdAsync(User)}) hasn't access to edit annual report (id: {annualReport.ID})");
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
                _loggerService.LogInformation($"User (id: {await _userManagerService.GetUserIdAsync(User)}) confirmed annual report (id: {id})");
                return StatusCode(StatusCodes.Status200OK, new { message = _localizer["Confirmed"] });
            }
            catch (NullReferenceException)
            {
                _loggerService.LogError($"Annual report (id: {id}) not found");
                return StatusCode(StatusCodes.Status404NotFound, new { message = _localizer["NotFound"] });
            }
            catch (UnauthorizedAccessException)
            {
                _loggerService.LogError($"User (id: {await _userManagerService.GetUserIdAsync(User)}) hasn't access to confirm annual report (id: {id})");
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
                _loggerService.LogInformation($"User (id: {await _userManagerService.GetUserIdAsync(User)}) canceled annual report (id: {id})");
                return StatusCode(StatusCodes.Status200OK, new { message = _localizer["Canceled"] });
            }
            catch (NullReferenceException)
            {
                _loggerService.LogError($"Annual report (id: {id}) not found");
                return StatusCode(StatusCodes.Status404NotFound, new { message = _localizer["NotFound"] });
            }
            catch (UnauthorizedAccessException)
            {
                _loggerService.LogError($"User (id: {await _userManagerService.GetUserIdAsync(User)}) hasn't access to cancel annual report (id: {id})");
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
                _loggerService.LogInformation($"User (id: {await _userManagerService.GetUserIdAsync(User)}) deleted annual report (id: {id})");
                return StatusCode(StatusCodes.Status200OK, new { message = _localizer["Deleted"] });
            }
            catch (NullReferenceException)
            {
                _loggerService.LogError($"Annual report (id: {id}) not found");
                return StatusCode(StatusCodes.Status404NotFound, new { message = _localizer["NotFound"] });
            }
            catch (UnauthorizedAccessException)
            {
                _loggerService.LogError($"User (id: {await _userManagerService.GetUserIdAsync(User)}) hasn't access to delete annual report (id: {id})");
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