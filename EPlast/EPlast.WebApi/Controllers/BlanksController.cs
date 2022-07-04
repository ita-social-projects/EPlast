using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BLL;
using EPlast.BLL.DTO.Blank;
using EPlast.BLL.Interfaces.Blank;
using EPlast.BLL.Interfaces.Logging;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlanksController : ControllerBase
    {
        private readonly IBlankBiographyDocumentService _blankBiographyDocumentService;
        private readonly IBlankAchievementDocumentService _blankAchievementDocumentService;
        private readonly IBlankExtractFromUpuDocumentService _blankExtractFromUPUDocumentService;
        private readonly IPdfService _pdfService;
        private readonly ILoggerService _loggerService;
        private readonly UserManager<User> _userManager;


        public BlanksController(IBlankBiographyDocumentService blankBiographyDocumentService,
           IBlankAchievementDocumentService blankAchievementDocumentService,
           IBlankExtractFromUpuDocumentService blankExtractFromUPUDocumentService,
           ILoggerService loggerService,
           IPdfService pdfService, UserManager<User> userManager)
        {
            _blankBiographyDocumentService = blankBiographyDocumentService;
            _blankAchievementDocumentService = blankAchievementDocumentService;
            _blankExtractFromUPUDocumentService = blankExtractFromUPUDocumentService;
            _pdfService = pdfService;
            _loggerService = loggerService;
            _userManager = userManager;
        }

        private async Task<bool> HasAccessSupporterAndRegisteredAsync(string userId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(currentUser);
            if (currentUser.Id == userId)
                return true;
            var role = roles.FirstOrDefault(x => Roles.HeadsAndHeadDeputiesAndAdminAndPlastun.Contains(x));
            if(role != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Add a document to the biography blank
        /// </summary>
        /// <param name="biographyDocument">An information about a specific document</param>
        /// <returns>A newly created Blank biography document</returns>
        [HttpPost("AddDocument/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddBiographyDocument(BlankBiographyDocumentsDto biographyDocument)
        {
            await _blankBiographyDocumentService.AddDocumentAsync(biographyDocument);

            return Created("", biographyDocument);
        }

        /// <summary>
        /// Add a document to the achievement blank
        /// </summary>
        /// <param name="achievementDocuments">An information about a specific document</param>
        /// <returns>A newly created Blank achievement document</returns>
        [HttpPost("AddAchievementDocumet/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddAchievementDocument(IEnumerable<AchievementDocumentsDto> achievementDocuments)
        {
            await _blankAchievementDocumentService.AddDocumentAsync(achievementDocuments);

            return Created("AchievementDocument", achievementDocuments);
        }

        /// <summary>
        /// Add a document to the ExtractFromUPU blank
        /// </summary>
        /// <param name="extractFromUPUDocumentsDTO">An information about a specific document</param>
        /// <returns>A newly created Blank biography document</returns>
        [HttpPost("AddExtractFromUPUDocument/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddExtractFromUPUDocument(ExtractFromUpuDocumentsDto extractFromUPUDocumentsDTO)
        {
            await _blankExtractFromUPUDocumentService.AddDocumentAsync(extractFromUPUDocumentsDTO);

            return Created("", extractFromUPUDocumentsDTO);
        }

        /// <summary>
        /// Get a document by User ID
        /// </summary>
        /// <param Id="userId">An Id of user</param>
        /// <returns>A blank document that attached to user</returns>
        [HttpGet("GetDocumentByUserId/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetDocumentByUserId(string userId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if(await HasAccessSupporterAndRegisteredAsync(userId))
                return Ok(await _blankBiographyDocumentService.GetDocumentByUserId(userId));
            _loggerService.LogError($"User (id: {currentUser.Id}) hasn't access to document (id: {userId})");
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        /// <summary>
        /// Get a document by User ID
        /// </summary>
        /// <param Id="userId">An Id of user</param>
        /// <returns>A blank document that attached to user</returns>
        [HttpGet("GetExtractFromUPUDocumentByUserId/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetExtractFromUPUByUserId(string userId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (await HasAccessSupporterAndRegisteredAsync(userId))
                return Ok(await _blankExtractFromUPUDocumentService.GetDocumentByUserId(userId));
            _loggerService.LogError($"User (id: {currentUser.Id}) hasn't access to ExtractFromUPUDocument (id: {userId})");
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        [HttpGet("InfinityScroll")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetPartOfAchievement(int pageNumber, int pageSize, string userId)
        {
            return Ok(await _blankAchievementDocumentService.GetPartOfAchievementAsync(pageNumber, pageSize, userId));
        }

        /// <summary>
        /// Get a list of achievement documents by User ID
        /// </summary>
        /// <param Id="userId">An Id of user</param>
        /// <returns>A achievement documents that attached to user</returns>
        [HttpGet("GetAchievementDocumentsByUserId/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAchievementDocumentsByUserId(string userId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (await HasAccessSupporterAndRegisteredAsync(userId))
                return Ok(await _blankAchievementDocumentService.GetDocumentsByUserIdAsync(userId));
            _loggerService.LogError($"User (id: {currentUser.Id}) hasn't access to achievement documents (id: {userId})");
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        /// <summary>
        /// Delete the document by document ID
        /// </summary>
        /// <param Id="documentId">An Id of document</param>
        [HttpDelete("RemoveBiographyDocument/{documentId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> RemoveDocument(int documentId)
        {
            await _blankBiographyDocumentService.DeleteFileAsync(documentId);

            return NoContent();
        }

        /// <summary>
        /// Delete the achievement document by document ID
        /// </summary>
        /// <param Id="documentId">An Id of document</param>
        [HttpDelete("RemoveAchievementDocument/{documentId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> RemoveAchievementDocument(int documentId)
        {
            await _blankAchievementDocumentService.DeleteFileAsync(documentId);

            return NoContent();
        }

        /// <summary>
        /// Delete the Extract From UPU document by document ID
        /// </summary>
        /// <param Id="documentId">An Id of document</param>
        [HttpDelete("RemoveExtractFromUPUDocument/{documentId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> RemoveExtractFromUPUDocument(int documentId)
        {
            await _blankExtractFromUPUDocumentService.DeleteFileAsync(documentId);

            return NoContent();
        }

        /// <summary>
        /// Download  document by file name
        /// </summary>
        /// <param name="fileName">A file blob name</param>
        /// <returns>A Base64 format of biography document</returns>
        [HttpGet("BiographyDocumentBase64/{fileName}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetFileBase64(string fileName)
        {
            return Ok(await _blankBiographyDocumentService.DownloadFileAsync(fileName));
        }

        /// <summary>
        /// Download  document by file name
        /// </summary>
        /// <param name="fileName">A file blob name</param>
        /// <returns>A Base64 format of Achievement document</returns>
        [HttpGet("AchievementDocumentBase64/{fileName}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetFileAchievementBase64(string fileName)
        {
            return Ok(await _blankAchievementDocumentService.DownloadFileAsync(fileName));
        }

        /// <summary>
        /// Download  document by file name
        /// </summary>
        /// <param name="fileName">A file blob name</param>
        /// <returns>A Base64 format of Extract From UPU document</returns>
        [HttpGet("ExtractFromUPUDocumentBase64/{fileName}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetFileExtractFromUPUBase64(string fileName)
        {
            return Ok(await _blankExtractFromUPUDocumentService.DownloadFileAsync(fileName));
        }

        /// <summary>
        /// Generate user document by user Id
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>A Base64 format Generated document</returns>
        [HttpGet("getGenerationFile/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetGenerationFile(string userId)
        {
            return Ok(await _pdfService.BlankCreatePDFAsync(userId));
        }

    }
}
