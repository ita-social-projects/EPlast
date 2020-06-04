using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.DTO.City;
using EPlast.BussinessLayer.Exceptions;
using EPlast.BussinessLayer.Interfaces.City;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.Models.Enums;
using EPlast.ViewModels;
using EPlast.ViewModels.City;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        [Authorize(Roles = "Голова Станиці")]
        [HttpGet]
        public async Task<IActionResult> CreateAsync()
        {
            try
            {
                var citiesDTO = await _cityAccessService.GetCitiesAsync(User);
                var city = _mapper.Map<CityDTO, CityViewModel>(citiesDTO.First());
                await _annualReportService.CheckCreatedAndUnconfirmed(city.ID);
                return View("CreateEditAsync", await GetCreateEditViewModel(city, AnnualReportOperation.Creating));
            }
            catch (AnnualReportException e)
            {
                ViewData["ErrorMessage"] = e.Message;
                return View("CreateEditAsync");
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status500InternalServerError });
            }
        }

        [Authorize(Roles = "Admin, Голова Округу")]
        [HttpGet]
        public async Task<IActionResult> CreateAsync(int cityId)
        {
            try
            {
                if (await _cityAccessService.HasAccessAsync(User, cityId))
                {
                    await _annualReportService.CheckCreatedAndUnconfirmed(cityId);
                    var cityDTO = _cityService.GetById(cityId);
                    var city = _mapper.Map<CityDTO, CityViewModel>(cityDTO);
                    return View("CreateEditAsync", await GetCreateEditViewModel(city, AnnualReportOperation.Creating));
                }
                else
                {
                    _loggerService.LogError($"Exception: {User} does not have access to the city (cityId - {cityId})");
                    return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status500InternalServerError });
                }
            }
            catch (AnnualReportException e)
            {
                ViewData["ErrorMessage"] = e.Message;
                return View("CreateEditAsync");
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status500InternalServerError });
            }
        }

        [Authorize(Roles = "Admin, Голова Округу, Голова Станиці")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(AnnualReportViewModel annualReport)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var annualReportDTO = _mapper.Map<AnnualReportViewModel, AnnualReportDTO>(annualReport);
                    await _annualReportService.CreateAsync(User, annualReportDTO);
                    ViewData["Message"] = "Річний звіт станиці успішно створено!";
                    return View("CreateEditAsync");
                }
                else
                {
                    var cityDTO = _cityService.GetById(annualReport.CityId);
                    var city = _mapper.Map<CityDTO, CityViewModel>(cityDTO);
                    ViewData["ErrorMessage"] = "Річний звіт заповнений некоректно!";
                    return View("CreateEditAsync", await GetCreateEditViewModel(city, AnnualReportOperation.Creating, annualReport));
                }
            }
            catch (AnnualReportException e)
            {
                ViewData["ErrorMessage"] = e.Message;
                return View("CreateEditAsync");
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status500InternalServerError });
            }
        }

        [Authorize(Roles = "Admin, Голова Округу")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var citiesDTO = await _cityAccessService.GetCitiesAsync(User);
                var annualReportsDTO = await _annualReportService.GetAllAsync(User);
                var cities = _mapper.Map<IEnumerable<CityDTO>, IEnumerable<CityViewModel>>(citiesDTO);
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
        public async Task<IActionResult> GetAsync(int id)
        {
            try
            {
                var annualReportDTO = await _annualReportService.GetByIdAsync(User, id);
                var annualReport = _mapper.Map<AnnualReportDTO, AnnualReportViewModel>(annualReportDTO);
                return PartialView("_Get", annualReport);
            }
            catch (AnnualReportException e)
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
        public async Task<IActionResult> ConfirmAsync(int id)
        {
            try
            {
                await _annualReportService.ConfirmAsync(User, id);
                return Ok("Річний звіт станиці успішно підтверджено!");
            }
            catch (AnnualReportException e)
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
        public async Task<IActionResult> CancelAsync(int id)
        {
            try
            {
                await _annualReportService.CancelAsync(User, id);
                return Ok("Річний звіт станиці успішно скасовано!");
            }
            catch (AnnualReportException e)
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
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                await _annualReportService.DeleteAsync(User, id);
                return Ok("Річний звіт станиці успішно видалено!");
            }
            catch (AnnualReportException e)
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
        public async Task<IActionResult> EditAsync(int id)
        {
            try
            {
                var annualReportDTO = await _annualReportService.GetByIdAsync(User, id);
                var annualReport = _mapper.Map<AnnualReportDTO, AnnualReportViewModel>(annualReportDTO);
                var cityDTO = _cityService.GetById(annualReport.CityId);
                var city = _mapper.Map<CityDTO, CityViewModel>(cityDTO);
                return View("CreateEditAsync", await GetCreateEditViewModel(city, AnnualReportOperation.Editing, annualReport));
            }
            catch (AnnualReportException e)
            {
                ViewData["ErrorMessage"] = e.Message;
                return View("CreateEditAsync");
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status500InternalServerError });
            }
        }

        [Authorize(Roles = "Admin, Голова Округу")]
        [HttpPost]
        public async Task<IActionResult> EditAsync(AnnualReportViewModel annualReport)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var annualReportDTO = _mapper.Map<AnnualReportViewModel, AnnualReportDTO>(annualReport);
                    await _annualReportService.EditAsync(User, annualReportDTO);
                    ViewData["Message"] = "Річний звіт станиці успішно відредаговано!";
                    return View("CreateEditAsync");
                }
                else
                {
                    var cityDTO = _cityService.GetById(annualReport.CityId);
                    var city = _mapper.Map<CityDTO, CityViewModel>(cityDTO);
                    ViewData["ErrorMessage"] = "Річний звіт заповнений некоректно!";
                    return View("CreateEditAsync", await GetCreateEditViewModel(city, AnnualReportOperation.Editing, annualReport));
                }
            }
            catch (AnnualReportException e)
            {
                ViewData["ErrorMessage"] = e.Message;
                return View("CreateEditAsync");
            }
            catch (Exception e)
            {
                _loggerService.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status500InternalServerError });
            }
        }

        private async Task<CreateEditAnnualReportViewModel> GetCreateEditViewModel(CityViewModel city, AnnualReportOperation operation)
        {
            var cityMemebrsDTO = await _cityMembersService.GetCurrentByCityIdAsync(city.ID);
            var cityMembers = _mapper.Map<IEnumerable<CityMembersDTO>, IEnumerable<CityMembersViewModel>>(cityMemebrsDTO);
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

        private async Task<CreateEditAnnualReportViewModel> GetCreateEditViewModel(CityViewModel city, AnnualReportOperation operation, AnnualReportViewModel annualReport)
        {
            var createEditAnnualReportViewModel = await GetCreateEditViewModel(city, operation);
            createEditAnnualReportViewModel.AnnualReport = annualReport;
            return createEditAnnualReportViewModel;
        }
    }
}