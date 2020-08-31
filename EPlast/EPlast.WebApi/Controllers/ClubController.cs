using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Services.Interfaces;
using EPlast.WebApi.Models.Club;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
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
        private readonly IUserManagerService _userManagerService;
        private readonly IMapper _mapper;

        public ClubController(IClubService clubService, IClubAdministrationService clubAdministrationService,
            IClubMembersService clubMembersService, IUserManagerService userManagerService, IMapper mapper)
        {
            _clubService = clubService;
            _clubAdministrationService = clubAdministrationService;
            _clubMembersService = clubMembersService;
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
        [Authorize(AuthenticationSchemes = "Bearer")]
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
        /// Gets a specific number of clubs.
        /// </summary>
        /// <param name="pageNumber">A number of the page.</param>
        /// <param name="pageSize">A count of clubs to display.</param>
        /// <returns>Returns a specific number of clubs.</returns>
        /// <response code="200">Object array of a specific number of clubs.</response>
        [HttpGet("page/{pageNumber:int}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetPartOfClubs(int pageNumber, int pageSize)
        {
            var sampleClubs = await _clubService.GetPartOfClubsAsync(pageNumber, pageSize);

            foreach (var club in sampleClubs)
            {
                club.Logo = await _clubService.GetImageBase64Async(club.Logo);
            }

            return Ok(sampleClubs);
        }

        /// <summary>
        /// Gets a general count of clubs.
        /// </summary>
        /// <returns>Returns count of clubs.</returns>
        /// <response code="200">Count of clubs.</response>
        [HttpGet("count")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetClubsCount()
        {
            var clubsCount = await _clubService.GetClubsCountAsync();

            return Ok(clubsCount);
        }

        /// <summary>
        /// Get image in base64 format
        /// </summary>
        /// <param name="imageName">Image name</param>
        /// <returns>Image in base64 format</returns>
        /// <response code="200">An base64 image</response>
        [HttpGet("getImage/{imageName}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
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
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetClub(int clubId)
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
        [Authorize(AuthenticationSchemes = "Bearer")]
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
        [Authorize(AuthenticationSchemes = "Bearer")]
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
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetClubDescription(int clubId)
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
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Edit(ClubViewModel club)
        {
            var mappedClub = _mapper.Map<ClubViewModel, ClubDTO>(club);

            var isClubNameNotChanged = await _clubService.VerifyClubNameIsNotChangedAsync(mappedClub);

            if (!isClubNameNotChanged)
            {
                var isValid = await _clubService.ValidateAsync(mappedClub);

                if (!isValid)
                {
                    return StatusCode((int)HttpStatusCode.UnprocessableEntity);
                }
            }

            await _clubService.UpdateAsync(mappedClub);

            return Ok("Updated");
        }

        /// <summary>
        /// Create club
        /// </summary>
        /// <param name="club">Club</param>
        /// <returns>Info that club was created</returns>
        /// <response code="200">An instance of club was created</response>
        [HttpPost("create")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Create(ClubViewModel club)
        {
            var isValid =  await _clubService.ValidateAsync(_mapper.Map<ClubViewModel, ClubDTO>(club));

            if (!isValid)
            {
                return StatusCode((int)HttpStatusCode.UnprocessableEntity);
            }

            return Ok(await _clubService.CreateAsync(_mapper.Map<ClubViewModel, ClubDTO>(club)));
        }

        /// <summary>
        /// Get club administration
        /// </summary>
        /// <param name="clubId">Club id</param>
        /// <returns></returns>
        /// <response code="200">An instance of club</response>
        [HttpGet("{clubId:int}/administration")]
        [Authorize(AuthenticationSchemes = "Bearer")]
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
        [Authorize(AuthenticationSchemes = "Bearer")]
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
        [Authorize(AuthenticationSchemes = "Bearer")]
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
        [Authorize(AuthenticationSchemes = "Bearer")]
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
        [Authorize(AuthenticationSchemes = "Bearer")]
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
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddFollower(int clubId, string userId)
        {
            userId = User.IsInRole("Admin") ? userId : await _userManagerService.GetUserIdAsync(User);

            return Ok(_mapper.Map<ClubMembersDTO, ClubMembersViewModel>(
                await _clubMembersService.AddFollowerAsync(clubId, userId)));
        }
    }
}