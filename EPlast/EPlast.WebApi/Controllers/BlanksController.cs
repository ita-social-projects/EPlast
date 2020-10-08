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
        private readonly IBlankBiographyDocumentService _blankBiographyDocumentService;

        public BlanksController(IBlankBiographyDocumentService blankBiographyDocumentService)
        {
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

            return Created("",document);
        }

        [HttpGet("GetDocumentByUserId/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetDocumentByUserId(string userId)
        {
            return Ok(await _blankBiographyDocumentService.GetDocumentByUserId(userId));
        }

        [HttpDelete("RemoveBiographyDocument/{documentId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> RemoveDocument(int documentId)
        {
            await _blankBiographyDocumentService.DeleteFileAsync(documentId);

            return NoContent();
        }

        [HttpGet("BiographyDocumentBase64/{fileName}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetFileBase64(string fileName)
        {
            return Ok(await _blankBiographyDocumentService.DownloadFileAsync(fileName));
        }
    }
}
