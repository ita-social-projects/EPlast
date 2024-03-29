﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.DTO.AnnualReport;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.ExtensionMethods;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using EPlast.WebApi.Models.Club;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace EPlast.WebApi.Controllers
{
    [ApiController, Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.HeadsAndHeadDeputiesAndAdmin)]
    public class AnnualReportController : ControllerBase
    {
        private readonly IAnnualReportService _annualReportService;
        private readonly ILoggerService<AnnualReportController> _loggerService;
        private readonly UserManager<User> _userManager;
        private readonly IClubAnnualReportService _clubAnnualReportService;
        private readonly IMapper _mapper;

        public AnnualReportController(
            IAnnualReportService annualReportService, 
            ILoggerService<AnnualReportController> loggerService, 
            UserManager<User> userManager,
            IClubAnnualReportService clubAnnualReportService, 
            IMapper mapper)
        {
            _annualReportService = annualReportService;
            _loggerService = loggerService;
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
        [Authorize(Roles = Roles.AdminAndCityHeadAndCityHeadDeputy)]
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
        [Authorize(Roles = Roles.AdminCityHeadOkrugaHeadCityHeadDeputyOkrugaHeadDeputy)] 
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, new { annualReport = await _annualReportService.GetByIdAsync(await _userManager.GetUserAsync(User), id) });
            }
            catch (NullReferenceException)
            {
                _loggerService.LogError($"Annual report (id: {id}) not found");
                return StatusCode(StatusCodes.Status404NotFound, new { message = "Не вдалося знайти річний звіт!" });
            }
            catch (UnauthorizedAccessException)
            {
                _loggerService.LogError($"User (id: {(await _userManager.GetUserAsync(User)).Id}) hasn't access to annual report (id: {id})");
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Ви не маєте доступу до даного річного звіту!" });
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
        [Authorize(Roles = Roles.AdminCityHeadOkrugaHeadCityHeadDeputyOkrugaHeadDeputy)]
        public async Task<IActionResult> Get(string searchedData, int page, int pageSize, int sortKey, bool auth)
        {
            User user = await _userManager.GetUserAsync(User);
            try
            {
                IList<string> userRoles = await _userManager.GetRolesAsync(user);
                bool isAdminOrGBAdmin = userRoles.Contains(Roles.Admin) || userRoles.Contains(Roles.GoverningBodyAdmin);
                return StatusCode(StatusCodes.Status200OK, new
                {
                    annualReports =
                    await _annualReportService.GetAllAsync(
                        user,
                        isAdminOrGBAdmin,
                        searchedData,
                        page,
                        pageSize,
                        sortKey,
                        auth
                    )
                });
            }
            catch (NullReferenceException)
            {
                _loggerService.LogError($"Annual reports not found");
                return StatusCode(StatusCodes.Status404NotFound, new { message = "Не вдалося знайти річний звіт станиці!" });
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
        [Authorize(Roles = Roles.AdminRegionBoardHeadOkrugaCityHeadAndDeputy)] 
        public async Task<IActionResult> GetEditForm(int id)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, new { annualReport = await _annualReportService.GetEditFormByIdAsync(await _userManager.GetUserAsync(User), id) });
            }
            catch (NullReferenceException)
            {
                _loggerService.LogError($"Annual report (id: {id}) not found");
                return StatusCode(StatusCodes.Status404NotFound, new { message = "Не вдалося знайти річний звіт станиці!" });
            }
            catch (UnauthorizedAccessException)
            {
                _loggerService.LogError($"User (id: {(await _userManager.GetUserAsync(User)).Id}) hasn't access to annual report (id: {id})");
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Ви не маєте доступу до даного річного звіту!" });
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
        [Authorize(Roles = Roles.AdminCityHeadOkrugaHeadCityHeadDeputyOkrugaHeadDeputy)]
        public async Task<IActionResult> Create(AnnualReportDto annualReport)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _annualReportService.CreateAsync(await _userManager.GetUserAsync(User), annualReport);
                    _loggerService.LogInformation($"User (id: {(await _userManager.GetUserAsync(User)).Id}) created annual report for city (id: {annualReport.CityId})");
                    return StatusCode(StatusCodes.Status201Created, new { message = "Річний звіт станиці успішно створено!" });
                }
                catch (InvalidOperationException)
                {
                    _loggerService.LogError($"City (id: {annualReport.CityId}) has created annual report");
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = "Станиця вже має створений річний звіт!" });
                }
                catch (UnauthorizedAccessException)
                {
                    _loggerService.LogError($"User (id: {(await _userManager.GetUserAsync(User)).Id}) hasn't access to city (id: {annualReport.CityId})");
                    return StatusCode(StatusCodes.Status403Forbidden, new { message = "Ви не маєте доступу до даної станиці!" });
                }
                catch (NullReferenceException)
                {
                    _loggerService.LogError($"City (id: {annualReport.CityId}) not found");
                    return StatusCode(StatusCodes.Status404NotFound, new { message = "Не вдалося знайти інформацію про станицю!" });
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
        [Authorize(Roles = Roles.AdminRegionBoardHeadOkrugaCityHeadAndDeputy)]
        public async Task<IActionResult> Edit(AnnualReportDto annualReport)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _annualReportService.EditAsync(await _userManager.GetUserAsync(User), annualReport);
                    _loggerService.LogInformation($"User (id: {(await _userManager.GetUserAsync(User)).Id}) edited annual report (id: {annualReport.ID})");
                    return StatusCode(StatusCodes.Status200OK, new { message = "Річний звіт успішно відредаговано!" });
                }
                catch (InvalidOperationException)
                {
                    _loggerService.LogError($"Annual report (id: {annualReport.ID}) can not be edited");
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = "Не вдалося редагувати річний звіт!" });
                }
                catch (NullReferenceException)
                {
                    _loggerService.LogError($"Annual report (id: {annualReport.ID}) not found");
                    return StatusCode(StatusCodes.Status404NotFound, new { message = "Не вдалося знайти річний звіт!" });
                }
                catch (UnauthorizedAccessException)
                {
                    _loggerService.LogError($"User (id: {(await _userManager.GetUserAsync(User)).Id}) hasn't access to edit annual report (id: {annualReport.ID})");
                    return StatusCode(StatusCodes.Status403Forbidden, new { message = "Ви не маєте доступу до даного річного звіту!" });
                }
            }
            return BadRequest(ModelState);
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
        [Authorize(Roles = Roles.AdminAndGBAdmin)]
        public async Task<IActionResult> Confirm(int id)
        {
            try
            {
                await _annualReportService.ConfirmAsync(await _userManager.GetUserAsync(User), id);
                _loggerService.LogInformation($"User (id: {(await _userManager.GetUserAsync(User)).Id}) confirmed annual report (id: {id})");
                return StatusCode(StatusCodes.Status200OK, new { message = "Річний звіт успішно підтверджено!" });
            }
            catch (NullReferenceException)
            {
                _loggerService.LogError($"Annual report (id: {id}) not found");
                return StatusCode(StatusCodes.Status404NotFound, new { message = "Не вдалося знайти річний звіт!" });
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
        [Authorize(Roles = Roles.AdminAndGBAdmin)]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                await _annualReportService.CancelAsync(await _userManager.GetUserAsync(User), id);
                _loggerService.LogInformation($"User (id: {(await _userManager.GetUserAsync(User)).Id}) canceled annual report (id: {id})");
                return StatusCode(StatusCodes.Status200OK, new { message = "Річний звіт успішно скасовано!" });
            }
            catch (NullReferenceException)
            {
                _loggerService.LogError($"Annual report (id: {id}) not found");
                return StatusCode(StatusCodes.Status404NotFound, new { message = "Не вдалося знайти річний звіт!" });
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
        [Authorize(Roles = Roles.AdminAndGBAdmin)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _annualReportService.DeleteAsync(await _userManager.GetUserAsync(User), id);
                _loggerService.LogInformation($"User (id: {(await _userManager.GetUserAsync(User)).Id}) deleted annual report (id: {id})");
                return StatusCode(StatusCodes.Status200OK, new { message = "Річний звіт успішно видалено!" });
            }
            catch (NullReferenceException)
            {
                _loggerService.LogError($"Annual report (id: {id}) not found");
                return StatusCode(StatusCodes.Status404NotFound, new { message = "Не вдалося знайти річний звіт!" });
            }
        }

        private async Task<IActionResult> CheckBlocks(int id, bool city)
        {
            try
            {
                if (city
                    ? await _annualReportService.CheckCreated(await _userManager.GetUserAsync(User), id)
                    : await _clubAnnualReportService.CheckCreated(await _userManager.GetUserAsync(User), id))
                {
                    return StatusCode(StatusCodes.Status200OK,
                        new {hasCreated = true, message = city ? "Станиця вже має створений річний звіт!" : "Курінь вже має створений річний звіт!" });
                }
                return StatusCode(StatusCodes.Status200OK, new { hasCreated = false });
            }
            catch (NullReferenceException)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new { message = city ? "Не вдалося знайти інформацію про станицю!" : "Не вдалося знайти інформацію про курінь!" });
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new {message = city ? "Ви не маєте доступу до даної станиці!" : "Ви не маєте доступу до даного куреня!" });
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
            return await CheckBlocks(cityId, true);
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
            foreach (var enumValue in Enum.GetValues(typeof(AnnualReportStatusDto)).Cast<AnnualReportStatusDto>())
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
            return await CheckBlocks(clubId, false);
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
        [Authorize(Roles = Roles.AdminAndKurinHeadAndKurinHeadDeputy)]
        public async Task<IActionResult> GetAllClubAnnualReports(string searchedData, int page, int pageSize, int sortKey, bool auth)
        {
            User user = await _userManager.GetUserAsync(User);
            try
            {
                IList<string> userRoles = (await _userManager.GetRolesAsync(user));
                bool isAdminOrGBAdmin = userRoles.Contains(Roles.Admin) || userRoles.Contains(Roles.GoverningBodyAdmin);
                return base.StatusCode(StatusCodes.Status200OK, new
                {
                    clubAnnualReports = await _clubAnnualReportService.GetAllAsync(
                        user,
                        isAdminOrGBAdmin,
                        searchedData,
                        page,
                        pageSize,
                        sortKey,
                        auth
                    )
                });
            }
            catch (NullReferenceException)
            {
                _loggerService.LogError($"Annual reports not found");
                return StatusCode(StatusCodes.Status404NotFound, new { message = "Річні звіти куреня не знайдено!" });
            }
        }

        /// <summary>
        /// Method to get all club reports that the user has access to
        /// </summary>
        /// <returns>List of annual reports</returns>
        /// <response code="200">Successful operation</response>

        [HttpGet("~/api/Club/GetAllClubAnnualReports")]
        [Authorize(Roles = Roles.AdminAndKurinHeadAndKurinHeadDeputy)]
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
        [Authorize(Roles = Roles.AdminAndKurinHeadAndKurinHeadDeputy)]
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
        [Authorize(Roles = Roles.AdminAndKurinHeadAndKurinHeadDeputy)]
        public async Task<IActionResult> CreateClubAnnualReport([FromBody] ClubAnnualReportViewModel annualReport)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var clubAnnualReport = _mapper.Map<ClubAnnualReportViewModel, ClubAnnualReportDto>(annualReport);
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
        [Authorize(Roles = Roles.AdminAndGBAdmin)]
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
        [Authorize(Roles = Roles.AdminAndGBAdmin)]
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
        [Authorize(Roles = Roles.AdminAndGBAdmin)]
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
        [Authorize(Roles = Roles.AdminAndKurinHeadAndKurinHeadDeputy)]
        public async Task<IActionResult> EditClubAnnualReport(ClubAnnualReportDto clubAnnualReport)
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
            return BadRequest(ModelState);
        }

    }
}
