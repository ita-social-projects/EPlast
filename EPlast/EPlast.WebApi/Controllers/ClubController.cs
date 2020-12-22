using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.Logging;
using EPlast.WebApi.Models.Club;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubController : ControllerBase
    {
        private readonly ILoggerService<ClubController> _logger;
        private readonly IMapper _mapper;
        private readonly IClubService _ClubService;
        private readonly IClubParticipantsService _ClubParticipantsService;
        private readonly IClubDocumentsService _ClubDocumentsService;
        private readonly IClubAccessService _ClubAccessService;
        private readonly IClubAnnualReportService _ClubAnnualReportService;
        private readonly UserManager<User> _userManager;

        public ClubController(ILoggerService<ClubController> logger,
            IMapper mapper,
            IClubService ClubService,
            IClubParticipantsService ClubParticipantsService,
            IClubDocumentsService ClubDocumentsService,
            IClubAccessService ClubAccessService,
            IClubAnnualReportService ClubAnnualReportService, UserManager<User> userManager)
        {
            _logger = logger;
            _mapper = mapper;
            _ClubService = ClubService;
            _ClubParticipantsService = ClubParticipantsService;
            _ClubDocumentsService = ClubDocumentsService;
            _ClubAccessService = ClubAccessService;
            _ClubAnnualReportService = ClubAnnualReportService;
            _userManager = userManager;
        }

        /// <summary>
        /// Get a specific number of Clubs 
        /// </summary>
        /// <param name="page">A number of the page</param>
        /// <param name="pageSize">A count of Clubs to display</param>
        /// <param name="ClubName">Optional param to find Clubs by name</param>
        /// <returns>A specific number of Clubs</returns>
        [HttpGet("Profiles/{page}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetClubs(int page, int pageSize, string ClubName = null)
        {
            var clubs = await _ClubService.GetAllDTOAsync(ClubName);
            var ClubsViewModel = new ClubsViewModel(page, pageSize, clubs, User.IsInRole("Admin"));

            return Ok(ClubsViewModel);
        }

        /// <summary>
        /// Get all clubs 
        /// </summary>
        /// <returns>List of clubs</returns>
        [HttpGet("Clubs")]
        public async Task<IActionResult> GetClubs()
        {
            var cities = await _ClubService.GetClubs();
            return Ok(cities);

        }

        /// <summary>
        /// Get a specific Club
        /// </summary>
        /// <param name="ClubId">The id of the Club</param>
        /// <returns>A specific Club</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Club not found</response>
        [HttpGet("Profile/{ClubId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetProfile(int ClubId)
        {
            var ClubProfileDto = await _ClubService.GetClubProfileAsync(ClubId, await _userManager.GetUserAsync(User));
            if (ClubProfileDto == null)
            {
                return NotFound();
            }

            var ClubProfile = _mapper.Map<ClubProfileDTO, ClubViewModel>(ClubProfileDto);

            return Ok(ClubProfile);
        }

        /// <summary>
        /// Get all members of a specific Club
        /// </summary>
        /// <param name="ClubId">The id of the Club</param>
        /// <returns>All members of a specific Club</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Club not found</response>
        [HttpGet("Members/{ClubId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetMembers(int ClubId)
        {
            var ClubProfileDto = await _ClubService.GetClubMembersAsync(ClubId);
            if (ClubProfileDto == null)
            {
                return NotFound();
            }

            var ClubProfile = _mapper.Map<ClubProfileDTO, ClubViewModel>(ClubProfileDto);
            ClubProfile.CanEdit = await _ClubAccessService.HasAccessAsync(await _userManager.GetUserAsync(User), ClubId);

            return Ok(new { ClubProfile.Members, ClubProfile.CanEdit, ClubProfile.Name });
        }

        /// <summary>
        /// Get all followers of a specific Club
        /// </summary>
        /// <param name="ClubId">The id of the Club</param>
        /// <returns>All followers of a specific Club</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Club not found</response>
        [HttpGet("Followers/{ClubId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetFollowers(int ClubId)
        {
            var ClubProfileDto = await _ClubService.GetClubFollowersAsync(ClubId);
            if (ClubProfileDto == null)
            {
                return NotFound();
            }

            var ClubProfile = _mapper.Map<ClubProfileDTO, ClubViewModel>(ClubProfileDto);
            ClubProfile.CanEdit = await _ClubAccessService.HasAccessAsync(await _userManager.GetUserAsync(User), ClubId);

            return Ok(new { ClubProfile.Followers, ClubProfile.CanEdit, ClubProfile.Name });
        }

        /// <summary>
        /// Get all administrators of a specific Club
        /// </summary>
        /// <param name="ClubId">The id of the Club</param>
        /// <returns>All administrators of a specific Club</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Club not found</response>
        [HttpGet("Admins/{ClubId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAdmins(int ClubId)
        {
            var ClubProfileDto = await _ClubService.GetClubAdminsAsync(ClubId);
            if (ClubProfileDto == null)
            {
                return NotFound();
            }

            var ClubProfile = _mapper.Map<ClubProfileDTO, ClubViewModel>(ClubProfileDto);
            ClubProfile.CanEdit = await _ClubAccessService.HasAccessAsync(await _userManager.GetUserAsync(User), ClubId);

            return Ok(new { ClubProfile.Administration, ClubProfile.Head, ClubProfile.CanEdit, ClubProfile.Name });
        }

        /// <summary>
        /// Get all documents of a specific Club
        /// </summary>
        /// <param name="ClubId">The id of the Club</param>
        /// <returns>All documents of a specific Club</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Club not found</response>
        [HttpGet("Documents/{ClubId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetDocuments(int ClubId)
        {
            var ClubProfileDto = await _ClubService.GetClubDocumentsAsync(ClubId);
            if (ClubProfileDto == null)
            {
                return NotFound();
            }

            var ClubProfile = _mapper.Map<ClubProfileDTO, ClubViewModel>(ClubProfileDto);
            ClubProfile.CanEdit = await _ClubAccessService.HasAccessAsync(await _userManager.GetUserAsync(User), ClubId);

            return Ok(new { ClubProfile.Documents, ClubProfile.CanEdit });
        }

        /// <summary>
        /// Get an information about a specific Club
        /// </summary>
        /// <param name="ClubId">The id of the Club</param>
        /// <returns>An information about a specific Club</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Club not found</response>
        [HttpGet("Details/{ClubId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Details(int ClubId)
        {
            var ClubDto = await _ClubService.GetClubProfileAsync(ClubId);
            if (ClubDto == null)
            {
                return NotFound();
            }

            return Ok(ClubDto);
        }

        /// <summary>
        /// Get a photo in base64 format
        /// </summary>
        /// <param name="logoName">The name of a Club logo</param>
        /// <returns>A base64 string of the Club logo</returns>
        [HttpGet("LogoBase64")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetPhotoBase64(string logoName)
        {
            var logoBase64 = await _ClubService.GetLogoBase64(logoName);

            return Ok(logoBase64);
        }

        /// <summary>
        /// Create a new Club
        /// </summary>
        /// <param name="Club">An information about a new Club</param>
        /// <returns>An id of a new Club</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="400">Wrong input</response>
        [HttpPost("CreateClub")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Create(ClubViewModel Club)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var ClubDTO = _mapper.Map<ClubViewModel, ClubDTO>(Club);
                    ClubDTO.ID = await _ClubService.CreateAsync(ClubDTO);
                    _logger.LogInformation($"Club {{{ClubDTO.Name}}} was created.");
                    return Ok(ClubDTO.ID);
                }
                catch (InvalidOperationException)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Edit a specific Club
        /// </summary>
        /// <param name="Club">An information about an edited Club</param>
        /// <response code="200">Successful operation</response>
        /// <response code="400">Wrong input</response>
        [HttpPut("EditClub/{ClubId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Edit(ClubViewModel Club)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ClubDTO = _mapper.Map<ClubViewModel, ClubDTO>(Club);

            await _ClubService.EditAsync(ClubDTO);
            _logger.LogInformation($"Club {{{ClubDTO.Name}}} was edited.");

            return Ok();
        }

        /// <summary>
        /// Remove a specific Club
        /// </summary>
        /// <param name="ClubId">The id of the Club</param>
        [HttpDelete("RemoveClub/{ClubId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Remove(int ClubId)
        {
            await _ClubService.RemoveAsync(ClubId);
            _logger.LogInformation($"Club with id {{{ClubId}}} was deleted.");

            return Ok();
        }

        /// <summary>
        /// Add a current user to followers
        /// </summary>
        /// <param name="ClubId">An id of the Club</param>
        /// <returns>An information about a new follower</returns>
        [HttpPost("AddFollower/{ClubId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddFollower(int ClubId)
        {
            var follower = await _ClubParticipantsService.AddFollowerAsync(ClubId, await _userManager.GetUserAsync(User));
            _logger.LogInformation($"User {{{follower.UserId}}} became a follower of Club {{{ClubId}}}.");

            return Ok(follower);
        }

        /// <summary>
        /// Add the user to followers
        /// </summary>
        /// <param name="clubId">An id of the city</param>
        /// <param name="userId">An id of the user</param>
        /// <returns>An information about a new follower</returns>
        [HttpPost("AddFollowerWithId/{clubId}/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddFollowerWithId(int clubId, string userId)
        {
            var follower = await _ClubParticipantsService.AddFollowerAsync(clubId, userId);
            _logger.LogInformation($"User {{{follower.UserId}}} became a follower of city {{{clubId}}}.");

            return Ok(follower);
        }

        /// <summary>
        /// Remove a specific follower from the Club
        /// </summary>
        /// <param name="followerId">The id of the follower</param>
        [HttpDelete("RemoveFollower/{followerId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> RemoveFollower(int followerId)
        {
            await _ClubParticipantsService.RemoveFollowerAsync(followerId);
            _logger.LogInformation($"Follower with ID {{{followerId}}} was removed.");

            return Ok();
        }

        /// <summary>
        /// Toggle an approve status for member
        /// </summary>
        /// <param name="memberId">The id of the member</param>
        /// <returns>An information about a specific member</returns>
        [HttpPut("ChangeApproveStatus/{memberId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> ChangeApproveStatus(int memberId)
        {
            var member = await _ClubParticipantsService.ToggleApproveStatusAsync(memberId);
            _logger.LogInformation($"Status of member with ID {{{memberId}}} was changed.");

            return Ok(member);
        }

        /// <summary>
        /// Add a new administrator to the Club
        /// </summary>
        /// <param name="newAdmin">An information about a new administrator</param>
        /// <returns>An information about a new administrator</returns>
        [HttpPost("AddAdmin/{ClubId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddAdmin(ClubAdministrationViewModel newAdmin)
        {
            var admin = _mapper.Map<ClubAdministrationViewModel, ClubAdministrationDTO>(newAdmin);
            await _ClubParticipantsService.AddAdministratorAsync(admin);

            _logger.LogInformation($"User {{{admin.UserId}}} became admin for Club {{{admin.ClubId}}}" +
                $" with role {{{admin.AdminType.AdminTypeName}}}.");

            return Ok(admin);
        }

        /// <summary>
        /// Remove a specific administrator from the Club
        /// </summary>
        /// <param name="adminId">The id of the administrator</param>
        [HttpPut("RemoveAdmin/{adminId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> RemoveAdmin(int adminId)
        {
            await _ClubParticipantsService.RemoveAdministratorAsync(adminId);
            _logger.LogInformation($"Admin with ID {{{adminId}}} was removed.");

            return Ok();
        }

        /// <summary>
        /// Edit an information about a specific admininstrator
        /// </summary>
        /// <param name="admin">An information about a new administrator</param>
        /// <returns>An information about a specific admininstrator</returns>
        [HttpPut("EditAdmin/{adminId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> EditAdmin(ClubAdministrationViewModel admin)
        {
            var adminDTO = _mapper.Map<ClubAdministrationViewModel, ClubAdministrationDTO>(admin);

            await _ClubParticipantsService.EditAdministratorAsync(adminDTO);
            _logger.LogInformation($"Admin with User-ID {{{admin.UserId}}} was edited.");

            return Ok(adminDTO);
        }

        /// <summary>
        /// Add a document to the Club
        /// </summary>
        /// <param name="document">An information about a specific document</param>
        /// <returns>A newly created document</returns>
        [HttpPost("AddDocument/{ClubId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddDocument(ClubDocumentsViewModel document)
        {
            var documentDTO = _mapper.Map<ClubDocumentsViewModel, ClubDocumentsDTO>(document);

            await _ClubDocumentsService.AddDocumentAsync(documentDTO);
            _logger.LogInformation($"Document with id {{{documentDTO.ID}}} was added.");

            return Ok(documentDTO);
        }

        /// <summary>
        /// Get a file in base64 format
        /// </summary>
        /// <param name="fileName">The name of a Club file</param>
        /// <returns>A base64 string of the file</returns>
        [HttpGet("FileBase64/{fileName}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetFileBase64(string fileName)
        {
            var fileBase64 = await _ClubDocumentsService.DownloadFileAsync(fileName);

            return Ok(fileBase64);
        }

        /// <summary>
        /// Remove a specific document
        /// </summary>
        /// <param name="documentId">The id of a specific document</param>
        [HttpDelete("RemoveDocument/{documentId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> RemoveDocument(int documentId)
        {
            await _ClubDocumentsService.DeleteFileAsync(documentId);
            _logger.LogInformation($"Document with id {{{documentId}}} was deleted.");

            return Ok();
        }

        [HttpGet("GetDocumentTypes")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetDocumentTypesAsync()
        {
            var documentTypes = await _ClubDocumentsService.GetAllClubDocumentTypesAsync();

            return Ok(documentTypes);
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetClubsThatUserHasAccessTo()
        {
            return Ok(new { Clubs = await _ClubAccessService.GetClubsAsync(await _userManager.GetUserAsync(User)) });
        }



        [HttpGet("GetUserAdmins/{UserId}")]

        public async Task<IActionResult> GetUserAdministrations(string UserId)
        {
            var userAdmins = await _ClubParticipantsService.GetAdministrationsOfUserAsync(UserId);

            return Ok(userAdmins);
        }


        [HttpGet("GetUserPreviousAdmins/{UserId}")]

        public async Task<IActionResult> GetUserPreviousAdministrations(string UserId)
        {
            var userAdmins = await _ClubParticipantsService.GetPreviousAdministrationsOfUserAsync(UserId);

            return Ok(userAdmins);
        }

        [HttpGet("GetAllAdministrationStatuses/{UserId}")]
        public async Task<IActionResult> GetAllAdministrationStatuses(string UserId)
        {
            var userAdmins = await _ClubParticipantsService.GetAdministrationStatuses(UserId);

            return Ok(userAdmins);
        }

        /// <summary>
        /// Method to get all club reports that the user has access to
        /// </summary>
        /// <returns>List of annual reports</returns>
        /// <response code="200">Successful operation</response>

        [HttpGet("GetAllClubAnnualReports")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Голова Округу, Голова Станиці, Голова Куреня")]
        public async Task<IActionResult> GetAllClubAnnualReports()
        {
            return StatusCode(StatusCodes.Status200OK, new { clubAnnualReports = await _ClubAnnualReportService.GetAllAsync(await _userManager.GetUserAsync(User)) });
        }

        /// <summary>
        /// Method to get club annual report
        /// </summary>
        /// <param name="id">Club annual report identification number</param>
        /// <returns>Annual report</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="403">User hasn't access to annual report</response>
        /// <response code="404">The club annual report does not exist</response>

        [HttpGet("GetClubAnnualReportById/{id:int}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Голова Округу, Голова Станиці, Голова Куреня")]
        public async Task<IActionResult> GetClubAnnualReportById(int id)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, new { annualreport = await _ClubAnnualReportService.GetByIdAsync(await _userManager.GetUserAsync(User), id) });
            }
            catch (NullReferenceException)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
        }

        [HttpPost("CreateClubAnnualReport")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Голова Округу, Голова Станиці, Голова Куреня")]
        public async Task<IActionResult> CreateClubAnnualReport([FromBody] ClubAnnualReportViewModel annualReport)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var clubAnnualReport = _mapper.Map<ClubAnnualReportViewModel, ClubAnnualReportDTO>(annualReport);
                    await _ClubAnnualReportService.CreateAsync(await _userManager.GetUserAsync(User), clubAnnualReport);
                }
                catch (InvalidOperationException)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                catch (NullReferenceException)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

            }
            else
            {
                return BadRequest(ModelState);
            }

            return StatusCode(StatusCodes.Status201Created);
        }


        /// <summary>
        /// Method to confirm annual report
        /// </summary>
        /// <param name="id">Annual report identification number</param>
        /// <returns>Answer from backend</returns>
        /// <response code="200">Annual report was successfully confirmed</response>
        /// <response code="403">User hasn't access to annual report</response>
        /// <response code="404">The annual report does not exist</response>
        [HttpPut("confirmClubAnnualReport/{id:int}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Голова Округу, Голова Станиці, Голова Куреня ")]
        public async Task<IActionResult> ConfirmClubAnnualReport(int id)
        {
            try
            {
                await _ClubAnnualReportService.ConfirmAsync(await _userManager.GetUserAsync(User), id);
                return Ok();
            }
            catch (NullReferenceException)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
        }

        /// <summary>
        /// Method to cancel club annual report
        /// </summary>
        /// <param name="id">Annual report identification number</param>
        /// <returns>Answer from backend</returns>
        /// <response code="200">Annual report was successfully canceled</response>
        /// <response code="403">User hasn't access to annual report</response>
        /// <response code="404">The annual report does not exist</response>
        [HttpPut("cancelClubAnnualReport/{id:int}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Голова Округу, Голова Станиці, Голова Куреня ")]
        public async Task<IActionResult> CancelClubAnnualReport(int id)
        {
            try
            {
                await _ClubAnnualReportService.CancelAsync(await _userManager.GetUserAsync(User), id);
                return Ok();
            }
            catch (NullReferenceException)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
        }

        /// <summary>
        /// Method to delete annual report
        /// </summary>
        /// <param name="id">Club annual report identification number</param>
        /// <returns>Answer from backend</returns>
        /// <response code="200">Annual report was successfully deleted</response>
        /// <response code="403">User hasn't access to annual report</response>
        /// <response code="404">The annual report does not exist</response>
        [HttpDelete("deleteClubAnnualReport/{id:int}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Голова Округу, Голова Станиці, Голова Куреня ")]
        public async Task<IActionResult> DeleteClubAnnualReport(int id)
        {
            try
            {
                await _ClubAnnualReportService.DeleteClubReportAsync(await _userManager.GetUserAsync(User), id);
                return Ok();
            }
            catch (NullReferenceException)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (UnauthorizedAccessException)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
        }

        /// <summary>
        /// Method to edit club annual report
        /// </summary>
        /// <param name="clubAnnualReport"></param>
        /// <returns>Answer from backend</returns>
        /// <response code="200">Club annual report was successfully edited</response>
        /// <response code="403">User hasn't access to club annual report</response>
        /// <response code="404">The club annual report does not exist</response>
        /// <response code="404">Annual report model is not valid</response>
        [HttpPut("editClubAnnualReport")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Голова Округу, Голова Станиці, Голова Куреня ")]
        public async Task<IActionResult> EditClubAnnualReport(ClubAnnualReportDTO clubAnnualReport)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _ClubAnnualReportService.EditClubReportAsync(await _userManager.GetUserAsync(User), clubAnnualReport);
                    return Ok();
                }
                catch (NullReferenceException)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
                catch (UnauthorizedAccessException)
                {
                    return StatusCode(StatusCodes.Status403Forbidden);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}
