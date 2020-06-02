using AutoMapper;
using EPlast.BussinessLayer;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.DTO.City;
using EPlast.BussinessLayer.Exceptions;
using EPlast.BussinessLayer.Interfaces.City;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.Models;
using EPlast.Models.Enums;
using EPlast.ViewModels;
using EPlast.ViewModels.City;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Organization = EPlast.Models.Organization;

namespace EPlast.Controllers
{
    public class DocumentationController : Controller
    {
        private readonly IDecisionService _decisionService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IPdfService _PDFService;
        private readonly IAnnualReportService _annualReportService;
        private readonly ICityAccessService _cityAccessService;
        private readonly ICityMembersService _cityMembersService;
        private readonly ICityService _cityService;

        public DocumentationController(IPdfService PDFService, ILogger<DocumentationController> logger, IDecisionService decisionService, IMapper mapper, 
            IAnnualReportService annualReportService, ICityAccessService cityAccessService, ICityMembersService cityMembersService, ICityService cityService)
        {
            _PDFService = PDFService;
            _logger = logger;
            _decisionService = decisionService;
            _mapper = mapper;
            _annualReportService = annualReportService;
            _cityAccessService = cityAccessService;
            _cityMembersService = cityMembersService;
            _cityService = cityService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public DecisionViewModel CreateDecision()
        {
            DecisionViewModel decesionViewModel = null;
            try
            {
                var organizations = _mapper.Map<List<Organization>>(_decisionService.GetOrganizationList());
                decesionViewModel = new DecisionViewModel
                {
                    DecisionWrapper = _mapper.Map<DecisionWrapper>(_decisionService.CreateDecision()),
                    OrganizationListItems = from item in organizations
                                            select new SelectListItem
                                            {
                                                Text = item.OrganizationName,
                                                Value = item.ID.ToString()
                                            },
                    DecisionTargets = _mapper.Map<List<DecisionTarget>>(_decisionService.GetDecisionTargetList()),
                    DecisionStatusTypeListItems = _decisionService.GetDecisionStatusTypes()
                };
            }
            catch (Exception e)
            {
                RedirectToAction("HandleError", "Error");
                return null;
            }

            return decesionViewModel;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public JsonResult GetDecision(int id)
        {
            try
            {
                var decision = _mapper.Map<Decesion>(_decisionService.GetDecision(id));
                return Json(new { success = true, decision });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public JsonResult ChangeDecision(DecisionViewModel decesionViewModel)
        {
            var success = false;
            try
            {
                success = _decisionService.ChangeDecision(
                    _mapper.Map<DecisionDTO>(decesionViewModel.DecisionWrapper.Decision));
                return Json(new
                {
                    success,
                    text = "Зміни пройшли успішно!",
                    decesion = decesionViewModel.DecisionWrapper.Decision
                });
            }
            catch
            {
                return Json(new { success });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<JsonResult> SaveDecision(DecisionViewModel decesionViewModel)
        {
            var success = false;
            try
            {
                ModelState.Remove("Decesion.DecesionStatusType");
                if (!ModelState.IsValid && decesionViewModel.DecisionWrapper.Decision.DecisionTarget.ID != 0 ||
                    decesionViewModel == null)
                {
                    ModelState.AddModelError("", "Дані введені неправильно");
                    return Json(new
                    {
                        success,
                        text = ModelState.Values.SelectMany(v => v.Errors),
                        model = decesionViewModel,
                        modelstate = ModelState
                    });
                }

                if (decesionViewModel.DecisionWrapper.File != null &&
                    decesionViewModel.DecisionWrapper.File.Length > 10485760)
                {
                    ModelState.AddModelError("", "файл за великий (більше 10 Мб)");
                    return Json(new { success, text = "file length > 10485760" });
                }

                decesionViewModel.DecisionWrapper.Decision.HaveFile = decesionViewModel.DecisionWrapper.File != null;
                success = await _decisionService.SaveDecisionAsync(
                    _mapper.Map<DecisionWrapperDTO>(decesionViewModel.DecisionWrapper));
                return Json(new
                {
                    success,
                    Text = "Рішення додано!",
                    decision = decesionViewModel.DecisionWrapper.Decision,
                    decesionOrganization = _mapper.Map<Organization>(
                        _decisionService.GetDecisionOrganization(decesionViewModel.DecisionWrapper.Decision.ID))
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    success,
                    text = e.Message
                });
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ReadDecision()
        {
            List<DecisionViewModel> decisions = null;
            try
            {
                decisions = new List<DecisionViewModel>
                (
                    _mapper.Map<List<DecisionWrapper>>(_decisionService.GetDecisionList())
                        .Select(decesion => new DecisionViewModel { DecisionWrapper = decesion })
                        .ToList()
                );
            }
            catch
            {
                return RedirectToAction("HandleError", "Error");
            }

            return View(Tuple.Create(CreateDecision(), decisions));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public JsonResult DeleteDecision(int id)
        {
            return _decisionService.DeleteDecision(id) ? Json(new
            {
                success = true,
                text = "Зміни пройшли успішно!"
            }) :
               Json(new { success = false });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Download(int id, string filename)
        {
            byte[] fileBytes;
            try
            {
                if (id <= 0) throw new ArgumentException("Decision id cannot be null lest than zero");
                fileBytes = await _decisionService.DownloadDecisionFileAsync(id);
            }
            catch (Exception)
            {
                return RedirectToAction("HandleError", "Error");
            }
            return File(fileBytes, _decisionService.GetContentType(id, filename), filename);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> CreatePDFAsync(int objId)
        {
            try
            {
                if (objId <= 0) throw new ArgumentException("Cannot crated pdf id is not valid");

                var arr = await _PDFService.DecisionCreatePDFAsync(objId);
                return File(arr, "application/pdf");
            }
            catch
            {
                return RedirectToAction("HandleError", "Error");
            }
        }

        [Authorize(Roles = "Голова Станиці")]
        [HttpGet]
        public async Task<IActionResult> CreateAnnualReportAsync()
        {
            try
            {
                var citiesDTO = await _cityAccessService.GetCitiesAsync(User);
                var city = _mapper.Map<CityDTO, CityViewModel>(citiesDTO.First());
                if (_annualReportService.HasUnconfirmed(city.ID))
                {
                    throw new AnnualReportException("Станиця має непідтверджені звіти!");
                }
                if (_annualReportService.HasCreated(city.ID))
                {
                    throw new AnnualReportException("Річний звіт для даної станиці вже створений!");
                }
                var cityMemebrsDTO = _cityMembersService.GetCurrentByCityId(city.ID);
                var cityMembers = _mapper.Map<IEnumerable<CityMembersDTO>, IEnumerable<CityMembersViewModel>>(cityMemebrsDTO);
                var createEditAnnualReportViewModel = new CreateEditAnnualReportViewModel(cityMembers)
                {
                    Operation = AnnualReportOperation.Creating,
                    CityName = city.Name,
                    AnnualReport = new AnnualReportViewModel
                    {
                        CityId = city.ID,
                        MembersStatistic = new MembersStatisticViewModel()
                    }
                };
                return View("CreateEditAnnualReport", createEditAnnualReportViewModel);
            }
            catch (AnnualReportException e)
            {
                ViewData["ErrorMessage"] = e.Message;
                return View("CreateEditAnnualReport");
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status500InternalServerError });
            }
        }

        [Authorize(Roles = "Admin, Голова Округу")]
        [HttpGet]
        public async Task<IActionResult> CreateAnnualReportLikeAdminAsync(int cityId)
        {
            try
            {
                if (await _cityAccessService.HasAccessAsync(User, cityId))
                {
                    if (_annualReportService.HasUnconfirmed(cityId))
                    {
                        throw new AnnualReportException("Станиця має непідтверджені звіти!");
                    }
                    if (_annualReportService.HasCreated(cityId))
                    {
                        throw new AnnualReportException("Річний звіт для даної станиці вже створений!");
                    }
                    var cityDTO = _cityService.GetById(cityId);
                    var city = _mapper.Map<CityDTO, CityViewModel>(cityDTO);
                    var cityMemebrsDTO = _cityMembersService.GetCurrentByCityId(city.ID);
                    var cityMembers = _mapper.Map<IEnumerable<CityMembersDTO>, IEnumerable<CityMembersViewModel>>(cityMemebrsDTO);
                    var createEditAnnualReportViewModel = new CreateEditAnnualReportViewModel(cityMembers)
                    {
                        Operation = AnnualReportOperation.Creating,
                        CityName = city.Name,
                        AnnualReport = new AnnualReportViewModel
                        {
                            CityId = cityId,
                            MembersStatistic = new MembersStatisticViewModel()
                        }
                    };
                    return View("CreateEditAnnualReport", createEditAnnualReportViewModel);
                }
                else
                {
                    throw new Exception("Немає доступу до міста");
                }
            }
            catch (AnnualReportException e)
            {
                ViewData["ErrorMessage"] = e.Message;
                return View("CreateEditAnnualReport");
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status500InternalServerError });
            }
        }

        [Authorize(Roles = "Admin, Голова Округу, Голова Станиці")]
        [HttpPost]
        public async Task<IActionResult> CreateAnnualReportAsync(AnnualReportViewModel annualReport)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var annualReportDTO = _mapper.Map<AnnualReportViewModel, AnnualReportDTO>(annualReport);
                    await _annualReportService.CreateAsync(User, annualReportDTO);
                    ViewData["Message"] = "Річний звіт станиці успішно створено!";
                    return View("CreateEditAnnualReport");
                }
                else
                {
                    var cityDTO = _cityService.GetById(annualReport.CityId);
                    var city = _mapper.Map<CityDTO, CityViewModel>(cityDTO);
                    var cityMemebrsDTO = _cityMembersService.GetCurrentByCityId(city.ID);
                    var cityMembers = _mapper.Map<IEnumerable<CityMembersDTO>, IEnumerable<CityMembersViewModel>>(cityMemebrsDTO);
                    var createEditAnnualReportViewModel = new CreateEditAnnualReportViewModel(cityMembers)
                    {
                        Operation = AnnualReportOperation.Creating,
                        CityName = city.Name,
                        AnnualReport = annualReport
                    };
                    ViewData["ErrorMessage"] = "Річний звіт заповнений некоректно!";
                    return View("CreateEditAnnualReport", createEditAnnualReportViewModel);
                }
            }
            catch (AnnualReportException e)
            {
                ViewData["ErrorMessage"] = e.Message;
                return View("CreateEditAnnualReport");
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status500InternalServerError });
            }
        }

        [Authorize(Roles = "Admin, Голова Округу")]
        public async Task<IActionResult> ViewAnnualReportsAsync()
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
                _logger.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status500InternalServerError });
            }
        }

        [Authorize(Roles = "Admin, Голова Округу")]
        public async Task<IActionResult> GetAnnualReportAsync(int id)
        {
            try
            {
                var annualReportDTO = await _annualReportService.GetByIdAsync(User, id);
                var annualReport = _mapper.Map<AnnualReportDTO, AnnualReportViewModel>(annualReportDTO);
                return PartialView("_GetAnnualReport", annualReport);
            }
            catch (AnnualReportException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
                return NotFound("Не вдалося завантажити річний звіт");
            }
        }

        [Authorize(Roles = "Admin, Голова Округу")]
        public async Task<IActionResult> ConfirmAnnualReportAsync(int id)
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
                _logger.LogError($"Exception: {e.Message}");
                return NotFound("Не вдалося підтвердити річний звіт!");
            }
        }

        [Authorize(Roles = "Admin, Голова Округу")]
        public async Task<IActionResult> CancelAnnualReportAsync(int id)
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
                _logger.LogError($"Exception: {e.Message}");
                return NotFound("Не вдалося скасувати річний звіт!");
            }
        }

        [Authorize(Roles = "Admin, Голова Округу")]
        public async Task<IActionResult> DeleteAnnualReportAsync(int id)
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
                _logger.LogError($"Exception: {e.Message}");
                return NotFound("Не вдалося видалити річний звіт!");
            }
        }

        [Authorize(Roles = "Admin, Голова Округу")]
        [HttpGet]
        public async Task<IActionResult> EditAnnualReportAsync(int id)
        {
            try
            {
                var annualReportDTO = await _annualReportService.GetByIdAsync(User, id);
                var annualReport = _mapper.Map<AnnualReportDTO, AnnualReportViewModel>(annualReportDTO);
                var cityDTO = _cityService.GetById(annualReport.CityId);
                var city = _mapper.Map<CityDTO, CityViewModel>(cityDTO);
                var cityMemebrsDTO = _cityMembersService.GetCurrentByCityId(city.ID);
                var cityMembers = _mapper.Map<IEnumerable<CityMembersDTO>, IEnumerable<CityMembersViewModel>>(cityMemebrsDTO);
                var createEditAnnualReportViewModel = new CreateEditAnnualReportViewModel(cityMembers)
                {
                    Operation = AnnualReportOperation.Editing,
                    CityName = city.Name,
                    AnnualReport = annualReport
                };
                return View("CreateEditAnnualReport", createEditAnnualReportViewModel);
            }
            catch (AnnualReportException e)
            {
                ViewData["ErrorMessage"] = e.Message;
                return View("CreateEditAnnualReport");
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status500InternalServerError });
            }
        }

        [Authorize(Roles = "Admin, Голова Округу")]
        [HttpPost]
        public async Task<IActionResult> EditAnnualReportAsync(AnnualReportViewModel annualReport)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var annualReportDTO = _mapper.Map<AnnualReportViewModel, AnnualReportDTO>(annualReport);
                    await _annualReportService.EditAsync(User, annualReportDTO);
                    ViewData["Message"] = "Річний звіт станиці успішно відредаговано!";
                    return View("CreateEditAnnualReport");
                }
                else
                {
                    var cityDTO = _cityService.GetById(annualReport.CityId);
                    var city = _mapper.Map<CityDTO, CityViewModel>(cityDTO);
                    var cityMemebrsDTO = _cityMembersService.GetCurrentByCityId(city.ID);
                    var cityMembers = _mapper.Map<IEnumerable<CityMembersDTO>, IEnumerable<CityMembersViewModel>>(cityMemebrsDTO);
                    var createEditAnnualReportViewModel = new CreateEditAnnualReportViewModel(cityMembers)
                    {
                        Operation = AnnualReportOperation.Editing,
                        CityName = city.Name,
                        AnnualReport = annualReport
                    };
                    ViewData["ErrorMessage"] = "Річний звіт заповнений некоректно!";
                    return View("CreateEditAnnualReport", createEditAnnualReportViewModel);
                }
            }
            catch (AnnualReportException e)
            {
                ViewData["ErrorMessage"] = e.Message;
                return View("CreateEditAnnualReport");
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status500InternalServerError });
            }
        }
    }
}