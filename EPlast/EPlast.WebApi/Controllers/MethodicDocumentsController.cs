using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.DTO;
using EPlast.WebApi.Models.MethodicDocument;
using Microsoft.AspNetCore.Authorization;
using EPlast.Resources;
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
        private readonly IPdfService _pdfService;

        public MethodicDocumentsController(IMethodicDocumentService docService, IMapper mapper, IPdfService pdfService)
        {
            _methodicDocService = docService;
            _mapper = mapper;
            _pdfService = pdfService;
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
        /// Returns last MethodicDocument
        /// </summary>
        /// <returns>Last MethodicDocuments</returns>
        /// <response code="200">MethodicDocuments object</response>
        /// <response code="404">Any MethodicDocument not exist</response>
        [HttpGet("Last")]
        public async Task<IActionResult> GetLast()
        {
            try
            {
                var methodic = await _methodicDocService.GetLastAsync();
                return Ok(methodic);
            }
            catch (Exception)
            {
                return NotFound();
            }
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
        /// Get all Users Documents
        /// </summary>
        /// <param name="searchedData">Searched Data</param>
        /// <param name="page">Current page on pagination</param>
        /// <param name="pageSize">Number of records per page</param>
        /// <param name="status">Type of document</param>
        /// <returns>List of UserDocumentsTableObject</returns>
        /// <response code="200">Successful operation</response>
        [HttpGet("DocumentsForTable")]
        public IActionResult GetDocumentsForTable(string searchedData, int page, int pageSize, string status)
        {
            var documents = _methodicDocService.GetDocumentsForTable(searchedData, page, pageSize, status);
            return Ok(documents);
        }

        /// <summary>
        /// Creates new MethodicDocument
        /// </summary>
        /// <param name="documentWrapper">MethodicDocument wrapper</param>
        /// <returns>Created MethodicDocument object</returns>
        /// <response code="201">Created MethodicDocument object</response>
        /// <response code="400">Problem with file validation or model state is not valid</response>
        [HttpPost]
        [Authorize(Roles = Roles.HeadsAndHeadDeputiesAndAdmin)]
        public async Task<IActionResult> Save(MethodicDocumentWraperDTO documentWrapper)
        {
            if (documentWrapper.FileAsBase64 == null && documentWrapper.MethodicDocument.FileName != null)
            {
                return BadRequest("Проблеми з завантаженням файлу");
            }
            documentWrapper.MethodicDocument.ID = await _methodicDocService.SaveMethodicDocumentAsync(documentWrapper);
            var methodicDocumentOrganizations = (await _methodicDocService
                        .GetMethodicDocumentOrganizationAsync(documentWrapper.MethodicDocument.GoverningBody))
                        .GoverningBodyName;

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
        [Authorize(Roles = Roles.AdminAndGBAdmin)]
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

        /// <summary>
        ///  Returns pdf file as base64
        /// </summary>
        /// <param name="objId">MethodicDocument id</param>
        /// <returns>Pdf file as base64 what was created with MethodicDocument data</returns>
        /// <response code="200">Pdf file as base64</response>
        [HttpGet("createpdf/{objId:int}")]
        public async Task<IActionResult> CreatePdf(int objId)
        {
            var fileBytes = await _pdfService.MethodicDocumentCreatePdfAsync(objId);
            var base64EncodedPdf = Convert.ToBase64String(fileBytes);

            return Ok(base64EncodedPdf);
        }
    }
}
