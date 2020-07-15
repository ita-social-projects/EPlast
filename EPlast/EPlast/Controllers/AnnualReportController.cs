using AutoMapper;
using EPlast.BLL.DTO.AnnualReport;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Services.Interfaces;
using EPlast.Models.Enums;
using EPlast.ViewModels.AnnualReport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BLL.Interfaces.Logging;
using CityDTOs = EPlast.BLL.DTO.City;
using CityVMs = EPlast.ViewModels.City;

namespace EPlast.Controllers
{
    public class AnnualReportController : Controller
    {
        private readonly ILoggerService<AnnualReportController> _loggerService;
        private readonly IMapper _mapper;
        private readonly IAnnualReportService _annualReportService;
        private readonly ICityAccessService _cityAccessService;
        private readonly ICityMembersService _cityMembersService;
        private readonly ICityService _cityService;

        public AnnualReportController(ILoggerService<AnnualReportController> loggerService, IMapper mapper, IAnnualReportService annualReportService, ICityAccessService cityAccessService,
            ICityMembersService cityMembersService, ICityService cityService)
        {
            _loggerService = loggerService;
            _mapper = mapper;
            _annualReportService = annualReportService;
            _cityAccessService = cityAccessService;
            _cityMembersService = cityMembersService;
            _cityService = cityService;
        }

        [Authorize(Roles = "Admin, Голова Округу")]
        [HttpGet]
        public async Task<IActionResult> CreateAsAdmin(int cityId)
        {
            try
            {
                if (await _cityAccessService.HasAccessAsync(User, cityId))
                {
                    await _annualReportService.CheckCanBeCreatedAsync(cityId);
                    var cityDTO = await _cityService.GetByIdAsync(cityId);
                    var city = _mapper.Map<CityDTOs.CityDTO, CityVMs.CityViewModel>(cityDTO);
                    return View("CreateEdit", await GetCreateEditViewModel(city, AnnualReportOperation.Creating));
                }
                else
                {
                    _loggerService.LogError($"Exception: {User} does not have access to the city (cityId - {cityId})");
                    return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status500InternalServerError });
                }
            }
            catch (InvalidOperationException e)
            {
                ViewData["ErrorMessage"] = e.Message;
                return View("CreateEdit");
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status500InternalServerError });
            }
        }

        [Authorize(Roles = "Голова Станиці")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var citiesDTO = await _cityAccessService.GetCitiesAsync(User);
                var city = _mapper.Map<CityDTOs.CityDTO, CityVMs.CityViewModel>(citiesDTO.First());
                await _annualReportService.CheckCanBeCreatedAsync(city.ID);
                return View("CreateEdit", await GetCreateEditViewModel(city, AnnualReportOperation.Creating));
            }
            catch (InvalidOperationException e)
            {
                ViewData["ErrorMessage"] = e.Message;
                return View("CreateEdit");
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status500InternalServerError });
            }
        }

        [Authorize(Roles = "Admin, Голова Округу, Голова Станиці")]
        [HttpPost]
        public async Task<IActionResult> Create(AnnualReportViewModel annualReport)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var annualReportDTO = _mapper.Map<AnnualReportViewModel, AnnualReportDTO>(annualReport);
                    await _annualReportService.CreateAsync(User, annualReportDTO);
                    ViewData["Message"] = "Річний звіт станиці успішно створено!";
                    return View("CreateEdit");
                }
                else
                {
                    var cityDTO = await _cityService.GetByIdAsync(annualReport.CityId);
                    var city = _mapper.Map<CityDTOs.CityDTO, CityVMs.CityViewModel>(cityDTO);
                    ViewData["ErrorMessage"] = "Річний звіт заповнений некоректно!";
                    return View("CreateEdit", await GetCreateEditViewModel(city, AnnualReportOperation.Creating, annualReport));
                }
            }
            catch (InvalidOperationException e)
            {
                ViewData["ErrorMessage"] = e.Message;
                return View("CreateEdit");
            }
            catch (UnauthorizedAccessException e)
            {
                ViewData["ErrorMessage"] = e.Message;
                return View("CreateEdit");
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status500InternalServerError });
            }
        }

        [Authorize(Roles = "Admin, Голова Округу")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var citiesDTO = await _cityAccessService.GetCitiesAsync(User);
                var annualReportsDTO = await _annualReportService.GetAllAsync(User);
                var cities = _mapper.Map<IEnumerable<CityDTOs.CityDTO>, IEnumerable<CityVMs.CityViewModel>>(citiesDTO);
                var annualReports = _mapper.Map<IEnumerable<AnnualReportDTO>, IEnumerable<AnnualReportViewModel>>(annualReportsDTO);
                var viewAnnualReportsViewModel = new ViewAnnualReportsViewModel(cities)
                {
                    AnnualReports = annualReports
                };
                return View(viewAnnualReportsViewModel);
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status500InternalServerError });
            }
        }

        [Authorize(Roles = "Admin, Голова Округу")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var annualReportDTO = await _annualReportService.GetByIdAsync(User, id);
                var annualReport = _mapper.Map<AnnualReportDTO, AnnualReportViewModel>(annualReportDTO);
                return PartialView("_Get", annualReport);
            }
            catch (UnauthorizedAccessException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return NotFound("Не вдалося завантажити річний звіт!");
            }
        }

        [Authorize(Roles = "Admin, Голова Округу")]
        public async Task<IActionResult> Confirm(int id)
        {
            try
            {
                await _annualReportService.ConfirmAsync(User, id);
                return Ok("Річний звіт станиці успішно підтверджено!");
            }
            catch (UnauthorizedAccessException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return NotFound("Не вдалося підтвердити річний звіт!");
            }
        }

        [Authorize(Roles = "Admin, Голова Округу")]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                await _annualReportService.CancelAsync(User, id);
                return Ok("Річний звіт станиці успішно скасовано!");
            }
            catch (UnauthorizedAccessException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return NotFound("Не вдалося скасувати річний звіт!");
            }
        }

        [Authorize(Roles = "Admin, Голова Округу")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _annualReportService.DeleteAsync(User, id);
                return Ok("Річний звіт станиці успішно видалено!");
            }
            catch (UnauthorizedAccessException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return NotFound("Не вдалося видалити річний звіт!");
            }
        }

        [Authorize(Roles = "Admin, Голова Округу")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var annualReportDTO = await _annualReportService.GetByIdAsync(User, id);
                var annualReport = _mapper.Map<AnnualReportDTO, AnnualReportViewModel>(annualReportDTO);
                var cityDTO = await _cityService.GetByIdAsync(annualReport.CityId);
                var city = _mapper.Map<CityDTOs.CityDTO, CityVMs.CityViewModel>(cityDTO);
                return View("CreateEdit", await GetCreateEditViewModel(city, AnnualReportOperation.Editing, annualReport));
            }
            catch (UnauthorizedAccessException e)
            {
                ViewData["ErrorMessage"] = e.Message;
                return View("CreateEdit");
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status500InternalServerError });
            }
        }

        [Authorize(Roles = "Admin, Голова Округу")]
        [HttpPost]
        public async Task<IActionResult> Edit(AnnualReportViewModel annualReport)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var annualReportDTO = _mapper.Map<AnnualReportViewModel, AnnualReportDTO>(annualReport);
                    await _annualReportService.EditAsync(User, annualReportDTO);
                    ViewData["Message"] = "Річний звіт станиці успішно відредаговано!";
                    return View("CreateEdit");
                }
                else
                {
                    var cityDTO = await _cityService.GetByIdAsync(annualReport.CityId);
                    var city = _mapper.Map<CityDTOs.CityDTO, CityVMs.CityViewModel>(cityDTO);
                    ViewData["ErrorMessage"] = "Річний звіт заповнений некоректно!";
                    return View("CreateEdit", await GetCreateEditViewModel(city, AnnualReportOperation.Editing, annualReport));
                }
            }
            catch (InvalidOperationException e)
            {
                ViewData["ErrorMessage"] = e.Message;
                return View("CreateEdit");
            }
            catch (UnauthorizedAccessException e)
            {
                ViewData["ErrorMessage"] = e.Message;
                return View("CreateEdit");
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status500InternalServerError });
            }
        }

        private async Task<CreateEditAnnualReportViewModel> GetCreateEditViewModel(CityVMs.CityViewModel city, AnnualReportOperation operation)
        {
            var cityMemebrsDTO = await _cityMembersService.GetMembersByCityIdAsync(city.ID);
            var cityMembers = _mapper.Map<IEnumerable<CityDTOs.CityMembersDTO>, IEnumerable<CityVMs.CityMembersViewModel>>(cityMemebrsDTO);
            return new CreateEditAnnualReportViewModel(cityMembers)
            {
                Operation = operation,
                CityName = city.Name,
                AnnualReport = new AnnualReportViewModel
                {
                    CityId = city.ID,
                    MembersStatistic = new MembersStatisticViewModel()
                }
            };
        }

        private async Task<CreateEditAnnualReportViewModel> GetCreateEditViewModel(CityVMs.CityViewModel city, AnnualReportOperation operation, AnnualReportViewModel annualReport)
        {
            var createEditAnnualReportViewModel = await GetCreateEditViewModel(city, operation);
            createEditAnnualReportViewModel.AnnualReport = annualReport;
            return createEditAnnualReportViewModel;
        }
    }
}