using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.DTO;
using EPlast.WebApi.Models.Decision;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DecisionsController : ControllerBase
    {
        private readonly IDecisionService _decisionService;
        private readonly IPdfService _pdfService;
        private readonly IMapper _mapper;
        public DecisionsController(IPdfService pdfService, IDecisionService decisionService, IMapper mapper)
        {
            _pdfService = pdfService;
            _decisionService = decisionService;
            _mapper = mapper;
        }

        [HttpGet("NewDecision")]
        public async Task<ActionResult<DecisionCreateViewModel>> GetMetaData()
        {
            DecisionCreateViewModel decisionViewModel = new DecisionCreateViewModel
            {
                Organizations = await _decisionService.GetOrganizationListAsync(),
                DecisionTargets = await _decisionService.GetDecisionTargetListAsync(),
                DecisionStatusTypeListItems = _decisionService.GetDecisionStatusTypes()
            };

            return Ok(decisionViewModel);
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

            if (!ModelState.IsValid)
            {
                return BadRequest("Дані введені неправильно");
            }
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
            List<DecisionViewModel> decisions = (await _decisionService.GetDecisionListAsync())
                        .Select(decesion => {
                            var dvm = _mapper.Map<DecisionViewModel>(decesion.Decision);

                            dvm.DecisionStatusType = _decisionService.GetDecisionStatusTypes()
                            .FirstOrDefault(dst => dst.Value == decesion.Decision.DecisionStatusType.ToString()).Text;
                            dvm.FileName = decesion.Decision.FileName;

                            return dvm;
                        })
                        .ToList();

            return Ok(decisions);
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

        [HttpGet("downloadfile/{filename}")]
        public async Task<IActionResult> Download(string filename)
        {
            var base64 = await _decisionService.DownloadDecisionFileFromBlobAsync(filename);

            return Ok(base64);
        }

        [HttpGet("createpdf/{objId:int}")]
        public async Task<IActionResult> CreatePdf(int objId)
        {
            byte[] fileBytes = await _pdfService.DecisionCreatePDFAsync(objId);
            string base64EncodedPDF = Convert.ToBase64String(fileBytes);

            return Ok(base64EncodedPDF);
        }
    }
}