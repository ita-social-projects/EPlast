using AutoMapper;
using EPlast.BLL.DTO;
using EPlast.BLL.DTO.GoverningBody;
using EPlast.BLL.DTO.GoverningBody.Announcement;
using EPlast.BLL.Interfaces.GoverningBodies;
using EPlast.BLL.Interfaces.Logging;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using EPlast.WebApi.Models.GoverningBody;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoverningBodiesController : ControllerBase
    {
        private readonly IGoverningBodiesService _governingBodiesService;
        private readonly IGoverningBodyAdministrationService _governingBodyAdministrationService;
        private readonly IGoverningBodyDocumentsService _governingBodyDocumentsService;
        private readonly ILoggerService<GoverningBodiesController> _logger;
        private readonly IMapper _mapper;
        private readonly IGoverningBodyAnnouncementService _governingBodyAnnouncementService;

        public GoverningBodiesController(IGoverningBodiesService service,
            ILoggerService<GoverningBodiesController> logger,
            IGoverningBodyAdministrationService governingBodyAdministrationService,
            IGoverningBodyAnnouncementService governingBodyAnnouncementService,
            IMapper mapper,
            IGoverningBodyDocumentsService governingBodyDocumentsService)
        {
            _governingBodiesService = service;
            _logger = logger;
            _governingBodyAnnouncementService = governingBodyAnnouncementService;
            _governingBodyAdministrationService = governingBodyAdministrationService;
            _mapper = mapper;
            _governingBodyDocumentsService = governingBodyDocumentsService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetGoverningBodies()
        {
            return Ok(await _governingBodiesService.GetGoverningBodiesListAsync());
        }

        [HttpPost("CreateGoverningBody")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndGBHead)]
        public async Task<IActionResult> Create(GoverningBodyDTO governingBodyDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                governingBodyDTO.Id = await _governingBodiesService.CreateAsync(governingBodyDTO);
            }
            catch
            {
                return BadRequest();
            }

            _logger.LogInformation($"Governing body {{{governingBodyDTO.GoverningBodyName}}} was created.");

            return Ok(governingBodyDTO.Id);
        }

        [HttpPut("EditGoverningBody/{governingBodyId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndGBHead)]
        public async Task<IActionResult> Edit(GoverningBodyDTO governingBody)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _governingBodiesService.EditAsync(governingBody);
            _logger.LogInformation($"Governing body {{{governingBody.GoverningBodyName}}} was edited.");

            return Ok();
        }

        [HttpGet("LogoBase64/{logoName}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetPhotoBase64(string logoName)
        {
            if (logoName == null)
            {
                return BadRequest(logoName);
            }

            return Ok(await _governingBodiesService.GetLogoBase64Async(logoName));
        }

        [HttpGet("Profile/{governingBodyId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetProfile(int governingBodyId)
        {
            var governingBodyProfileDto = await _governingBodiesService.GetGoverningBodyProfileAsync(governingBodyId);
            if (governingBodyProfileDto == null)
            {
                return NotFound();
            }

            var governingBodyViewModel = _mapper.Map<GoverningBodyProfileDTO, GoverningBodyViewModel>(governingBodyProfileDto);

            return Ok(new { governingBodyViewModel, documentsCount = governingBodyProfileDto.GoverningBody.GoverningBodyDocuments.Count() });
        }

        [HttpDelete("RemoveGoverningBody/{governingBodyId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndGBHead)]
        public async Task<IActionResult> Remove(int governingBodyId)
        {
            await _governingBodiesService.RemoveAsync(governingBodyId);
            _logger.LogInformation($"GoverningBody with id {{{governingBodyId}}} was deleted.");

            return Ok();
        }

        /// <summary>
        /// Get all administrators of a specific Governing Body
        /// </summary>
        /// <param name="governingBodyId">The id of the Governing Body</param>
        /// <returns>All administrators of a specific Governing Body</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Governing Body not found</response>
        [HttpGet("Admins/{governingBodyId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAdmins(int governingBodyId)
        {
            var governingBodyProfileDto = await _governingBodiesService.GetGoverningBodyProfileAsync(governingBodyId);
            if (governingBodyProfileDto == null)
            {
                return NotFound();
            }

            var governingBodyViewModel = _mapper.Map<GoverningBodyProfileDTO, GoverningBodyViewModel>(governingBodyProfileDto);

            return Ok(new { Admins = governingBodyViewModel.Administration, governingBodyViewModel.Head, governingBodyViewModel.GoverningBodyName });
        }

        /// <summary>
        /// Add a new administrator to the Governing Body
        /// </summary>
        /// <param name="newAdmin">An information about a new administrator</param>
        /// <returns>An information about a new administrator</returns>
        [HttpPost("AddAdmin/{governingBodyId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndGBHead)]
        public async Task<IActionResult> AddAdmin(GoverningBodyAdministrationDTO newAdmin)
        {
            try
            {
                await _governingBodyAdministrationService.AddGoverningBodyAdministratorAsync(newAdmin);
            }
            catch
            {
                return BadRequest();
            }
            _logger.LogInformation($"User {{{newAdmin.UserId}}} became Admin for Governing Body {{{newAdmin.GoverningBodyId}}}" +
                                   $" with role {{{newAdmin.AdminType.AdminTypeName}}}.");

            return Ok(newAdmin);
        }

        /// <summary>
        /// Edit an information about a specific administrator
        /// </summary>
        /// <param name="adminDto">An information about a new administrator</param>
        /// <returns>An information about a specific administrator</returns>
        [HttpPut("EditAdmin/{adminId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndGBHead)]
        public async Task<IActionResult> EditAdmin(GoverningBodyAdministrationDTO adminDto)
        {
            await _governingBodyAdministrationService.EditGoverningBodyAdministratorAsync(adminDto);
            _logger.LogInformation($"Admin with User-ID {{{adminDto.UserId}}} was edited.");

            return Ok(adminDto);
        }

        /// <summary>
        /// Remove a specific administrator from the Governing Body
        /// </summary>
        /// <param name="adminId">The id of the administrator</param>
        [HttpPut("RemoveAdmin/{adminId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndGBHead)]
        public async Task<IActionResult> RemoveAdmin(int adminId)
        {
            await _governingBodyAdministrationService.RemoveAdministratorAsync(adminId);
            _logger.LogInformation($"Admin with ID {{{adminId}}} was removed.");

            return Ok();
        }

        /// <summary>
        /// Get all documents of a specific Governing Body
        /// </summary>
        /// <param name="governingBodyId">The id of the Governing Body</param>
        /// <returns>All documents of a specific Governing Body</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Governing Body not found</response>
        [HttpGet("Documents/{governingBodyId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetDocuments(int governingBodyId)
        {
            var governingBodyProfileDto = await _governingBodiesService.GetGoverningBodyDocumentsAsync(governingBodyId);
            if (governingBodyProfileDto == null)
            {
                return NotFound();
            }

            var governingBodyProfile = _mapper.Map<GoverningBodyProfileDTO, GoverningBodyViewModel>(governingBodyProfileDto);

            return Ok(new { governingBodyProfile.Documents });
        }

        /// <summary>
        /// Add a document to the Governing Body
        /// </summary>
        /// <param name="document">An information about a specific document</param>
        /// <returns>A newly created document</returns>
        [HttpPost("AddDocument/{governingBodyId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndGBHead)]
        public async Task<IActionResult> AddDocument(GoverningBodyDocumentsDTO document)
        {
            await _governingBodyDocumentsService.AddGoverningBodyDocumentAsync(document);
            _logger.LogInformation($"Document with id {{{document.Id}}} was added.");

            return Ok(document);
        }

        /// <summary>
        /// Get a file in base64 format
        /// </summary>
        /// <param name="fileName">The name of a Governing Body document</param>
        /// <returns>A base64 string of the file</returns>
        [HttpGet("FileBase64/{fileName}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminPlastMemberAndSupporter)]
        public async Task<IActionResult> GetFileBase64(string fileName)
        {
            var fileBase64 = await _governingBodyDocumentsService.DownloadGoverningBodyDocumentAsync(fileName);

            return Ok(fileBase64);
        }

        /// <summary>
        /// Remove a specific document
        /// </summary>
        /// <param name="documentId">The id of a specific document</param>
        [HttpDelete("RemoveDocument/{documentId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndGBHead)]
        public async Task<IActionResult> RemoveDocument(int documentId)
        {
            await _governingBodyDocumentsService.DeleteGoverningBodyDocumentAsync(documentId);
            _logger.LogInformation($"Document with id {{{documentId}}} was deleted.");

            return Ok();
        }

        /// <summary>
        /// Get document types
        /// </summary>
        [HttpGet("GetDocumentTypes")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetDocumentTypesAsync()
        {
            var documentTypes = await _governingBodyDocumentsService.GetAllGoverningBodyDocumentTypesAsync();

            return Ok(documentTypes);
        }

        [HttpGet("GetUserAccesses/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetUserAccess(string userId)
        {
            return Ok(await _governingBodiesService.GetUserAccessAsync(userId));
        }

        [HttpGet("GetUserAdmins/{UserId}")]
        public async Task<IActionResult> GetUserAdministrations(string UserId)
        {
            var userAdmins = await _governingBodiesService.GetAdministrationsOfUserAsync(UserId);

            return Ok(userAdmins);
        }

        [HttpGet("GetUserPreviousAdmins/{UserId}")]
        public async Task<IActionResult> GetUserPreviousAdministrations(string UserId)
        {
            var userAdmins = await _governingBodiesService.GetPreviousAdministrationsOfUserAsync(UserId);

            return Ok(userAdmins);
        }

        [HttpPost("AddAnnouncement/{text}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndGBHead)]
        public async Task<IActionResult> AddAnnouncement(string text)
        {
            if (ModelState.IsValid)
            {
                await _governingBodyAnnouncementService.AddAnnouncementAsync(text);

                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpPut("EditAnnouncement/{id:int}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndGBHead)]
        public async Task<IActionResult> EditAnnouncement(GoverningBodyAnnouncementUserDTO announcement)
        {
            if (ModelState.IsValid)
            {
                await _governingBodyAnnouncementService.EditAnnouncement(announcement);
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("DeleteAnnouncement/{id:int}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndGBHead)]
        public async Task<IActionResult> Delete(int id)
        {
            await _governingBodyAnnouncementService.DeleteAnnouncementAsync(id);

            return NoContent();
        }

        [HttpGet("GetAnnouncement/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            GoverningBodyAnnouncementUserDTO governingBodyAnnouncementUserDTO = await _governingBodyAnnouncementService.GetAnnouncementByIdAsync(id);

            if (governingBodyAnnouncementUserDTO == null)
            {
                return NotFound();
            }
            return Ok(governingBodyAnnouncementUserDTO);
        }

        [HttpGet("GetAllAnnouncements")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminPlastMemberAndSupporter)]
        public async Task<IActionResult> GetAllAnnouncement()
        {
            var announcements = await _governingBodyAnnouncementService.GetAllAnnouncementAsync();

            return Ok(announcements);
        }

        [HttpGet("GetAllUsersId")]
        public async Task<IActionResult> GetAllUserId()
        {
            var users = await _governingBodyAnnouncementService.GetAllUserAsync();

            return Ok(users);
        }

    }
}
