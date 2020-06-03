using AutoMapper;
using EPlast.BussinessLayer;
using EPlast.BussinessLayer.DTO;
using EPlast.DataAccess.Entities;
using EPlast.Models;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly IMapper _mapper;
        private readonly IPDFService _PDFService;

        public DocumentationController(IPDFService PDFService, IDecisionService decisionService, IMapper mapper)
        {
            _PDFService = PDFService;
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
    }
}