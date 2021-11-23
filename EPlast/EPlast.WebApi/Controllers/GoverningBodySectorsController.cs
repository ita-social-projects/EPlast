using AutoMapper;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.DTO.GoverningBody.Sector;
using EPlast.BLL.Interfaces.GoverningBodies.Sector;
using EPlast.Resources;
using EPlast.WebApi.Models.GoverningBody.Sector;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [Route("api/GoverningBodies/Sectors")]
    [ApiController]
    public class GoverningBodySectorsController : ControllerBase
    {
        private readonly ISectorService _sectorService;
        private readonly ISectorAdministrationService _sectorAdministrationService;
        private readonly ISectorDocumentsService _sectorDocumentsService;
        private readonly ILoggerService<GoverningBodiesController> _logger;
        private readonly IMapper _mapper;

        public GoverningBodySectorsController(ISectorService service,
                                                ILoggerService<GoverningBodiesController> logger,
                                                ISectorAdministrationService governingBodyAdministrationService,
                                                IMapper mapper,
                                                ISectorDocumentsService governingBodyDocumentsService)
        {
            _sectorService = service;
            _logger = logger;
            _sectorAdministrationService = governingBodyAdministrationService;
            _mapper = mapper;
            _sectorDocumentsService = governingBodyDocumentsService;
        }

        [HttpGet("{governingBodyId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetSectors(int governingBodyId)
        {
            return Ok(await _sectorService.GetSectorsByGoverningBodyAsync(governingBodyId));
        }

        [HttpPost("CreateSector")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndGBHeadAndGBSectorHead)]
        public async Task<IActionResult> Create(SectorDTO sectorDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                sectorDTO.Id = await _sectorService.CreateAsync(sectorDTO);
            }
            catch
            {
                return BadRequest();
            }
            _logger.LogInformation($"Governing body sector {{{sectorDTO.Name}}} was created.");

            return Ok(sectorDTO.Id);
        }

        [HttpPut("EditSector/{sectorId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndGBHeadAndGBSectorHead)]
        public async Task<IActionResult> Edit(SectorDTO sector)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _sectorService.EditAsync(sector);
            _logger.LogInformation($"Governing body sector {{{sector.Name}}} was edited.");

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

            return Ok(await _sectorService.GetLogoBase64Async(logoName));
        }

        [HttpGet("Profile/{sectorId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetProfile(int sectorId)
        {
            var sectorProfileDto = await _sectorService.GetSectorProfileAsync(sectorId);
            if (sectorProfileDto == null)
            {
                return NotFound();
            }

            var sectorViewModel = _mapper.Map<SectorProfileDTO, SectorViewModel>(sectorProfileDto);
            return Ok(new { sectorViewModel, documentsCount = sectorProfileDto.Sector.Documents.Count() });
        }

        [HttpDelete("RemoveSector/{sectorId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndGBHeadAndGBSectorHead)]
        public async Task<IActionResult> Remove(int sectorId)
        {
            await _sectorService.RemoveAsync(sectorId);
            _logger.LogInformation($"Governing body sector with id {{{sectorId}}} was deleted.");

            return Ok();
        }

        [HttpGet("Admins/{sectorId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAdmins(int sectorId)
        {
            var sectorProfileDto = await _sectorService.GetSectorProfileAsync(sectorId);
            if (sectorProfileDto == null)
            {
                return NotFound();
            }

            var sectorBodyViewModel = _mapper.Map<SectorProfileDTO, SectorViewModel>(sectorProfileDto);

            return Ok(new { Admins = sectorBodyViewModel.Administration, sectorBodyViewModel.Head, sectorBodyViewModel.Name });
        }

        [HttpPost("AddAdmin/{sectorId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndGBHeadAndGBSectorHead)]
        public async Task<IActionResult> AddAdmin(SectorAdministrationDTO newAdmin)
        {
            try
            {
                await _sectorAdministrationService.AddSectorAdministratorAsync(newAdmin);
            }
            catch
            {
                return BadRequest();
            }
            _logger.LogInformation($"User {{{newAdmin.UserId}}} became Admin for governing body sector {{{newAdmin.SectorId}}}" +
                                   $" with role {{{newAdmin.AdminType.AdminTypeName}}}.");

            return Ok(newAdmin);
        }

        [HttpPut("EditAdmin/{adminId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndGBHeadAndGBSectorHead)]
        public async Task<IActionResult> EditAdmin(SectorAdministrationDTO adminDto)
        {
            await _sectorAdministrationService.EditSectorAdministratorAsync(adminDto);
            _logger.LogInformation($"Admin with User-ID {{{adminDto.UserId}}} was edited.");

            return Ok(adminDto);
        }

        [HttpPut("RemoveAdmin/{adminId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndGBHeadAndGBSectorHead)]
        public async Task<IActionResult> RemoveAdmin(int adminId)
        {
            await _sectorAdministrationService.RemoveAdministratorAsync(adminId);
            _logger.LogInformation($"Admin with ID {{{adminId}}} was removed.");

            return Ok();
        }

        [HttpGet("Documents/{sectorId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminPlastMemberAndSupporter)]
        public async Task<IActionResult> GetDocuments(int sectorId)
        {
            var sectorProfileDto = await _sectorService.GetSectorDocumentsAsync(sectorId);
            if (sectorProfileDto == null)
            {
                return NotFound();
            }

            var sectorViewModel = _mapper.Map<SectorProfileDTO, SectorViewModel>(sectorProfileDto);

            return Ok(new { sectorViewModel.Documents });
        }

        [HttpPost("AddDocument/{sectorId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndGBHeadAndGBSectorHead)]
        public async Task<IActionResult> AddDocument(SectorDocumentsDTO document)
        {
            await _sectorDocumentsService.AddSectorDocumentAsync(document);
            _logger.LogInformation($"Document with id {{{document.Id}}} was added.");

            return Ok(document);
        }

        [HttpGet("FileBase64/{fileName}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminPlastMemberAndSupporter)]
        public async Task<IActionResult> GetFileBase64(string fileName)
        {
            var fileBase64 = await _sectorDocumentsService.DownloadSectorDocumentAsync(fileName);
            return Ok(fileBase64);
        }

        [HttpDelete("RemoveDocument/{documentId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndGBHeadAndGBSectorHead)]
        public async Task<IActionResult> RemoveDocument(int documentId)
        {
            await _sectorDocumentsService.DeleteSectorDocumentAsync(documentId);
            _logger.LogInformation($"Document with id {{{documentId}}} was deleted.");

            return Ok();
        }

        [HttpGet("GetDocumentTypes")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetDocumentTypesAsync()
        {
            var documentTypes = await _sectorDocumentsService.GetAllSectorDocumentTypesAsync();
            return Ok(documentTypes);
        }

        [HttpGet("GetUserAccesses/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetUserAccess(string userId)
        {
            return Ok(await _sectorService.GetUserAccessAsync(userId));
        }

        [HttpGet("GetUserAdmins/{UserId}")]
        public async Task<IActionResult> GetUserAdministrations(string UserId)
        {
            var userAdmins = await _sectorService.GetAdministrationsOfUserAsync(UserId);

            return Ok(userAdmins);
        }

        [HttpGet("GetUserPreviousAdmins/{UserId}")]
        public async Task<IActionResult> GetUserPreviousAdministrations(string UserId)
        {
            var userAdmins = await _sectorService.GetPreviousAdministrationsOfUserAsync(UserId);

            return Ok(userAdmins);
        }
    }
}