using System;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BussinessLayer.DTO.Club;
using EPlast.BussinessLayer.Interfaces.Club;
using EPlast.BussinessLayer.Interfaces.Logging;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.WebApi.Models.Club;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            return Ok(await _clubService.GetAllClubsAsync());
        }

        [HttpGet("{clubId:int}")]
        public async Task<IActionResult> Club(int clubId)
        {
            try
            {
                var viewModel =
                    _mapper.Map<ClubProfileDTO, ClubProfileViewModel>(await _clubService.GetClubProfileAsync(clubId));
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

        [HttpPost("edit")]
        [Authorize]
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

        [HttpPost("create")]
        public async Task<IActionResult> Create(ClubViewModel model, [FromForm] IFormFile file)
        {
            try
            {
                return Ok(await _clubService.CreateAsync(_mapper.Map<ClubViewModel, ClubDTO>(model), file));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

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

        [HttpDelete("administration/{adminId:int}")]
        public async Task<IActionResult> DeleteAdministration(int adminId)
        {
            if (await _clubAdministrationService.DeleteClubAdminAsync(adminId))
            {
                return Ok("Club Administrator with id={adminId} deleted.");
            }

            return BadRequest();
        }

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

        [HttpPut("administration/{clubAdministrationId:int}/change-end-date")]
        public async Task<IActionResult> AddEndDate(int clubAdministrationId, DateTime endDate)
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

        [HttpPost("{clubId:int}/add-follower/{userId:int}")]
        public async Task<IActionResult> AddFollower(int clubId, string userId)
        {
            try
            {
                userId = User.IsInRole("Admin") ? userId : await _userManagerService.GetUserIdAsync(User);

                return Ok(await _clubMembersService.AddFollowerAsync(clubId, userId));
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