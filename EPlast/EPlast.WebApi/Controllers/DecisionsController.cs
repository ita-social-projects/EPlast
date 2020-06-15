using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BussinessLayer;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EPlast.WebApi.Models.Decision;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DecisionsController : ControllerBase
    {
        private readonly IDecisionService _decisionService;
        private readonly IPdfService _pdfService;
        private readonly ILoggerService<DecisionsController> _loggerService;

        public DecisionsController(IPdfService pdfService,
                                  IDecisionService decisionService,
                                  ILoggerService<DecisionsController> loggerService)
        {
            _pdfService = pdfService;
            _decisionService = decisionService;
            _loggerService = loggerService;
        }

     //   [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<DecisionViewModel>> CreateDecision()
        {
            DecisionViewModel decisionViewModel = null;
            try
            {
                var organizations = await _decisionService.GetOrganizationListAsync();
                decisionViewModel = new DecisionViewModel
                {
                    DecisionWrapper = await _decisionService.CreateDecisionAsync(),
                    OrganizationListItems = from item in organizations
                                            select new SelectListItem
                                            {
                                                Text = item.OrganizationName,
                                                Value = item.ID.ToString()
                                            },
                    DecisionTargets = await _decisionService.GetDecisionTargetListAsync(),
                    DecisionStatusTypeListItems = _decisionService.GetDecisionStatusTypes()
                };
            }
            catch (Exception e)
            {
                _loggerService.LogError($"{e.Message}");
            }
            //TODO:
            return Created("Decision", decisionViewModel);
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                DecisionDTO decisionDto = await _decisionService.GetDecisionAsync(id);

                return Ok(decisionDto);
            }
            catch (Exception e)
            {
                _loggerService.LogError($"{e.Message}");

                return BadRequest();
            }
        }
    //   [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update(DecisionDTO decision)
        {
            try
            {
                await _decisionService.ChangeDecisionAsync(decision);
            }
            catch (Exception e)
            {
                _loggerService.LogError($"{e.Message}");

                return BadRequest();
            }

            return NoContent();

        }


    //    [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SaveDecision(DecisionWrapperDTO decisionWrapper)
        {
            try
            {
                //TODO
               // ModelState.Remove("DecisionWrapper.Decision.DecisionStatusType");
                if (decisionWrapper.Decision.DecisionTarget.ID != 0 || decisionWrapper == null)
                {
                  //  ModelState.AddModelError("", "Дані введені неправильно");

                    return BadRequest("Дані введені неправильно");
                    //return Json(new
                    //{
                    //    success = false,
                    //    text = ModelState.Values.SelectMany(v => v.Errors),
                    //    model = decisionWrapper,
                    //    modelstate = ModelState
                    //});
                }

                if (decisionWrapper.File != null && decisionWrapper.File.Length > 10485760)
                {
                    //   ModelState.AddModelError("", "файл за великий (більше 10 Мб)");

                    return BadRequest("файл за великий (більше 10 Мб)");
                   // return Json(new { success = false, text = "file length > 10485760" });
                }

                decisionWrapper.Decision.HaveFile = decisionWrapper.File != null;
                decisionWrapper.Decision.ID = await _decisionService.SaveDecisionAsync(decisionWrapper);

                return Created("Decision", decisionWrapper);
                //return Json(new
                //{
                //    success = true,
                //    Text = "Рішення додано!",
                //    decision = decisionWrapper.Decision,
                //    decisionOrganization = (await _decisionService
                //        .GetDecisionOrganizationAsync(_mapper.Map<OrganizationDTO>(decisionWrapper.Decision.Organization)))
                //        .OrganizationName
                //});
            }
            catch (Exception e)
            {
                _loggerService.LogError($"{e.Message}");

                return BadRequest(e.Message);
                //return Json(new
                //{
                //    success = false,
                //    text = e.Message
                //});
            }
        }

     //   [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ReadDecision()
        {
            List<DecisionViewModel> decisions = null;
            try
            {
                decisions = new List<DecisionViewModel>
                (
                    (await _decisionService.GetDecisionListAsync())
                        .Select(decesion => new DecisionViewModel { DecisionWrapper = decesion })
                        .ToList()
                );
            }
            catch (Exception e)
            {
                _loggerService.LogError($"{e.Message}");
            }

            return Ok(Tuple.Create(await CreateDecision(), decisions));
        }

  //      [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            if (await _decisionService.DeleteDecisionAsync(id))
            {
                return NoContent();
            }

            return NotFound();
        }

    //    [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Download(int id, string filename)
        {
            byte[] fileBytes;
            try
            {

                fileBytes = await _decisionService.DownloadDecisionFileAsync(id);
            }
            catch (Exception e)
            {
                _loggerService.LogError($"{e.Message}");

                return BadRequest();
            }

            return File(fileBytes, _decisionService.GetContentType(id, filename), filename);
        }

    //    [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreatePdfAsync(int objId)
        {
            try
            {
                var arr = await _pdfService.DecisionCreatePDFAsync(objId);

                return File(arr, "application/pdf");
            }
            catch (Exception e)
            {
                _loggerService.LogError($"{e.Message}");

                return BadRequest();
            }
        }
    }
}