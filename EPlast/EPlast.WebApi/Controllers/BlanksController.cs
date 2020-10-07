using AutoMapper;
using EPlast.BLL.DTO.Blank;
using EPlast.BLL.Interfaces.Blank;
using EPlast.BLL.Interfaces.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlanksController : ControllerBase
    {
        private readonly ILoggerService<BlanksController> _logger;
        private readonly IMapper _mapper;
        private readonly IBlankBiographyDocumentService _blankBiographyDocumentService;

        public BlanksController(ILoggerService<BlanksController> logger,
            IMapper mapper, 
            IBlankBiographyDocumentService blankBiographyDocumentService)
        {
            _logger = logger;
            _mapper = mapper;
            _blankBiographyDocumentService = blankBiographyDocumentService;
        }

        /// <summary>
        /// Add a document to the blank
        /// </summary>
        /// <param name="document">An information about a specific document</param>
        /// <returns>A newly created Blank document</returns>
        [HttpPost("AddDocument/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddDocument(BlankBiographyDocumentsDTO document)
        {
            await _blankBiographyDocumentService.AddDocumentAsync(document);
            _logger.LogInformation($"Document with id {{{document.ID}}} was added.");

            return Ok(document);
        }

        [HttpGet("GetDocumentByUserId/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetDocumentByUserId(string userId)
        {
           var document =  await _blankBiographyDocumentService.GetDocumentByUserId(userId);

            return Ok(document);
        }

        [HttpDelete("RemoveBiographyDocument/{documentId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> RemoveDocument(int documentId)
        {
            await _blankBiographyDocumentService.DeleteFileAsync(documentId);
            _logger.LogInformation($"Document with id {{{documentId}}} was deleted.");

            return Ok();
        }

        [HttpGet("BiographyDocumentBase64/{fileName}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetFileBase64(string fileName)
        {
            var fileBase64 = await _blankBiographyDocumentService.DownloadFileAsync(fileName);

            return Ok(fileBase64);
        }
    }
}
