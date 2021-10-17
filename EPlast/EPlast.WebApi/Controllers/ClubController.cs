using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.Logging;
using EPlast.WebApi.Models.Club;
using EPlast.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using AnnualReportDTOs = EPlast.BLL.DTO.AnnualReport;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ClubController : ControllerBase
    {
        private readonly ILoggerService<ClubController> _logger;
        private readonly IMapper _mapper;
        private readonly IClubService _clubService;
        private readonly IClubParticipantsService _clubParticipantsService;
        private readonly IClubDocumentsService _clubDocumentsService;
        private readonly IClubAccessService _clubAccessService;
        private readonly UserManager<User> _userManager;

        public ClubController(ILoggerService<ClubController> logger,
            IMapper mapper,
            IClubService clubService,
            IClubParticipantsService clubParticipantsService,
            IClubDocumentsService clubDocumentsService,
            IClubAccessService clubAccessService,
            UserManager<User> userManager)
        {
            _logger = logger;
            _mapper = mapper;
            _clubService = clubService;
            _clubParticipantsService = clubParticipantsService;
            _clubDocumentsService = clubDocumentsService;
            _clubAccessService = clubAccessService;
            _userManager = userManager;
        }
        /// <summary>
        /// Get all clubs 
        /// </summary>
        /// <returns>List of clubs</returns>
        [HttpGet("Clubs")]
        public async Task<IActionResult> GetClubs()
        {
            var clubs = await _clubService.GetClubs();
            return Ok(clubs);

        }

        /// <summary>
        /// Get a specific number of Clubs 
        /// </summary>
        /// <param name="page">A number of the page</param>
        /// <param name="pageSize">A count of Clubs to display</param>
        /// <param name="clubName">Optional param to find Clubs by name</param>
        /// <returns>A specific number of Clubs</returns>
        [HttpGet("Profiles/{page}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminPlastMemberAndSupporter)]
        public async Task<IActionResult> GetClubs(int page, int pageSize, string clubName = null)
        {
            var clubs = await _clubService.GetAllClubsAsync(clubName);
            var clubsViewModel = new ClubsViewModel(page, pageSize, clubs, User.IsInRole(Roles.Admin));

            return Ok(clubsViewModel);
        }

        /// <summary>
        /// Get a specific number of active clubs 
        /// </summary>
        /// <param name="page">A number of the page</param>
        /// <param name="pageSize">A count of cities to display</param>
        /// <param name="clubName">Optional param to find cities by name</param>
        /// <returns>A specific number of active clubs</returns>
        [HttpGet("Profiles/Active/{page}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetActiveClubs(int page, int pageSize, string clubName = null)
        {
            var cities = await _clubService.GetAllActiveClubsAsync(clubName);
            var citiesViewModel = new ClubsViewModel(page, pageSize, cities, User.IsInRole(Roles.Admin));

            return Ok(citiesViewModel);
        }

        /// <summary>
        /// Get a specific number of not active clubs 
        /// </summary>
        /// <param name="page">A number of the page</param>
        /// <param name="pageSize">A count of cities to display</param>
        /// <param name="clubName">Optional param to find cities by name</param>
        /// <returns>A specific number of not active clubs</returns>
        [HttpGet("Profiles/NotActive/{page}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetNotActiveClubs(int page, int pageSize, string clubName = null)
        {
            var cities = await _clubService.GetAllNotActiveClubsAsync(clubName);
            var citiesViewModel = new ClubsViewModel(page, pageSize, cities, User.IsInRole(Roles.Admin));

            return Ok(citiesViewModel);
        }

        

        /// <summary>
        /// Get id and name from all clubs that the user has access to
        /// </summary>
        /// <returns>Tuple (int, string)</returns>
        [HttpGet("ClubsOptions")]
        public async Task<IActionResult> GetClubsOptionsThatUserHasAccessTo()
        {
            var clubs = await _clubAccessService.GetAllClubsIdAndName(await _userManager.GetUserAsync(User));
            return Ok(clubs);
        }

        /// <summary>
        /// Get a specific Club
        /// </summary>
        /// <param name="clubId">The id of the Club</param>
        /// <returns>A specific Club</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Club not found</response>
        [HttpGet("Profile/{ClubId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminPlastMemberAndSupporter)]
        public async Task<IActionResult> GetProfile(int clubId)
        {
            var clubProfileDto = await _clubService.GetClubProfileAsync(clubId, await _userManager.GetUserAsync(User));
            if (clubProfileDto == null)
            {
                return NotFound();
            }

            var clubProfile = _mapper.Map<ClubProfileDTO, ClubViewModel>(clubProfileDto);

            return Ok(clubProfile);
        }

        /// <summary>
        /// Get members info of the specific Club
        /// </summary>
        /// <param name="clubId">The id of the Club</param>
        /// <returns>A specific Club</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Club not found</response>
        [HttpGet("ClubMembersInfo/{ClubId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminPlastMemberAndSupporter)]
        public async Task<IActionResult> GetClubMembersInfo(int clubId)
        {
            var clubProfileDto = await _clubService.GetClubDataForReport(clubId);


            if (clubProfileDto == null)
            {
                return NotFound();
            }

            return Ok(clubProfileDto);
        }

        /// <summary>
        /// Get all members of a specific Club
        /// </summary>
        /// <param name="clubId">The id of the Club</param>
        /// <returns>All members of a specific Club</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Club not found</response>
        [HttpGet("Members/{ClubId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminPlastMemberAndSupporter)]
        public async Task<IActionResult> GetMembers(int clubId)
        {
            var clubProfileDto = await _clubService.GetClubMembersAsync(clubId);
            if (clubProfileDto == null)
            {
                return NotFound();
            }

            var clubProfile = _mapper.Map<ClubProfileDTO, ClubViewModel>(clubProfileDto);

            return Ok(new { clubProfile.Members, clubProfile.Name });
        }

        /// <summary>
        /// Get all followers of a specific Club
        /// </summary>
        /// <param name="clubId">The id of the Club</param>
        /// <returns>All followers of a specific Club</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Club not found</response>
        [HttpGet("Followers/{ClubId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminPlastMemberAndSupporter)]
        public async Task<IActionResult> GetFollowers(int clubId)
        {
            var clubProfileDto = await _clubService.GetClubFollowersAsync(clubId);
            if (clubProfileDto == null)
            {
                return NotFound();
            }

            var clubProfile = _mapper.Map<ClubProfileDTO, ClubViewModel>(clubProfileDto);

            return Ok(new { clubProfile.Followers, clubProfile.Name });
        }

        /// <summary>
        /// Get all administrators of a specific Club
        /// </summary>
        /// <param name="clubId">The id of the Club</param>
        /// <returns>All administrators of a specific Club</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Club not found</response>
        [HttpGet("Admins/{ClubId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminPlastMemberAndSupporter)]
        public async Task<IActionResult> GetAdmins(int clubId)
        {
            var clubProfileDto = await _clubService.GetClubAdminsAsync(clubId);
            if (clubProfileDto == null)
            {
                return NotFound();
            }

            var clubProfile = _mapper.Map<ClubProfileDTO, ClubViewModel>(clubProfileDto);

            return Ok(new { clubProfile.Administration, clubProfile.Head, clubProfile.HeadDeputy, clubProfile.Name });
        }

        /// <summary>
        /// Get all documents of a specific Club
        /// </summary>
        /// <param name="clubId">The id of the Club</param>
        /// <returns>All documents of a specific Club</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Club not found</response>
        [HttpGet("Documents/{ClubId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminPlastMemberAndSupporter)]
        public async Task<IActionResult> GetDocuments(int clubId)
        {
            var clubProfileDto = await _clubService.GetClubDocumentsAsync(clubId);
            if (clubProfileDto == null)
            {
                return NotFound();
            }

            var clubProfile = _mapper.Map<ClubProfileDTO, ClubViewModel>(clubProfileDto);

            return Ok(new { clubProfile.Documents });
        }

        /// <summary>
        /// Get an information about a specific Club
        /// </summary>
        /// <param name="clubId">The id of the Club</param>
        /// <returns>An information about a specific Club</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Club not found</response>
        [HttpGet("Details/{ClubId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Details(int clubId)
        {
            var clubDto = await _clubService.GetClubProfileAsync(clubId);
            if (clubDto == null)
            {
                return NotFound();
            }

            return Ok(clubDto);
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
            var logoBase64 = await _clubService.GetLogoBase64(logoName);

            return Ok(logoBase64);
        }

        /// <summary>
        /// Create a new Club
        /// </summary>
        /// <param name="club">An information about a new Club</param>
        /// <returns>An id of a new Club</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="400">Wrong input</response>
        [HttpPost("CreateClub")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Create(ClubViewModel club)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var clubDto = _mapper.Map<ClubViewModel, ClubDTO>(club);
                    clubDto.ID = await _clubService.CreateAsync(clubDto);
                    _logger.LogInformation($"Club {{{clubDto.Name}}} was created.");
                    return Ok(clubDto.ID);
                }
                catch (InvalidOperationException)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Edit a specific Club
        /// </summary>
        /// <param name="club">An information about an edited Club</param>
        /// <response code="200">Successful operation</response>
        /// <response code="400">Wrong input</response>
        [HttpPut("EditClub/{ClubId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Edit(ClubViewModel club)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var clubDto = _mapper.Map<ClubViewModel, ClubDTO>(club);

            await _clubService.EditAsync(clubDto);
            _logger.LogInformation($"Club {{{clubDto.Name}}} was edited.");

            return Ok();
        }

        /// <summary>
        /// Remove a specific Club
        /// </summary>
        /// <param name="clubId">The id of the Club</param>
        [HttpDelete("RemoveClub/{ClubId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Remove(int clubId)
        {
            await _clubService.RemoveAsync(clubId);
            _logger.LogInformation($"Club with id {{{clubId}}} was deleted.");

            return Ok();
        }

        /// <summary>
        /// Add a current user to followers
        /// </summary>
        /// <param name="clubId">An id of the Club</param>
        /// <returns>An information about a new follower</returns>
        [HttpPost("AddFollower/{ClubId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.HeadsAndHeadDeputiesAndAdminPlastunAndSupporter)]
        public async Task<IActionResult> AddFollower(int clubId)
        {
            User ItFollower = await _userManager.GetUserAsync(User);

            await _clubParticipantsService.AddFollowerInHistoryAsync(clubId, ItFollower.Id);

            var follower = await _clubParticipantsService.AddFollowerAsync(clubId, ItFollower);
            _logger.LogInformation($"User {{{follower.UserId}}} became a follower of Club {{{clubId}}}.");

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
            User ItFollower = await _userManager.GetUserAsync(User);
            await _clubParticipantsService.UpdateStatusFollowerInHistoryAsync(ItFollower.Id, true, true);


            await _clubParticipantsService.RemoveFollowerAsync(followerId);
            _logger.LogInformation($"Follower with ID {{{followerId}}} was removed.");

            return Ok();
        }

        /// <summary>
        /// Toggle an approve status for member
        /// </summary>
        /// <param name="memberId">The id of the member</param>
        /// <returns>An information about a specific member</returns>
        [HttpPut("ChangeApproveStatus/{memberId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndKurinHeadAndKurinHeadDeputy)]
        public async Task<IActionResult> ChangeApproveStatus(int memberId)
        {
      
            var member = await _clubParticipantsService.ToggleApproveStatusAsync(memberId);
            if (!member.IsApproved)
            {
                await _clubParticipantsService.UpdateStatusFollowerInHistoryAsync(member.UserId, false, true);

                await _clubParticipantsService.AddFollowerInHistoryAsync(Convert.ToInt32(member.ClubId), member.User.ID);
            }
            else
            {
                await _clubParticipantsService.UpdateStatusFollowerInHistoryAsync(member.UserId, false, false);
            }
            _logger.LogInformation($"Status of member with ID {{{memberId}}} was changed.");

            return Ok(member);

        }
        /// <summary>
        /// Club name only for approved member
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns>club name string</returns>
        [HttpGet("ClubNameOfApprovedMember/{memberId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> ClubNameOfApprovedMember(string memberId)
        {
            var member = await _clubParticipantsService.ClubOfApprovedMember(memberId);

            return Ok(member);
        }

        /// <summary>
        /// Add a new administrator to the Club
        /// </summary>
        /// <param name="newAdmin">An information about a new administrator</param>
        /// <returns>An information about a new administrator</returns>
        [HttpPost("AddAdmin/{ClubId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndKurinHeadAndKurinHeadDeputy)]
        public async Task<IActionResult> AddAdmin(ClubAdministrationViewModel newAdmin)
        {
            var admin = _mapper.Map<ClubAdministrationViewModel, ClubAdministrationDTO>(newAdmin);
            await _clubParticipantsService.AddAdministratorAsync(admin);

            _logger.LogInformation($"User {{{admin.UserId}}} became Admin for Club {{{admin.ClubId}}}" +
                $" with role {{{admin.AdminType.AdminTypeName}}}.");

            return Ok(admin);
        }

        /// <summary>
        /// Archive a specific club
        /// </summary>
        /// <param name="clubId">The id of the club</param>
        [HttpPut("ArchiveClub/{clubId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Archive(int clubId)
        {
            await _clubService.ArchiveAsync(clubId);
            return Ok();
        }

        /// <summary>
        /// Archive a specific club
        /// </summary>
        /// <param name="clubId">The id of the club</param>
        [HttpPut("UnArchiveClub/{clubId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UnArchive(int clubId)
        {
            await _clubService.UnArchiveAsync(clubId);
            return Ok();
        }

        /// <summary>
        /// Remove a specific administrator from the Club
        /// </summary>
        /// <param name="adminId">The id of the administrator</param>
        [HttpPut("RemoveAdmin/{adminId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndKurinHeadAndKurinHeadDeputy)]
        public async Task<IActionResult> RemoveAdmin(int adminId)
        {
            await _clubParticipantsService.RemoveAdministratorAsync(adminId);
            _logger.LogInformation($"Admin with ID {{{adminId}}} was removed.");

            return Ok();
        }

        /// <summary>
        /// Edit an information about a specific administrator
        /// </summary>
        /// <param name="admin">An information about a new administrator</param>
        /// <returns>An information about a specific administrator</returns>
        [HttpPut("EditAdmin/{adminId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndKurinHeadAndKurinHeadDeputy)]
        public async Task<IActionResult> EditAdmin(ClubAdministrationViewModel admin)
        {
            if (admin.EndDate != null && admin.EndDate < DateTime.Today)
            {
                return BadRequest();
            }

            var adminDto = _mapper.Map<ClubAdministrationViewModel, ClubAdministrationDTO>(admin);

            await _clubParticipantsService.EditAdministratorAsync(adminDto);
            _logger.LogInformation($"Admin with User-ID {{{admin.UserId}}} was edited.");

            return Ok(adminDto);
        }

        /// <summary>
        /// Add a document to the Club
        /// </summary>
        /// <param name="document">An information about a specific document</param>
        /// <returns>A newly created document</returns>
        [HttpPost("AddDocument/{ClubId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndKurinHeadAndKurinHeadDeputy)]
        public async Task<IActionResult> AddDocument(ClubDocumentsViewModel document)
        {
            var documentDto = _mapper.Map<ClubDocumentsViewModel, ClubDocumentsDTO>(document);

            await _clubDocumentsService.AddDocumentAsync(documentDto);
            _logger.LogInformation($"Document with id {{{documentDto.ID}}} was added.");

            return Ok(documentDto);
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
            var fileBase64 = await _clubDocumentsService.DownloadFileAsync(fileName);

            return Ok(fileBase64);
        }

        /// <summary>
        /// Remove a specific document
        /// </summary>
        /// <param name="documentId">The id of a specific document</param>
        [HttpDelete("RemoveDocument/{documentId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndKurinHeadAndKurinHeadDeputy)]
        public async Task<IActionResult> RemoveDocument(int documentId)
        {
            await _clubDocumentsService.DeleteFileAsync(documentId);
            _logger.LogInformation($"Document with id {{{documentId}}} was deleted.");

            return Ok();
        }

        [HttpGet("GetDocumentTypes")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetDocumentTypesAsync()
        {
            var documentTypes = await _clubDocumentsService.GetAllClubDocumentTypesAsync();

            return Ok(documentTypes);
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetClubsThatUserHasAccessTo()
        {
            return Ok(new { Clubs = await _clubAccessService.GetClubsAsync(await _userManager.GetUserAsync(User)) });
        }



        [HttpGet("GetUserAdmins/{UserId}")]
        public async Task<IActionResult> GetUserAdministrations(string userId)
        {
            var userAdmins = await _clubParticipantsService.GetAdministrationsOfUserAsync(userId);

            return Ok(userAdmins);
        }


        [HttpGet("GetUserPreviousAdmins/{UserId}")]
        public async Task<IActionResult> GetUserPreviousAdministrations(string userId)
        {
            var userAdmins = await _clubParticipantsService.GetPreviousAdministrationsOfUserAsync(userId);

            return Ok(userAdmins);
        }

        [HttpGet("GetAllAdministrationStatuses/{UserId}")]
        public async Task<IActionResult> GetAllAdministrationStatuses(string userId)
        {
            var userAdmins = await _clubParticipantsService.GetAdministrationStatuses(userId);

            return Ok(userAdmins);
        }
    }
}
