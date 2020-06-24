using EPlast.BussinessLayer.DTO.AnnualReport;
using EPlast.BussinessLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using EPlast.BussinessLayer.Interfaces.Logging;

namespace EPlast.WebApi.Controllers
{
    [ApiController, Route("api/[controller]")]
    [Authorize(Roles = "Admin, Голова Округу")]
    public class AnnualReportController : ControllerBase
    {
        private readonly IAnnualReportService _annualReportService;
        private readonly ILoggerService<AnnualReportController> _loggerService;

        public AnnualReportController(IAnnualReportService annualReportService, ILoggerService<AnnualReportController> loggerService)
        {
            _annualReportService = annualReportService;
            _loggerService = loggerService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _annualReportService.GetAllAsync(User));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return Ok(await _annualReportService.GetByIdAsync(User, id));
            }
            catch (NullReferenceException e)
            {
                _loggerService.LogError(e.Message);
                return NotFound(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                _loggerService.LogError(e.Message);
                return Forbid(e.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Голова Округу, Голова Станиці")]
        public async Task<IActionResult> Create(AnnualReportDTO annualReport)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _annualReportService.CreateAsync(User, annualReport);
                    return Ok();
                }
                catch (InvalidOperationException e)
                {
                    _loggerService.LogError(e.Message);
                    return BadRequest(e.Message);
                }
                catch (UnauthorizedAccessException e)
                {
                    _loggerService.LogError(e.Message);
                    return Forbid(e.Message);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Edit(AnnualReportDTO annualReport)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _annualReportService.CreateAsync(User, annualReport);
                    return Ok();
                }
                catch (InvalidOperationException e)
                {
                    _loggerService.LogError(e.Message);
                    return BadRequest(e.Message);
                }
                catch (NullReferenceException e)
                {
                    _loggerService.LogError(e.Message);
                    return NotFound(e.Message);
                }
                catch (UnauthorizedAccessException e)
                {
                    _loggerService.LogError(e.Message);
                    return Forbid(e.Message);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut("confirm/{id:int}")]
        public async Task<IActionResult> Confirm(int id)
        {
            try
            {
                await _annualReportService.ConfirmAsync(User, id);
                return Ok();
            }
            catch (NullReferenceException e)
            {
                _loggerService.LogError(e.Message);
                return NotFound(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                _loggerService.LogError(e.Message);
                return Forbid(e.Message);
            }
        }

        [HttpPut("cancel/{id:int}")]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                await _annualReportService.CancelAsync(User, id);
                return Ok();
            }
            catch (NullReferenceException e)
            {
                _loggerService.LogError(e.Message);
                return NotFound(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                _loggerService.LogError(e.Message);
                return Forbid(e.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _annualReportService.DeleteAsync(User, id);
                return Ok();
            }
            catch (NullReferenceException e)
            {
                _loggerService.LogError(e.Message);
                return NotFound(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                _loggerService.LogError(e.Message);
                return Forbid(e.Message);
            }
        }
    }
}