using AutoMapper;
using EPlast.BussinessLayer;
using EPlast.BussinessLayer.AccessManagers.Interfaces;
using EPlast.BussinessLayer.DTO;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Models;
using EPlast.Models.ViewModelInitializations.Interfaces;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        private readonly IAnnualReportVMInitializer _annualReportVMCreator;
        private readonly ICityAccessManager _cityAccessManager;
        private readonly IDecisionService _decisionService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IPDFService _PDFService;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly UserManager<User> _userManager;
        private readonly IViewAnnualReportsVMInitializer _viewAnnualReportsVMInitializer;

        public DocumentationController(IRepositoryWrapper repoWrapper, UserManager<User> userManager,
            IAnnualReportVMInitializer annualReportVMCreator,
            IPDFService PDFService, IViewAnnualReportsVMInitializer viewAnnualReportsVMInitializer,
            ICityAccessManager cityAccessManager, ILogger<DocumentationController> logger,
            IDecisionService decisionService, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _annualReportVMCreator = annualReportVMCreator;
            _userManager = userManager;
            _PDFService = PDFService;
            _viewAnnualReportsVMInitializer = viewAnnualReportsVMInitializer;
            _cityAccessManager = cityAccessManager;
            _logger = logger;
            _decisionService = decisionService;
            _mapper = mapper;
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
                    return Json(new { success, text = "file lenght > 10485760" });
                }

                decesionViewModel.DecisionWrapper.Decision.HaveFile = decesionViewModel.DecisionWrapper.File != null;
                success = await _decisionService.SaveDecision(
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
            if (_decisionService.DeleteDecision(id))
            {
                return Json(new
                {
                    success = true,
                    text = "Зміни пройшли успішно!"
                });

            }
            return Json(new {success = false});
           
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Download(int id, string filename)
        {
            byte[] fileBytes;
            try
            {
                if (id <= 0) throw new ArgumentException("Decision id cannot be null lest than zero");
                fileBytes = await _decisionService.DownloadDecisionFile(id);
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
        public IActionResult CreateAnnualReport()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var city = _cityAccessManager.GetCities(userId).First();
                var annualReportCheck = _repoWrapper.AnnualReports
                    .FindByCondition(ar => ar.CityId == city.ID && ar.Date.Year == DateTime.Now.Year)
                    .FirstOrDefault();
                if (annualReportCheck == null)
                {
                    var cityMembers = _repoWrapper.User
                        .FindByCondition(u => u.CityMembers.Any(cm => cm.City.ID == city.ID && cm.EndDate == null))
                        .Include(u => u.UserPlastDegrees);
                    var annualReportViewModel = new AnnualReportViewModel
                    {
                        Operation = AnnualReportOperation.Creating,
                        CityName = city.Name,
                        CityMembers = _annualReportVMCreator.GetCityMembers(cityMembers),
                        CityLegalStatusTypes = _annualReportVMCreator.GetCityLegalStatusTypes(),
                        AnnualReport = _annualReportVMCreator.GetAnnualReport(userId, city.ID, cityMembers)
                    };
                    return View("CreateEditAnnualReport", annualReportViewModel);
                }

                ViewData["ErrorMessage"] = $"Звіт станиці {city.Name} за {DateTime.Now.Year} рік вже існує!";
                return View("CreateEditAnnualReport");
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [Authorize(Roles = "Admin, Голова Округу")]
        [HttpGet]
        public IActionResult CreateAnnualReportLikeAdmin(int cityId)
        {
            var userId = _userManager.GetUserId(User);
            if (!_cityAccessManager.HasAccess(userId, cityId))
                return RedirectToAction("HandleError", "Error", new { code = 403 });
            try
            {
                var city = _repoWrapper.City
                    .FindByCondition(c => c.ID == cityId)
                    .First();
                var annualReportCheck = _repoWrapper.AnnualReports
                    .FindByCondition(ar => ar.CityId == city.ID && ar.Date.Year == DateTime.Now.Year)
                    .FirstOrDefault();
                if (annualReportCheck == null)
                {
                    var cityMembers = _repoWrapper.User
                        .FindByCondition(u => u.CityMembers.Any(cm => cm.City.ID == cityId && cm.EndDate == null))
                        .Include(u => u.UserPlastDegrees);
                    var annualReportViewModel = new AnnualReportViewModel
                    {
                        Operation = AnnualReportOperation.Creating,
                        CityName = city.Name,
                        CityMembers = _annualReportVMCreator.GetCityMembers(cityMembers),
                        CityLegalStatusTypes = _annualReportVMCreator.GetCityLegalStatusTypes(),
                        AnnualReport = _annualReportVMCreator.GetAnnualReport(userId, city.ID, cityMembers)
                    };
                    return View("CreateEditAnnualReport", annualReportViewModel);
                }

                ViewData["ErrorMessage"] = $"Звіт станиці {city.Name} за {DateTime.Now.Year} рік вже існує!";
                return View("CreateEditAnnualReport");
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [Authorize(Roles = "Admin, Голова Округу, Голова Станиці")]
        [HttpPost]
        public IActionResult CreateAnnualReport(AnnualReport annualReport)
        {
            if (!_cityAccessManager.HasAccess(annualReport.UserId, annualReport.CityId))
                return RedirectToAction("HandleError", "Error", new { code = 403 });
            try
            {
                var city = _repoWrapper.City
                    .FindByCondition(c => c.ID == annualReport.CityId)
                    .First();
                if (ModelState.IsValid)
                {
                    var annualReportCheck = _repoWrapper.AnnualReports
                        .FindByCondition(ar =>
                            ar.CityId == annualReport.CityId && ar.Date.Year == annualReport.Date.Year)
                        .FirstOrDefault();
                    if (annualReportCheck == null)
                    {
                        _repoWrapper.AnnualReports.Create(annualReport);
                        _repoWrapper.Save();
                        ViewData["Message"] = $"Звіт станиці {city.Name} за {annualReport.Date.Year} рік створено!";
                    }
                    else
                    {
                        ViewData["ErrorMessage"] =
                            $"Звіт станиці {city.Name} за {annualReport.Date.Year} рік вже існує!";
                    }

                    return View("CreateEditAnnualReport");
                }

                var cityMembers = _repoWrapper.User
                    .FindByCondition(u =>
                        u.CityMembers.Any(cm => cm.City.ID == annualReport.CityId && cm.EndDate == null))
                    .Include(u => u.UserPlastDegrees);
                var annualReportViewModel = new AnnualReportViewModel
                {
                    Operation = AnnualReportOperation.Creating,
                    CityName = city.Name,
                    CityMembers = _annualReportVMCreator.GetCityMembers(cityMembers),
                    CityLegalStatusTypes = _annualReportVMCreator.GetCityLegalStatusTypes(),
                    AnnualReport = annualReport
                };
                ViewData["ErrorMessage"] = "Звіт заповнений некоректно!";
                return View("CreateEditAnnualReport", annualReportViewModel);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [Authorize(Roles = "Admin, Голова Округу")]
        public IActionResult ViewAnnualReports()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                var cities = _cityAccessManager.GetCities(userId);
                var annualReports = _repoWrapper.AnnualReports
                    .FindAll()
                    .Include(ar => ar.City)
                    .ThenInclude(c => c.Region)
                    .Include(ar => ar.User)
                    .ToList();
                annualReports.RemoveAll(ar => !cities.Any(c => c.ID == ar.CityId));
                var viewAnnualReportsViewModel = new ViewAnnualReportsViewModel
                {
                    AnnualReports = annualReports,
                    Cities = _viewAnnualReportsVMInitializer.GetCities(cities)
                };
                return View(viewAnnualReportsViewModel);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [Authorize(Roles = "Admin, Голова Округу")]
        public IActionResult GetAnnualReport(int id)
        {
            try
            {
                var annualReport = _repoWrapper.AnnualReports
                    .FindByCondition(ar => ar.ID == id)
                    .Include(ar => ar.City)
                    .Include(ar => ar.MembersStatistic)
                    .Include(ar => ar.CityManagement)
                    .ThenInclude(cm => cm.CityAdminNew)
                    .First();
                var userId = _userManager.GetUserId(User);
                if (!_cityAccessManager.HasAccess(userId, annualReport.CityId))
                    return RedirectToAction("HandleError", "Error", new { code = 403 });
                return PartialView("_GetAnnualReport", annualReport);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
                return NotFound("Не вдалося завантажити річний звіт!");
            }
        }

        [Authorize(Roles = "Admin, Голова Округу")]
        public async Task<IActionResult> ConfirmAnnualReport(int id)
        {
            try
            {
                var annualReport = _repoWrapper.AnnualReports
                    .FindByCondition(ar => ar.ID == id && ar.Status == AnnualReportStatus.Unconfirmed)
                    .Include(ar => ar.City)
                    .Include(ar => ar.CityManagement)
                    .ThenInclude(cm => cm.CityAdminNew)
                    .First();
                var userId = _userManager.GetUserId(User);
                if (!_cityAccessManager.HasAccess(userId, annualReport.CityId))
                    return RedirectToAction("HandleError", "Error", new { code = 403 });
                var annualReportOld = _repoWrapper.AnnualReports
                    .FindByCondition(
                        ar => ar.CityId == annualReport.CityId && ar.Status == AnnualReportStatus.Confirmed)
                    .Include(ar => ar.City)
                    .Include(ar => ar.CityManagement)
                    .ThenInclude(cm => cm.CityAdminNew)
                    .FirstOrDefault();

                // update annualReport status
                if (annualReportOld != null)
                {
                    annualReportOld.Status = AnnualReportStatus.Saved;
                    _repoWrapper.AnnualReports.Update(annualReportOld);
                }

                annualReport.Status = AnnualReportStatus.Confirmed;
                _repoWrapper.AnnualReports.Update(annualReport);

                // update oldCityAdmin EndDate
                var adminType = _repoWrapper.AdminType
                    .FindByCondition(at => at.AdminTypeName == "Голова Станиці")
                    .First();
                var cityAdminOld = _repoWrapper.CityAdministration
                    .FindByCondition(ca => ca.CityId == annualReport.CityId && ca.AdminTypeId == adminType.ID)
                    .Include(ca => ca.User)
                    .LastOrDefault();
                annualReport.CityManagement.CityAdminOldId = cityAdminOld?.ID;
                if (cityAdminOld != null && annualReport.CityManagement.CityAdminNew != null
                                         && annualReport.CityManagement.UserId != cityAdminOld.UserId &&
                                         cityAdminOld.EndDate == null)
                {
                    cityAdminOld.EndDate = DateTime.Now;
                    _repoWrapper.CityAdministration.Update(cityAdminOld);
                    await _userManager.RemoveFromRoleAsync(cityAdminOld.User, adminType.AdminTypeName);
                }

                // create newCityAdmin
                if ((cityAdminOld == null || cityAdminOld?.EndDate != null) &&
                    annualReport.CityManagement.CityAdminNew != null)
                {
                    var cityAdminNew = new CityAdministration
                    {
                        UserId = annualReport.CityManagement.UserId,
                        CityId = annualReport.CityId,
                        AdminTypeId = adminType.ID,
                        StartDate = DateTime.Now
                    };
                    _repoWrapper.CityAdministration.Create(cityAdminNew);
                    await _userManager.AddToRoleAsync(annualReport.CityManagement.CityAdminNew,
                        adminType.AdminTypeName);
                }

                // update oldCityLegalStatus EndDate
                var cityLegalStatusOld = _repoWrapper.CityLegalStatuses
                    .FindByCondition(cls => cls.CityId == annualReport.CityId)
                    .LastOrDefault();
                annualReport.CityManagement.CityLegalStatusOldId = cityLegalStatusOld?.Id;
                if (cityLegalStatusOld != null && annualReport.CityManagement.CityLegalStatusNew !=
                                               cityLegalStatusOld?.LegalStatusType
                                               && cityLegalStatusOld?.DateFinish == null)
                {
                    cityLegalStatusOld.DateFinish = DateTime.Now;
                    _repoWrapper.CityLegalStatuses.Update(cityLegalStatusOld);
                }

                // create newCityLegalStatus
                if (cityLegalStatusOld == null || cityLegalStatusOld.DateFinish != null)
                {
                    var cityLegalStatusNew = new CityLegalStatus
                    {
                        CityId = annualReport.CityId,
                        LegalStatusType = annualReport.CityManagement.CityLegalStatusNew,
                        DateStart = DateTime.Now
                    };
                    _repoWrapper.CityLegalStatuses.Create(cityLegalStatusNew);
                }

                _repoWrapper.Save();
                return Ok($"Звіт станиці {annualReport.City.Name} за {annualReport.Date.Year} рік підтверджено!");
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
                return NotFound("Не вдалося підтвердити річний звіт!");
            }
        }

        [Authorize(Roles = "Admin, Голова Округу")]
        public async Task<IActionResult> CancelAnnualReport(int id)
        {
            try
            {
                var annualReport = _repoWrapper.AnnualReports
                    .FindByCondition(ar => ar.ID == id && ar.Status == AnnualReportStatus.Confirmed)
                    .Include(ar => ar.City)
                    .Include(ar => ar.CityManagement)
                    .ThenInclude(cm => cm.CityLegalStatusOld)
                    .First();
                var userId = _userManager.GetUserId(User);
                if (!_cityAccessManager.HasAccess(userId, annualReport.CityId))
                    return RedirectToAction("HandleError", "Error", new { code = 403 });

                // cityAdmin revert
                var adminType = _repoWrapper.AdminType
                    .FindByCondition(at => at.AdminTypeName == "Голова Станиці")
                    .First();
                var cityAdminOld = _repoWrapper.CityAdministration
                    .FindByCondition(ca => ca.ID == annualReport.CityManagement.CityAdminOldId)
                    .Include(ca => ca.User)
                    .FirstOrDefault();
                if (cityAdminOld != null)
                {
                    cityAdminOld.EndDate = null;
                    _repoWrapper.CityAdministration.Update(cityAdminOld);
                    await _userManager.AddToRoleAsync(cityAdminOld.User, adminType.AdminTypeName);
                }

                var cityAdministrations = _repoWrapper.CityAdministration
                    .FindByCondition(ca =>
                        ca.ID > annualReport.CityManagement.CityAdminOldId && ca.CityId == annualReport.CityId &&
                        ca.AdminTypeId == adminType.ID)
                    .Include(ca => ca.User);
                foreach (var cityAdministration in cityAdministrations)
                {
                    _repoWrapper.CityAdministration.Delete(cityAdministration);
                    await _userManager.RemoveFromRoleAsync(cityAdministration.User, adminType.AdminTypeName);
                }

                // cityLegalStatus revert
                if (annualReport.CityManagement.CityLegalStatusOld != null)
                    annualReport.CityManagement.CityLegalStatusOld.DateFinish = null;
                var cityLegalStatuses = _repoWrapper.CityLegalStatuses
                    .FindByCondition(cls => cls.Id > annualReport.CityManagement.CityLegalStatusOldId);
                foreach (var cityLegalStatus in cityLegalStatuses)
                    _repoWrapper.CityLegalStatuses.Delete(cityLegalStatus);

                // save changes
                annualReport.Status = AnnualReportStatus.Unconfirmed;
                annualReport.CityManagement.CityAdminOld = null;
                annualReport.CityManagement.CityLegalStatusOld = null;
                _repoWrapper.AnnualReports.Update(annualReport);
                _repoWrapper.Save();

                return Ok($"Звіт станиці {annualReport.City.Name} за {annualReport.Date.Year} рік скасовано!");
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
                return NotFound("Не вдалося скасувати річний звіт!");
            }
        }

        [Authorize(Roles = "Admin, Голова Округу")]
        public IActionResult DeleteAnnualReport(int id)
        {
            try
            {
                var annualReport = _repoWrapper.AnnualReports
                    .FindByCondition(ar => ar.ID == id && ar.Status == AnnualReportStatus.Unconfirmed)
                    .Include(ar => ar.City)
                    .First();
                var userId = _userManager.GetUserId(User);
                if (!_cityAccessManager.HasAccess(userId, annualReport.CityId))
                    return RedirectToAction("HandleError", "Error", new { code = 403 });
                _repoWrapper.AnnualReports.Delete(annualReport);
                _repoWrapper.Save();
                return Ok($"Звіт станиці {annualReport.City.Name} за {annualReport.Date.Year} рік видалено!");
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
                return NotFound("Не вдалося видалити річний звіт!");
            }
        }

        [Authorize(Roles = "Admin, Голова Округу")]
        [HttpGet]
        public IActionResult EditAnnualReport(int id)
        {
            try
            {
                var annualReport = _repoWrapper.AnnualReports
                    .FindByCondition(ar => ar.ID == id && ar.Status == AnnualReportStatus.Unconfirmed)
                    .Include(ar => ar.City)
                    .Include(ar => ar.CityManagement)
                    .Include(ar => ar.MembersStatistic)
                    .First();
                var userId = _userManager.GetUserId(User);
                if (!_cityAccessManager.HasAccess(userId, annualReport.CityId))
                    return RedirectToAction("HandleError", "Error", new { code = 403 });
                var cityMembers = _repoWrapper.User
                    .FindByCondition(u =>
                        u.CityMembers.Any(cm => cm.City.ID == annualReport.CityId && cm.EndDate == null))
                    .Include(u => u.UserPlastDegrees);
                var annualReportVM = new AnnualReportViewModel
                {
                    Operation = AnnualReportOperation.Editing,
                    CityName = annualReport.City.Name,
                    CityMembers = _annualReportVMCreator.GetCityMembers(cityMembers),
                    CityLegalStatusTypes = _annualReportVMCreator.GetCityLegalStatusTypes(),
                    AnnualReport = annualReport
                };
                return View("CreateEditAnnualReport", annualReportVM);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }

        [Authorize(Roles = "Admin, Голова Округу")]
        [HttpPost]
        public IActionResult EditAnnualReport(AnnualReport annualReport)
        {
            try
            {
                var annualReportCheck = _repoWrapper.AnnualReports
                    .FindByCondition(ar => ar.ID == annualReport.ID && ar.CityId == annualReport.CityId &&
                                           ar.UserId == annualReport.UserId
                                           && ar.Status == AnnualReportStatus.Unconfirmed)
                    .Include(ar => ar.City)
                    .First();
                var userId = _userManager.GetUserId(User);
                if (!_cityAccessManager.HasAccess(userId, annualReport.CityId))
                    return RedirectToAction("HandleError", "Error", new { code = 403 });
                if (ModelState.IsValid)
                {
                    _repoWrapper.AnnualReports.Update(annualReport);
                    _repoWrapper.Save();
                    ViewData["Message"] =
                        $"Звіт станиці {annualReportCheck.City.Name} за {annualReportCheck.Date.Year} рік відредаговано!";
                    return View("CreateEditAnnualReport");
                }

                var cityMembers = _repoWrapper.User
                    .FindByCondition(u =>
                        u.CityMembers.Any(cm => cm.City.ID == annualReport.CityId && cm.EndDate == null))
                    .Include(u => u.UserPlastDegrees);
                var annualReportViewModel = new AnnualReportViewModel
                {
                    Operation = AnnualReportOperation.Editing,
                    CityName = annualReportCheck.City.Name,
                    CityMembers = _annualReportVMCreator.GetCityMembers(cityMembers),
                    CityLegalStatusTypes = _annualReportVMCreator.GetCityLegalStatusTypes(),
                    AnnualReport = annualReport
                };
                return View("CreateEditAnnualReport", annualReportViewModel);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }
    }
}