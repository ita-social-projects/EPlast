using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Services.Interfaces;
using EPlast.WebApi.Models.Club;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubController : ControllerBase
    {
        private readonly IClubService _clubService;
        private readonly IClubAdministrationService _clubAdministrationService;
        private readonly IClubMembersService _clubMembersService;
        private readonly ILoggerService<ClubController> _logger;
        private readonly IUserManagerService _userManagerService;
        private readonly IMapper _mapper;

        public ClubController(IClubService clubService, IClubAdministrationService clubAdministrationService,
            IClubMembersService clubMembersService, ILoggerService<ClubController> logger,
            IUserManagerService userManagerService, IMapper mapper)
        {
            _clubService = clubService;
            _clubAdministrationService = clubAdministrationService;
            _clubMembersService = clubMembersService;
            _logger = logger;
            _userManagerService = userManagerService;
            _mapper = mapper;
        }

        private async Task<ClubProfileViewModel> CheckCurrentUserRoles(ClubProfileViewModel viewModel)
        {
            var userId = await _userManagerService.GetUserIdAsync(User);
            viewModel.IsCurrentUserClubAdmin = userId != null && userId == viewModel.ClubAdmin?.Id;
            viewModel.IsCurrentUserAdmin = User.IsInRole("Admin");

            return viewModel;
        }

        /// <summary>
        /// Get all clubs
        /// </summary>
        /// <returns>All clubs in object array</returns>
        /// <response code="200">Object array of all clubs</response>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var clubs = await _clubService.GetAllClubsAsync();
            foreach (var club in clubs)
            {
                club.Logo = await _clubService.GetImageBase64Async(club.Logo);
            }

            return Ok(clubs);
        }

        /// <summary>
        /// Get image in base64 format
        /// </summary>
        /// <param name="imageName">Image name</param>
        /// <returns>Image in base64 format</returns>
        /// <response code="200">An base64 image</response>
        [HttpGet("getImage/{imageName}")]
        public async Task<string> GetImage(string imageName)
        {
            return await _clubService.GetImageBase64Async(imageName);
        }

        /// <summary>
        /// Get club by id
        /// </summary>
        /// <param name="clubId">Club id</param>
        /// <returns>Club object</returns>
        /// <response code="200">An instance of club</response>
        [HttpGet("{clubId:int}")]
        public async Task<IActionResult> Club(int clubId)
        {
            var viewModel =
                _mapper.Map<ClubProfileDTO, ClubProfileViewModel>(
                    await _clubService.GetClubProfileAsync(clubId));
            viewModel = await CheckCurrentUserRoles(viewModel);
            viewModel.Club.Logo = await _clubService.GetImageBase64Async(viewModel.Club.Logo);

            return Ok(viewModel);

        }

        /// <summary>
        /// Get all club members by club id
        /// </summary>
        /// <param name="clubId">Club id</param>
        /// <returns>Object array with club members</returns>
        /// <response code="200">An instance of club members</response>
        [HttpGet("{clubId:int}/members")]
        public async Task<IActionResult> GetClubMembers(int clubId)
        {
            var viewModel =
                _mapper.Map<ClubProfileDTO, ClubProfileViewModel>(
                    await _clubService.GetClubMembersOrFollowersAsync(clubId, true));
            viewModel = await CheckCurrentUserRoles(viewModel);

            return Ok(viewModel);
        }

        /// <summary>
        /// Get all club followers by club id
        /// </summary>
        /// <param name="clubId">Club id</param>
        /// <returns>Object array with club followers</returns>
        /// <response code="200">An instance of club followers</response>
        [HttpGet("{clubId:int}/followers")]
        public async Task<IActionResult> GetClubFollowers(int clubId)
        {
            var viewModel =
                _mapper.Map<ClubProfileDTO, ClubProfileViewModel>(
                    await _clubService.GetClubMembersOrFollowersAsync(clubId, false));
            viewModel = await CheckCurrentUserRoles(viewModel);

            return Ok(viewModel);
        }

        /// <summary>
        /// Get club info by id
        /// </summary>
        /// <param name="clubId">Club id</param>
        /// <returns>Club object</returns>
        /// <response code="200">An instance of club</response>
        [HttpGet("{clubId:int}/description")]
        public async Task<IActionResult> ClubDescription(int clubId)
        {
            return Ok(await _clubService.GetClubInfoByIdAsync(clubId));
        }

        /// <summary>
        /// Updated club
        /// </summary>
        /// <param name="club">Club</param>
        /// <returns>Info that club was updated</returns>
        /// <response code="200">An instance of club was updated</response>
        [HttpPost("edit")]
        //[Authorize]
        public async Task<IActionResult> Edit(ClubViewModel club)
        {
            await _clubService.UpdateAsync(_mapper.Map<ClubViewModel, ClubDTO>(club));

            return Ok("Updated");
        }

        /// <summary>
        /// Create club
        /// </summary>
        /// <param name="club">Club</param>
        /// <returns>Info that club was created</returns>
        /// <response code="200">An instance of club was created</response>
        [HttpPost("create")]
        public async Task<IActionResult> Create(ClubViewModel club)
        {
            return Ok(await _clubService.CreateAsync(_mapper.Map<ClubViewModel, ClubDTO>(club)));
        }

        /// <summary>
        /// Get club administration
        /// </summary>
        /// <param name="clubId">Club id</param>
        /// <returns></returns>
        /// <response code="200">An instance of club</response>
        [HttpGet("{clubId:int}/administration")]
        public async Task<IActionResult> GetClubAdministration(int clubId)
        {
            var viewModel =
                _mapper.Map<ClubProfileDTO, ClubProfileViewModel>(
                    await _clubAdministrationService.GetClubAdministrationByIdAsync(clubId));
            viewModel = await CheckCurrentUserRoles(viewModel);

            return Ok(viewModel);
        }

        /// <summary>
        /// Delete administrator from club
        /// </summary>
        /// <param name="adminId">admin id</param>
        /// <returns>Info that the administrator was deleted</returns>
        /// <response code="200">The club administration deleted</response>
        [HttpDelete("administration/{adminId:int}")]
        public async Task<IActionResult> DeleteAdministration(int adminId)
        {
            await _clubAdministrationService.DeleteClubAdminAsync(adminId);

            return Ok($"Club Administrator with id={adminId} deleted.");
        }

        /// <summary>
        /// Change club member approve status
        /// </summary>
        /// <param name="clubId">Club id</param>
        /// <param name="memberId">Club member id</param>
        /// <returns>Object array of club members</returns>
        /// <response code="200">The club member approve changed</response>
        [HttpPut("{clubId:int}/member/{memberId:int}/change-status")]
        public async Task<IActionResult> ChangeApproveStatus(int clubId, int memberId)
        {
            return Ok(_mapper.Map<ClubMembersDTO, ClubMembersViewModel>(
                await _clubMembersService.ToggleIsApprovedInClubMembersAsync(memberId, clubId)));
        }

        /// <summary>
        /// Set club administrator privilege end date
        /// </summary>
        /// <param name="clubAdministrationId">Club administrator id</param>
        /// <param name="endDate">End date</param>
        /// <returns>New club administrator object</returns>
        /// <response code="200">The club administrator date changed</response>
        [HttpPut("administration/{clubAdministrationId:int}/change-end-date")]
        public async Task<IActionResult> SetClubAdministratorEndDate(int clubAdministrationId, DateTime endDate)
        {
            return Ok(await _clubAdministrationService.SetAdminEndDateAsync(clubAdministrationId, endDate));
        }

        /// <summary>
        /// Add new administrator to club
        /// </summary>
        /// <param name="clubId">Club id</param>
        /// <param name="createdAdmin">New administrator</param>
        /// <returns>New club administrator object</returns>
        /// <response code="200">A new club administrator added</response>
        [HttpPost("{clubId:int}/add-administration")]
        public async Task<IActionResult> AddAdmin(int clubId, ClubAdministrationViewModel createdAdmin)
        {
            var club = await _clubService.GetClubInfoByIdAsync(clubId);
            var clubAdministration = _mapper.Map<ClubAdministrationViewModel, ClubAdministrationDTO>(createdAdmin);
            clubAdministration.ClubId = club.ID;

            return Ok(_mapper.Map<ClubAdministrationDTO, ClubAdministrationViewModel>(
                await _clubAdministrationService.AddClubAdminAsync(clubAdministration)));

        }

        /// <summary>
        /// Add new follower to club
        /// </summary>
        /// <param name="clubId">Club id</param>
        /// <param name="userId">User id</param>
        /// <returns>New club follower object</returns>
        ///<response code="200">A new club follower added</response>
        [HttpPost("{clubId:int}/add-follower/{userId}")]
        public async Task<IActionResult> AddFollower(int clubId, string userId)
        {
            userId = User.IsInRole("Admin") ? userId : await _userManagerService.GetUserIdAsync(User);

            return Ok(_mapper.Map<ClubMembersDTO, ClubMembersViewModel>(
                await _clubMembersService.AddFollowerAsync(clubId, userId)));
        }
    }
}