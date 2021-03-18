using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.DTO;
using EPlast.WebApi.Models.Decision;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Голова Округу, Голова Станиці, Голова Куреня, Пластун")]
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

        /// <summary>
        /// Returns data for creating new decision
        /// </summary>
        /// <returns>Data for creating new decision</returns>
        /// <response code="200">Array of organizations, targets and status types</response>
        [HttpGet("NewDecision")]
        public async Task<ActionResult<DecisionCreateViewModel>> GetMetaData()
        {
            DecisionCreateViewModel decisionViewModel = new DecisionCreateViewModel
            {
                GoverningBodies = await _decisionService.GetGoverningBodyListAsync(),
                DecisionTargets = await _decisionService.GetDecisionTargetListAsync(),
                DecisionStatusTypeListItems = _decisionService.GetDecisionStatusTypes()
            };

            return Ok(decisionViewModel);
        }

        /// <summary>
        /// Returns the decision by id
        /// </summary>
        /// <param name="id">Decision id</param>
        /// <returns>Decision object</returns>
        /// <response code="200">An instance of decision</response>
        /// <response code="404">The decision does not exist</response>
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

        /// <summary>
        /// Returns all decisions
        /// </summary>
        /// <returns>All decisions</returns>
        /// <response code="200">Array of all decisions</response>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<DecisionViewModel> decisions = (await _decisionService.GetDecisionListAsync())
                        .Select(decesion =>
                        {
                            var dvm = _mapper.Map<DecisionViewModel>(decesion.Decision);

                            dvm.DecisionStatusType = _decisionService.GetDecisionStatusTypes()
                            .FirstOrDefault(dst => dst.Value == decesion.Decision.DecisionStatusType.ToString()).Text;
                            dvm.FileName = decesion.Decision.FileName;

                            return dvm;
                        })
                        .ToList();

            return Ok(decisions);
        }

        /// <summary>
        /// Updates decision
        /// </summary>
        /// <param name="id">decision id</param>
        /// <param name="decision">decision</param>
        /// <returns>Info that decision was created</returns>
        /// <response code="204">An instance of decision was created</response>
        /// <response code="400">The id and decision id are not same</response>
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

        /// <summary>
        /// Creates new decision
        /// </summary>
        /// <param name="decisionWrapper">Decision wrapper</param>
        /// <returns>Created decision object</returns>
        /// <response code="201">Created decision object</response>
        /// <response code="400">Problem with file validation or model state is not valid</response>
        [HttpPost]
        public async Task<IActionResult> Save(DecisionWrapperDTO decisionWrapper)
        {

            if (decisionWrapper.FileAsBase64 == null && decisionWrapper.Decision.FileName != null)
            {
                return BadRequest("Проблеми з завантаженням файлу");
            }
            decisionWrapper.Decision.ID = await _decisionService.SaveDecisionAsync(decisionWrapper);
            var decisionOrganizations = (await _decisionService
                        .GetDecisionOrganizationAsync(decisionWrapper.Decision.GoverningBody))
                        .Name;

            return Created("Decisions", new
            {
                decision = decisionWrapper.Decision,
                decisionOrganization = decisionOrganizations
            });
        }

        /// <summary>
        /// Deletes decision by id
        /// </summary>
        /// <param name="id">Decision id</param>
        /// <returns>Info that decision was deleted</returns>
        /// <response code="204">Decision was deleted</response>
        /// <response code="404">Decision does not exist</response>
        [HttpDelete("{id:int}")]

        public async Task<IActionResult> Delete(int id)
        {
            await _decisionService.DeleteDecisionAsync(id);

            return NoContent();
        }

        /// <summary>
        /// Returns file by name as base64
        /// </summary>
        /// <param name="filename">File name</param>
        /// <returns>File as base64</returns>
        /// <response code="200">File as base64</response>
        [HttpGet("downloadfile/{filename}")]
        public async Task<IActionResult> Download(string filename)
        {
            var base64 = await _decisionService.DownloadDecisionFileFromBlobAsync(filename);

            return Ok(base64);
        }

        /// <summary>
        ///  Returns pdf file as base64
        /// </summary>
        /// <param name="objId">Decision id</param>
        /// <returns>Pdf file as base64 what was created with decision data</returns>
        /// <response code="200">Pdf file as base64</response>
        [HttpGet("createpdf/{objId:int}")]
        public async Task<IActionResult> CreatePdf(int objId)
        {
            byte[] fileBytes = await _pdfService.DecisionCreatePDFAsync(objId);
            string base64EncodedPDF = Convert.ToBase64String(fileBytes);

            return Ok(base64EncodedPDF);
        }

    }
}