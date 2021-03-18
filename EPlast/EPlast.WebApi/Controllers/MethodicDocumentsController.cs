using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.DTO;
using EPlast.WebApi.Models.MethodicDocument;
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
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class MethodicDocumentsController : ControllerBase
    {

        private readonly IMethodicDocumentService _methodicDocService;
        private readonly IMapper _mapper;
        public MethodicDocumentsController(IMethodicDocumentService docService, IMapper mapper)
        {
            _methodicDocService = docService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns data for creating new MethodicDocument
        /// </summary>
        /// <returns>Data for creating new MethodicDocument</returns>
        /// <response code="200">Array of organizations, targets and status types</response>
        [HttpGet("NewMethodicDocument")]
        public async Task<ActionResult<MethodicDocumentCreateViewModel>> GetMetaData()
        {
            MethodicDocumentCreateViewModel documentViewModel = new MethodicDocumentCreateViewModel
            {
                GoverningBodies = await _methodicDocService.GetGoverningBodyListAsync(),
                MethodicDocumentTypesItems = _methodicDocService.GetMethodicDocumentTypes()
            };

            return Ok(documentViewModel);
        }

        /// <summary>
        /// Returns the MethodicDocument by id
        /// </summary>
        /// <param name="id">MethodicDocument id</param>
        /// <returns>MethodicDocument object</returns>
        /// <response code="200">An instance of MethodicDocument</response>
        /// <response code="404">The MethodicDocument does not exist</response>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            MethodicDocumentDTO documentDto = await _methodicDocService.GetMethodicDocumentAsync(id);
            if (documentDto == null)
            {
                return NotFound();
            }
            return Ok(documentDto);
        }

        /// <summary>
        /// Returns all MethodicDocuments
        /// </summary>
        /// <returns>All MethodicDocuments</returns>
        /// <response code="200">Array of all MethodicDocuments</response>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<MethodicDocumentViewModel> documents = (await _methodicDocService.GetMethodicDocumentListAsync())
                        .Select(document =>
                        {
                            var dvm = _mapper.Map<MethodicDocumentViewModel>(document.MethodicDocument);

                            dvm.Type = _methodicDocService.GetMethodicDocumentTypes()
                            .FirstOrDefault(dst => dst.Value == document.MethodicDocument.Type.ToString()).Text;
                            dvm.FileName = document.MethodicDocument.FileName;

                            return dvm;
                        })
                        .ToList();

            return Ok(documents);
        }

        /// <summary>
        /// Updates MethodicDocument
        /// </summary>
        /// <param name="id">MethodicDocument id</param>
        /// <param name="documentDTO">MethodicDocument</param>
        /// <returns>Info that MethodicDocument was created</returns>
        /// <response code="204">An instance of MethodicDocument was created</response>
        /// <response code="400">The id and MethodicDocument id are not same</response>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, MethodicDocumentDTO documentDTO)
        {
            if (id != documentDTO.ID)
            {
                return BadRequest();
            }
            await _methodicDocService.ChangeMethodicDocumentAsync(documentDTO);

            return NoContent();
        }

        /// <summary>
        /// Creates new MethodicDocument
        /// </summary>
        /// <param name="documentWrapper">MethodicDocument wrapper</param>
        /// <returns>Created MethodicDocument object</returns>
        /// <response code="201">Created MethodicDocument object</response>
        /// <response code="400">Problem with file validation or model state is not valid</response>
        [HttpPost]
        public async Task<IActionResult> Save(MethodicDocumentWraperDTO documentWrapper)
        {

            if (documentWrapper.FileAsBase64 == null && documentWrapper.MethodicDocument.FileName != null)
            {
                return BadRequest("Проблеми з завантаженням файлу");
            }
            documentWrapper.MethodicDocument.ID = await _methodicDocService.SaveMethodicDocumentAsync(documentWrapper);
            var methodicDocumentOrganizations = (await _methodicDocService
                        .GetMethodicDocumentOrganizationAsync(documentWrapper.MethodicDocument.GoverningBody))
                        .Name;

            return Created("MethodicDocument", new
            {
                methodicDocument = documentWrapper.MethodicDocument,
                methodicDocumentOrganization = methodicDocumentOrganizations
            });
        }

        /// <summary>
        /// Deletes MethodicDocument by id
        /// </summary>
        /// <param name="id">MethodicDocument id</param>
        /// <returns>Info that MethodicDocument was deleted</returns>
        /// <response code="204">MethodicDocument was deleted</response>
        /// <response code="404">MethodicDocument does not exist</response>
        [HttpDelete("{id:int}")]

        public async Task<IActionResult> Delete(int id)
        {
            await _methodicDocService.DeleteMethodicDocumentAsync(id);

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
            var base64 = await _methodicDocService.DownloadMethodicDocumentFileFromBlobAsync(filename);
            return Ok(base64);
        }

    }
}
