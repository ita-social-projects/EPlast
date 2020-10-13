using EPlast.BLL.DTO.Blank;
using EPlast.BLL.Interfaces.Blank;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlanksController : ControllerBase
    {
        private readonly IBlankBiographyDocumentService _blankBiographyDocumentService;
        private readonly IBlankAchievementDocumentService _blankAchievementDocumentService;

        public BlanksController(IBlankBiographyDocumentService blankBiographyDocumentService,
           IBlankAchievementDocumentService blankAchievementDocumentService)
        {
            _blankBiographyDocumentService = blankBiographyDocumentService;
            _blankAchievementDocumentService = blankAchievementDocumentService;
        }

        /// <summary>
        /// Add a document to the biography blank
        /// </summary>
        /// <param name="biographyDocument">An information about a specific document</param>
        /// <returns>A newly created Blank biography document</returns>
        [HttpPost("AddDocument/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddBiographyDocument(BlankBiographyDocumentsDTO biographyDocument)
        {
            await _blankBiographyDocumentService.AddDocumentAsync(biographyDocument);

            return Created("", biographyDocument);
        }

        [HttpPost("AddAchievementDocumet/{userId}")]

        public async Task<IActionResult> AddAchievementDocument(List<AchievementDocumentsDTO> achievementDocuments)
        {
            await _blankAchievementDocumentService.AddDocumentAsync(achievementDocuments);

            return Created("AchievementDocument", achievementDocuments);
        }

        [HttpGet("GetDocumentByUserId/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetDocumentByUserId(string userId)
        {
            return Ok(await _blankBiographyDocumentService.GetDocumentByUserId(userId));
        }

        [HttpGet("GetAchievementDocumentsByUserId/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetDocumentsByUserId(string userId)
        {
            return Ok(await _blankAchievementDocumentService.GetDocumentsByUserId(userId));
        }

        [HttpDelete("RemoveBiographyDocument/{documentId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> RemoveDocument(int documentId)
        {
            await _blankBiographyDocumentService.DeleteFileAsync(documentId);

            return NoContent();
        }

        [HttpDelete("RemoveAchievementDocument/{documentId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> RemoveAchievementDocument(int documentId)
        {
            await _blankAchievementDocumentService.DeleteFileAsync(documentId);

            return NoContent();
        }

        [HttpGet("BiographyDocumentBase64/{fileName}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetFileBase64(string fileName)
        {
            return Ok(await _blankBiographyDocumentService.DownloadFileAsync(fileName));
        }

        [HttpGet("AchievementDocumentBase64/{fileName}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetFileAchievementBase64(string fileName)
        {
            return Ok(await _blankAchievementDocumentService.DownloadFileAsync(fileName));
        }

    }
}
