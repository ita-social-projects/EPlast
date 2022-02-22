using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.DTO;
using EPlast.BLL.Interfaces.GoverningBodies;
using EPlast.BLL.Services.Interfaces;
using EPlast.Resources;
using EPlast.WebApi.Models.Decision;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using EPlast.BLL.ExtensionMethods;
using EPlast.BLL.Commands.Decision;
using EPlast.BLL.Queries.Decision;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.HeadsAndHeadDeputiesAndAdminAndPlastun)]
    public class DecisionsController : ControllerBase
    {
        private readonly IPdfService _pdfService;
        private readonly IMapper _mapper;
        private readonly IGoverningBodiesService _governingBodiesService;
        private readonly IUserManagerService _userManagerService;
        private readonly IMediator _mediator;
        public DecisionsController(IPdfService pdfService, IUserManagerService userManagerService, IMapper mapper, IGoverningBodiesService governingBodiesService, IMediator mediator)
        {
            _pdfService = pdfService;
            _userManagerService = userManagerService;
            _mapper = mapper;
            _governingBodiesService = governingBodiesService;
            _mediator = mediator;
        }

        /// <summary>
        /// Returns data for creating new decision
        /// </summary>
        /// <returns>Data for creating new decision</returns>
        /// <response code="200">Array of organizations, targets and status types</response>
        [HttpGet("NewDecision")]
        [Authorize(Roles = Roles.HeadsAndHeadDeputiesAndAdmin)]
        public async Task<ActionResult<DecisionCreateViewModel>> GetMetaData()
        {
            DecisionCreateViewModel decisionViewModel = new DecisionCreateViewModel
            {
                GoverningBodies = await _governingBodiesService.GetGoverningBodiesListAsync(),
                DecisionStatusTypeListItems = new GetDecisionStatusTypesExtention().GetDecesionStatusTypes()
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
            var query = new GetDecisionAsyncQuery(id);
            var decisionDto = await _mediator.Send(query);
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
            var query = new GetDecisionListAsyncQuery();         
            List<DecisionViewModel> decisions = (await _mediator.Send(query))
                        .Select(decesion =>
                        {
                            var dvm = _mapper.Map<DecisionViewModel>(decesion.Decision);

                            dvm.DecisionStatusType = new GetDecisionStatusTypesExtention().GetDecesionStatusTypes()
                            .FirstOrDefault(dst => dst.Value == decesion.Decision.DecisionStatusType.ToString()).Text;
                            dvm.FileName = decesion.Decision.FileName;

                            return dvm;
                        })
                        .ToList();

            return Ok(decisions);
        }

        /// <summary>
        /// Get all Decisions
        /// </summary>
        /// <param name="searchedData">Searched Data</param>
        /// <param name="page">Current page on pagination</param>
        /// <param name="pageSize">Number of records per page</param>
        /// <returns>List of DecisionTableObject</returns>
        /// <response code="200">Successful operation</response>
        [HttpGet("DecisionsForTable")]
        public IActionResult GetDecisionsForTable(string searchedData, int page, int pageSize)
        {
            var query = new GetDecisionsForTableQuery(searchedData, page, pageSize);
            var decisions = _mediator.Send(query).Result;
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
        [Authorize(Roles = Roles.HeadsAndHeadDeputiesAndAdmin)]
        public async Task<IActionResult> Update(int id, DecisionDTO decision)
        {
            if (id != decision.ID)
            {
                return BadRequest();
            }

            var userId = _userManagerService.GetCurrentUserId(HttpContext.User);
            var query = new GetDecisionAsyncQuery(decision.ID);
            var decisionCreatorIs = (await _mediator.Send(query)).UserId;
            if (!decisionCreatorIs.Equals(userId) )
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            var command = new UpdateCommand(decision);
            await _mediator.Send(command);

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
        [Authorize(Roles = Roles.HeadsAndHeadDeputiesAndAdmin)]
        public async Task<IActionResult> Save(DecisionWrapperDTO decisionWrapper)
        {

            if (decisionWrapper.FileAsBase64 == null && decisionWrapper.Decision.FileName != null)
            {
                return BadRequest("Проблеми з завантаженням файлу");
            }
            var query = new SaveDecisionAsyncCommand(decisionWrapper);
            decisionWrapper.Decision.ID = await _mediator.Send(query); //_decisionService.SaveDecisionAsync(decisionWrapper);
            var getDecisionOrganizationAsync = new GetDecisionOrganizationAsyncQuery(decisionWrapper.Decision.GoverningBody);
            var decisionOrganizations = (await _mediator.Send(getDecisionOrganizationAsync)).GoverningBodyName;

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
        [Authorize(Roles = Roles.AdminAndRegionBoardHead)]
        public async Task<IActionResult> Delete(int id)
        {
            var query = new DeleteDecisionAsyncCommand(id);
            await _mediator.Send(query);

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
            var query = new DownloadDecisionFileFromBlobAsyncQuery(filename);
            var base64 = await _mediator.Send(query); //_decisionService.DownloadDecisionFileFromBlobAsync(filename);

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
        /// <summary>
        /// Get targets
        /// </summary>
        /// <param name="searchedData">Searched Data</param>
        /// <returns>List of Targets</returns>
        /// <response code="200">Successful operation</response>
        [HttpGet("targetList/{searchedData}")]
        public async Task<IActionResult> GetDecisionTargetSearchList(string searchedData)
        {
            var query = new GetDecisionTargetSearchListAsyncQuery(searchedData);
            var targets = await _mediator.Send(query);
            return Ok(targets);
        }
    }
}