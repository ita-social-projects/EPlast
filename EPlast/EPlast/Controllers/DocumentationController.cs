using EPlast.BussinessLayer;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Models.ViewModelInitializations.Interfaces;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EPlast.Wrapper;
using EPlast.BussinessLayer.AccessManagers.Interfaces;

namespace EPlast.Controllers
{
    public class DocumentationController : Controller
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IAnnualReportVMInitializer _annualReportVMCreator;
        private readonly IDecisionVMIitializer _decisionVMCreator;
        private readonly IPDFService _PDFService;
        private readonly UserManager<User> _userManager;
        private readonly IHostingEnvironment _appEnvironment;
        private readonly IViewAnnualReportsVMInitializer _viewAnnualReportsVMInitializer;
        private readonly IDirectoryManager _directoryManager;
        private readonly IFileManager _fileManager;
        private readonly IFileStreamManager _fileStreamManager;
        private readonly ICityAccessManager _cityAccessManager;
        private readonly ILogger _logger;

        private const string DecesionsDocumentFolder = @"\documents\";

        public DocumentationController(IRepositoryWrapper repoWrapper, UserManager<User> userManager, IAnnualReportVMInitializer annualReportVMCreator,
            IDecisionVMIitializer decisionVMCreator, IPDFService PDFService, IHostingEnvironment appEnvironment, IViewAnnualReportsVMInitializer viewAnnualReportsVMInitializer,
            ICityAccessManager cityAccessManager, IDirectoryManager directoryManager, IFileManager fileManager, IFileStreamManager fileStreamManager,
            ILogger<DocumentationController> logger)
        {
            _repoWrapper = repoWrapper;
            _annualReportVMCreator = annualReportVMCreator;
            _userManager = userManager;
            _PDFService = PDFService;
            _decisionVMCreator = decisionVMCreator;
            _appEnvironment = appEnvironment;
            _viewAnnualReportsVMInitializer = viewAnnualReportsVMInitializer;
            _directoryManager = directoryManager;
            _fileManager = fileManager;
            _fileStreamManager = fileStreamManager;
            _cityAccessManager = cityAccessManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public DecesionViewModel CreateDecesion()
        {
            try
            {
                DecesionViewModel decesionViewModel = new DecesionViewModel
                {
                    Decesion = new Decesion(),
                    OrganizationListItems = (from item in _repoWrapper.Organization.FindAll()
                                             select new SelectListItem
                                             {
                                                 Text = item.OrganizationName,
                                                 Value = item.ID.ToString()
                                             }),
                    DecesionTargets = _repoWrapper.DecesionTarget.FindAll().ToList(),
                    DecesionStatusTypeListItems = _decisionVMCreator.GetDecesionStatusTypes()
                };

                return decesionViewModel;
            }
            catch
            {
                RedirectToAction("HandleError", "Error");
                return null;
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public JsonResult GetDecesion(int id)
        {
            try
            {
                var decesion = _repoWrapper.Decesion.FindByCondition(x => x.ID == id).First();
                return Json(new { success = true, decesion });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public JsonResult ChangeDecesion(DecesionViewModel decesionViewModel)
        {
            try
            {
                var decesion = _repoWrapper.Decesion.FindByCondition(x => x.ID == decesionViewModel.Decesion.ID).First();
                decesion.Name = decesionViewModel.Decesion.Name;
                decesion.Description = decesionViewModel.Decesion.Description;
                _repoWrapper.Decesion.Update(decesion);
                _repoWrapper.Save();
                return Json(new
                {
                    success = true,
                    text = "Зміни пройшли успішно!",
                    decesion
                });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<JsonResult> SaveDecesionAsync(DecesionViewModel decesionViewModel)
        {
            try
            {
                ModelState.Remove("Decesion.DecesionStatusType");
                if (!ModelState.IsValid && decesionViewModel.Decesion.DecesionTarget.ID != 0 || decesionViewModel == null)
                {
                    ModelState.AddModelError("", "Дані введені неправильно");
                    return Json(new { success = false, text = ModelState.Values.SelectMany(v => v.Errors), model = decesionViewModel, modelstate = ModelState });
                }

                if (decesionViewModel.File != null && decesionViewModel.File.Length > 10485760)
                {
                    ModelState.AddModelError("", "файл за великий (більше 10 Мб)");
                    return Json(new { success = false, text = "file lenght > 10485760" });
                }

                decesionViewModel.Decesion.HaveFile = decesionViewModel.File != null ? true : false;

                _repoWrapper.Decesion.Attach(decesionViewModel.Decesion);
                _repoWrapper.Decesion.Create(decesionViewModel.Decesion);
                _repoWrapper.Save();

                if (decesionViewModel.Decesion.HaveFile)
                {
                    try
                    {
                        string path = _appEnvironment.WebRootPath + DecesionsDocumentFolder + decesionViewModel.Decesion.ID;
                        _directoryManager.CreateDirectory(path);

                        if (!_directoryManager.Exists(path))
                        {
                            throw new ArgumentException($"directory '{path}' is not exist");
                        }

                        if (decesionViewModel.File != null)
                        {
                            path = Path.Combine(path, decesionViewModel.File.FileName);

                            using (var stream = _fileStreamManager.GenerateFileStreamManager(path, FileMode.Create))
                            {
                                await _fileStreamManager.CopyToAsync(decesionViewModel.File, stream.GetStream());
                                if (!_fileManager.Exists(path))
                                {
                                    throw new ArgumentException($"File was not created it '{path}' directory");
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        return Json(new { success = false, text = e.Message });
                    }
                }
                return Json(new
                {
                    success = true,
                    Text = "Рішення додано!",
                    decesion = decesionViewModel.Decesion,
                    decesionOrganization = _repoWrapper.Organization.FindByCondition(x => x.ID == decesionViewModel.Decesion.Organization.ID).Select(x => x.OrganizationName)
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    success = false,
                    text = e.Message
                });
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ReadDecesion()
        {
            try
            {
                var decisions = new List<DecesionViewModel>(
                    _repoWrapper.Decesion
                    .Include(x => x.DecesionTarget, x => x.Organization)
                    .Take(200)
                    .Select(decesion => new DecesionViewModel
                    {
                        Decesion = decesion
                    })
                    .ToList());
                foreach (var decesion in decisions)
                {
                    string path = _appEnvironment.WebRootPath + DecesionsDocumentFolder + decesion.Decesion.ID;
                    if (!decesion.Decesion.HaveFile || !_directoryManager.Exists(path))
                    {
                        continue;
                    }
                    var files = _directoryManager.GetFiles(path);

                    if (files.Length == 0)
                    {
                        throw new ArgumentException($"File count in '{path}' is 0");
                    }

                    decesion.Filename = Path.GetFileName(files.First());
                }
                return View(Tuple.Create(CreateDecesion(), decisions));
            }
            catch
            {
                return RedirectToAction("HandleError", "Error");
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Download(string id, string filename)
        {
            try
            {
                if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(id))
                    return Content("filename or id not present");

                var path = Path.Combine(_appEnvironment.WebRootPath + DecesionsDocumentFolder, id);

                if (!_directoryManager.Exists(path) || _directoryManager.GetFiles(path).Length == 0)
                {
                    throw new ArgumentException($"directory '{path}' is not exist");
                }
                path = Path.Combine(path, filename);
                var memory = new MemoryStream();
                using (var stream = _fileStreamManager.GenerateFileStreamManager(path, FileMode.Open))
                {
                    await _fileStreamManager.CopyToAsync(stream.GetStream(), memory);

                    if (memory.Length == 0)
                    {
                        throw new ArgumentException("memory length is 0");
                    }
                }
                memory.Position = 0;
                return File(memory, GetContentType(path), Path.GetFileName(path));
            }
            catch
            {
                return RedirectToAction("HandleError", "Error");
            }
        }

        private static string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".mp4","video/mp4" }
            };
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> CreatePDFAsync(int objId)
        {
            try
            {
                if (objId <= 0)
                {
                    throw new ArgumentException("Cannot crated pdf id is not valid");
                }

                byte[] arr = await _PDFService.DecesionCreatePDFAsync(_repoWrapper.Decesion.Include(x => x.DecesionTarget,
                        x => x.Organization)
                    .FirstOrDefault(x => x.ID == objId));
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
                else
                {
                    ViewData["ErrorMessage"] = $"Звіт станиці {city.Name} за {DateTime.Now.Year} рік вже існує!";
                    return View("CreateEditAnnualReport");
                }
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
                else
                {
                    ViewData["ErrorMessage"] = $"Звіт станиці {city.Name} за {DateTime.Now.Year} рік вже існує!";
                    return View("CreateEditAnnualReport");
                }
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
                        .FindByCondition(ar => ar.CityId == annualReport.CityId && ar.Date.Year == annualReport.Date.Year)
                        .FirstOrDefault();
                    if (annualReportCheck == null)
                    {
                        _repoWrapper.AnnualReports.Create(annualReport);
                        _repoWrapper.Save();
                        ViewData["Message"] = $"Звіт станиці {city.Name} за {annualReport.Date.Year} рік створено!";
                    }
                    else
                    {
                        ViewData["ErrorMessage"] = $"Звіт станиці {city.Name} за {annualReport.Date.Year} рік вже існує!";
                    }
                    return View("CreateEditAnnualReport");
                }
                else
                {
                    var cityMembers = _repoWrapper.User
                        .FindByCondition(u => u.CityMembers.Any(cm => cm.City.ID == annualReport.CityId && cm.EndDate == null))
                        .Include(u => u.UserPlastDegrees);
                    var annualReportViewModel = new AnnualReportViewModel
                    {
                        Operation = AnnualReportOperation.Creating,
                        CityName = city.Name,
                        CityMembers = _annualReportVMCreator.GetCityMembers(cityMembers),
                        CityLegalStatusTypes = _annualReportVMCreator.GetCityLegalStatusTypes(),
                        AnnualReport = annualReport
                    };
                    ViewData["ErrorMessage"] = $"Звіт заповнений некоректно!";
                    return View("CreateEditAnnualReport", annualReportViewModel);
                }
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
                {
                    return RedirectToAction("HandleError", "Error", new { code = 403 });
                }
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
                {
                    return RedirectToAction("HandleError", "Error", new { code = 403 });
                }
                var annualReportOld = _repoWrapper.AnnualReports
                    .FindByCondition(ar => ar.CityId == annualReport.CityId && ar.Status == AnnualReportStatus.Confirmed)
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
                CityAdministration cityAdminOld = _repoWrapper.CityAdministration
                        .FindByCondition(ca => ca.CityId == annualReport.CityId && ca.AdminTypeId == adminType.ID)
                        .Include(ca => ca.User)
                        .LastOrDefault();
                annualReport.CityManagement.CityAdminOldId = cityAdminOld?.ID;
                if (cityAdminOld != null && annualReport.CityManagement.CityAdminNew != null
                    && annualReport.CityManagement.UserId != cityAdminOld.UserId && cityAdminOld.EndDate == null)
                {
                    cityAdminOld.EndDate = DateTime.Now;
                    _repoWrapper.CityAdministration.Update(cityAdminOld);
                    await _userManager.RemoveFromRoleAsync(cityAdminOld.User, adminType.AdminTypeName);
                }

                // create newCityAdmin
                if ((cityAdminOld == null || cityAdminOld?.EndDate != null) && annualReport.CityManagement.CityAdminNew != null)
                {
                    CityAdministration cityAdminNew = new CityAdministration
                    {
                        UserId = annualReport.CityManagement.UserId,
                        CityId = annualReport.CityId,
                        AdminTypeId = adminType.ID,
                        StartDate = DateTime.Now
                    };
                    _repoWrapper.CityAdministration.Create(cityAdminNew);
                    await _userManager.AddToRoleAsync(annualReport.CityManagement.CityAdminNew, adminType.AdminTypeName);
                }

                // update oldCityLegalStatus EndDate
                CityLegalStatus cityLegalStatusOld = _repoWrapper.CityLegalStatuses
                    .FindByCondition(cls => cls.CityId == annualReport.CityId)
                    .LastOrDefault();
                annualReport.CityManagement.CityLegalStatusOldId = cityLegalStatusOld?.Id;
                if (cityLegalStatusOld != null && annualReport.CityManagement.CityLegalStatusNew != cityLegalStatusOld?.LegalStatusType
                    && cityLegalStatusOld?.DateFinish == null)
                {
                    cityLegalStatusOld.DateFinish = DateTime.Now;
                    _repoWrapper.CityLegalStatuses.Update(cityLegalStatusOld);
                }

                // create newCityLegalStatus
                if (cityLegalStatusOld == null || cityLegalStatusOld.DateFinish != null)
                {
                    CityLegalStatus cityLegalStatusNew = new CityLegalStatus
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
                {
                    return RedirectToAction("HandleError", "Error", new { code = 403 });
                }

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
                    .FindByCondition(ca => ca.ID > annualReport.CityManagement.CityAdminOldId && ca.CityId == annualReport.CityId && ca.AdminTypeId == adminType.ID)
                    .Include(ca => ca.User);
                foreach (var cityAdministration in cityAdministrations)
                {
                    _repoWrapper.CityAdministration.Delete(cityAdministration);
                    await _userManager.RemoveFromRoleAsync(cityAdministration.User, adminType.AdminTypeName);
                }

                // cityLegalStatus revert
                if (annualReport.CityManagement.CityLegalStatusOld != null)
                {
                    annualReport.CityManagement.CityLegalStatusOld.DateFinish = null;
                }
                var cityLegalStatuses = _repoWrapper.CityLegalStatuses
                    .FindByCondition(cls => cls.Id > annualReport.CityManagement.CityLegalStatusOldId);
                foreach (var cityLegalStatus in cityLegalStatuses)
                {
                    _repoWrapper.CityLegalStatuses.Delete(cityLegalStatus);
                }

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
                {
                    return RedirectToAction("HandleError", "Error", new { code = 403 });
                }
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
                {
                    return RedirectToAction("HandleError", "Error", new { code = 403 });
                }
                var cityMembers = _repoWrapper.User
                    .FindByCondition(u => u.CityMembers.Any(cm => cm.City.ID == annualReport.CityId && cm.EndDate == null))
                    .Include(u => u.UserPlastDegrees);
                var annualReportVM = new AnnualReportViewModel
                {
                    Operation = AnnualReportOperation.Editing,
                    CityName = annualReport.City.Name,
                    CityMembers = _annualReportVMCreator.GetCityMembers(cityMembers),
                    CityLegalStatusTypes = _annualReportVMCreator.GetCityLegalStatusTypes(),
                    AnnualReport = annualReport,
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
                    .FindByCondition(ar => ar.ID == annualReport.ID && ar.CityId == annualReport.CityId && ar.UserId == annualReport.UserId
                    && ar.Status == AnnualReportStatus.Unconfirmed)
                    .Include(ar => ar.City)
                    .First();
                var userId = _userManager.GetUserId(User);
                if (!_cityAccessManager.HasAccess(userId, annualReport.CityId))
                {
                    return RedirectToAction("HandleError", "Error", new { code = 403 });
                }
                if (ModelState.IsValid)
                {
                    _repoWrapper.AnnualReports.Update(annualReport);
                    _repoWrapper.Save();
                    ViewData["Message"] = $"Звіт станиці {annualReportCheck.City.Name} за {annualReportCheck.Date.Year} рік відредаговано!";
                    return View("CreateEditAnnualReport");
                }
                else
                {
                    var cityMembers = _repoWrapper.User
                        .FindByCondition(u => u.CityMembers.Any(cm => cm.City.ID == annualReport.CityId && cm.EndDate == null))
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
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 500 });
            }
        }
    }
}