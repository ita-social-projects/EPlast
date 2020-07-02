using EPlast.BLL;
using EPlast.BLL.DTO;
using EPlast.WebApi.Models.Decision;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<DecisionViewModel>> GetMetaData()
        {
            return Ok(await DecisionViewModel.GetNewDecisionViewModel(_decisionService));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            DecisionDTO decisionDto = await _decisionService.GetDecisionAsync(id);
            if (decisionDto == null)
            {
                return NotFound();
            }

            return Ok(decisionDto);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, DecisionDTO decision)
        {
            if (id != decision.ID)
            {
                return BadRequest();
            }
            await _decisionService.ChangeDecisionAsync(decision);

            return NoContent();

        }

        [HttpPost]
        public async Task<IActionResult> Save(DecisionWrapperDTO decisionWrapper)
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

            return Created("Decisions", new
            {
                decision = decisionWrapper.Decision,
                decisionOrganization = decisionOrganizations
            });

        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<DecisionViewModel> decisions = new List<DecisionViewModel>
                (
                    (await _decisionService.GetDecisionListAsync())
                        .Select(decesion => new DecisionViewModel { DecisionWrapper = decesion })
                        .ToList()
                );

            return Ok(Tuple.Create(await DecisionViewModel.GetNewDecisionViewModel(_decisionService), decisions));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {

            if (await _decisionService.DeleteDecisionAsync(id))
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpPost("downloadfile/{id:int}")]
        public async Task<IActionResult> Download(int id, string filename)
        {
            byte[] fileBytes = await _decisionService.DownloadDecisionFileAsync(id);

            return File(fileBytes, _decisionService.GetContentType(id, filename), filename);
        }

        [HttpPost("createpdf/{objId:int}")]
        public async Task<IActionResult> CreatePdf(int objId)
        {
            byte[] fileBytes = await _pdfService.DecisionCreatePDFAsync(objId);

            return File(fileBytes, "application/pdf");
        }
    }
}