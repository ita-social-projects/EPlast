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
        /// <response code="404">There is no club</response>
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
        /// <response code="404">The image does not exist</response>
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
        /// <response code="404">The club does not exist</response>
        [HttpGet("{clubId:int}")]
        public async Task<IActionResult> Club(int clubId)
        {
            try
            {
                var viewModel =
                    _mapper.Map<ClubProfileDTO, ClubProfileViewModel>(await _clubService.GetClubProfileAsync(clubId));
                viewModel = await CheckCurrentUserRoles(viewModel);
                viewModel.Club.Logo = await _clubService.GetImageBase64Async(viewModel.Club.Logo);
                return Ok(viewModel);
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
                return BadRequest();
            }
        }

        /// <summary>
        /// Get all club members by club id
        /// </summary>
        /// <param name="clubId">Club id</param>
        /// <returns>Object array with club members</returns>
        /// <response code="200">An instance of club members</response>
        /// <response code="404">Club members does not exist</response>
        [HttpGet("{clubId:int}/members")]
        public async Task<IActionResult> GetClubMembers(int clubId)
        {
            try
            {
                var viewModel =
                    _mapper.Map<ClubProfileDTO, ClubProfileViewModel>(
                        await _clubService.GetClubMembersOrFollowersAsync(clubId, true));
                viewModel = await CheckCurrentUserRoles(viewModel);

                return Ok(viewModel);
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
                return BadRequest();
            }
        }

        /// <summary>
        /// Get all club followers by club id
        /// </summary>
        /// <param name="clubId">Club id</param>
        /// <returns>Object array with club followers</returns>
        /// <response code="200">An instance of club followers</response>
        /// <response code="404">Club followers does not exist</response>
        [HttpGet("{clubId:int}/followers")]
        public async Task<IActionResult> GetClubFollowers(int clubId)
        {
            try
            {
                var viewModel =
                    _mapper.Map<ClubProfileDTO, ClubProfileViewModel>(
                        await _clubService.GetClubMembersOrFollowersAsync(clubId, false));
                viewModel = await CheckCurrentUserRoles(viewModel);

                return Ok(viewModel);
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
                return BadRequest();
            }
        }

        /// <summary>
        /// Get club info by id
        /// </summary>
        /// <param name="clubId">Club id</param>
        /// <returns>Club object</returns>
        /// <response code="200">An instance of club</response>
        /// <response code="400">The club does not found</response>
        /// <response code="404">The club does not exist</response>
        [HttpGet("{clubId:int}/description")]
        public async Task<IActionResult> ClubDescription(int clubId)
        {
            try
            {
                return Ok(await _clubService.GetClubInfoByIdAsync(clubId));
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return BadRequest();
            }
        }

        /// <summary>
        /// Updated club
        /// </summary>
        /// <param name="club">Club</param>
        /// <returns>Info that club was updated</returns>
        /// <response code="200">An instance of club was updated</response>
        /// <response code="400">The id and club id are not same</response>
        [HttpPost("edit")]
        //[Authorize]
        public async Task<IActionResult> Edit(ClubViewModel club)
        {
            try
            {
                await _clubService.UpdateAsync(_mapper.Map<ClubViewModel, ClubDTO>(club));

                return Ok("Updated");
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        /// <summary>
        /// Create club
        /// </summary>
        /// <param name="club">Club</param>
        /// <returns>Info that club was created</returns>
        /// <response code="200">An instance of club was created</response>
        /// <response code="400">Problem with file validation or model state is not valid</response>
        [HttpPost("create")]
        public async Task<IActionResult> Create(ClubViewModel club)
        {
            try
            {
                return Ok(await _clubService.CreateAsync(_mapper.Map<ClubViewModel, ClubDTO>(club)));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        /// <summary>
        /// Get club administration
        /// </summary>
        /// <param name="clubId">Club id</param>
        /// <returns></returns>
        /// <response code="200">An instance of club</response>
        /// <response code="400">The club administration does not found</response>
        /// <response code="404">The club administration does not exist</response>
        [HttpGet("{clubId:int}/administration")]
        public async Task<IActionResult> GetClubAdministration(int clubId)
        {
            try
            {
                var viewModel =
                    _mapper.Map<ClubProfileDTO, ClubProfileViewModel>(
                        await _clubAdministrationService.GetClubAdministrationByIdAsync(clubId));
                viewModel = await CheckCurrentUserRoles(viewModel);

                return Ok(viewModel);
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        /// <summary>
        /// Delete administrator from club
        /// </summary>
        /// <param name="adminId">admin id</param>
        /// <returns>Info that the administrator was deleted</returns>
        /// <response code="200">The club administration deleted</response>
        /// <response code="400">The club administration does not found</response>
        /// <response code="404">The club administration does not exist</response>
        [HttpDelete("administration/{adminId:int}")]
        public async Task<IActionResult> DeleteAdministration(int adminId)
        {
            try
            {
                await _clubAdministrationService.DeleteClubAdminAsync(adminId);

                return Ok($"Club Administrator with id={adminId} deleted.");
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        /// <summary>
        /// Change club member approve status
        /// </summary>
        /// <param name="clubId">Club id</param>
        /// <param name="memberId">Club member id</param>
        /// <returns>Object array of club members</returns>
        /// <response code="200">The club member approve changed</response>
        /// <response code="400">The club member does not found</response>
        /// <response code="404">The club member does not exist</response>
        [HttpPut("{clubId:int}/member/{memberId:int}/change-status")]
        public async Task<IActionResult> ChangeApproveStatus(int clubId, int memberId)
        {
            try
            {
                return Ok(_mapper.Map<ClubMembersDTO, ClubMembersViewModel>(
                    await _clubMembersService.ToggleIsApprovedInClubMembersAsync(memberId, clubId)));
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        /// <summary>
        /// Set club administrator privilege end date
        /// </summary>
        /// <param name="clubAdministrationId">Club administrator id</param>
        /// <param name="endDate">End date</param>
        /// <returns>New club administrator object</returns>
        /// <response code="200">The club administrator date changed</response>
        /// <response code="400">The club administrator does not found</response>
        /// <response code="404">The club administrator does not exist</response>
        [HttpPut("administration/{clubAdministrationId:int}/change-end-date")]
        public async Task<IActionResult> SetClubAdministratorEndDate(int clubAdministrationId, DateTime endDate)
        {
            try
            {
                return Ok(await _clubAdministrationService.SetAdminEndDateAsync(clubAdministrationId, endDate));
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        /// <summary>
        /// Add new administrator to club
        /// </summary>
        /// <param name="clubId">Club id</param>
        /// <param name="createdAdmin">New administrator</param>
        /// <returns>New club administrator object</returns>
        /// <response code="400">The club does not found</response>
        /// <response code="404">The club does not exist</response>
        [HttpPost("{clubId:int}/add-administration")]
        public async Task<IActionResult> AddAdmin(int clubId, ClubAdministrationViewModel createdAdmin)
        {
            try
            {
                var club = await _clubService.GetClubInfoByIdAsync(clubId);
                var clubAdministration = _mapper.Map<ClubAdministrationViewModel, ClubAdministrationDTO>(createdAdmin);
                clubAdministration.ClubId = club.ID;

                return Ok(_mapper.Map<ClubAdministrationDTO, ClubAdministrationViewModel>(
                    await _clubAdministrationService.AddClubAdminAsync(clubAdministration)));
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        /// <summary>
        /// Add new follower to club
        /// </summary>
        /// <param name="clubId">Club id</param>
        /// <param name="userId">User id</param>
        /// <returns>New club follower object</returns>
        /// <response code="400">The club does not found</response>
        /// <response code="404">The club does not exist</response>
        [HttpPost("{clubId:int}/add-follower/{userId}")]
        public async Task<IActionResult> AddFollower(int clubId, string userId)
        {
            try
            {
                userId = User.IsInRole("Admin") ? userId : await _userManagerService.GetUserIdAsync(User);

                return Ok(_mapper.Map<ClubMembersDTO, ClubMembersViewModel>(
                    await _clubMembersService.AddFollowerAsync(clubId, userId)));
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }
    }
}