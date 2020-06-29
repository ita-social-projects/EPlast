using EPlast.BLL;
using EPlast.BLL.DTO;
using EPlast.BLL.Services.Interfaces;
using EPlast.WebApi.Models.Decision;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BLL.Interfaces.Logging;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
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

        [HttpGet("NewDecision")]
        public async Task<ActionResult<DecisionViewModel>> Create()
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

            return Ok(decisionViewModel);
        }

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


        [HttpPost]
        public async Task<IActionResult> Save(DecisionWrapperDTO decisionWrapper)
        {
            try
            {

                if (decisionWrapper.Decision.DecisionTarget.ID != 0 || decisionWrapper == null)
                {
                    return BadRequest("Дані введені неправильно");
                }

                if (decisionWrapper.File != null && decisionWrapper.File.Length > 10485760)
                {
                    return BadRequest("файл за великий (більше 10 Мб)");
                }

                decisionWrapper.Decision.HaveFile = decisionWrapper.File != null;
                decisionWrapper.Decision.ID = await _decisionService.SaveDecisionAsync(decisionWrapper);
                var decisionOrganizations = (await _decisionService
                            .GetDecisionOrganizationAsync(decisionWrapper.Decision.Organization))
                            .OrganizationName;
                return Created("decisions", new
                {
                    decision = decisionWrapper.Decision,
                    decisionOrganization = decisionOrganizations
                });
            }
            catch (Exception e)
            {
                _loggerService.LogError($"{e.Message}");

                return BadRequest(e.Message);
            }
        }

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

            return Ok(Tuple.Create(await Create(), decisions));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            if (await _decisionService.DeleteDecisionAsync(id))
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpPost("downloadfile")]
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

        [HttpPost("createpdf")]
        public async Task<IActionResult> CreatePdf(int objId)
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