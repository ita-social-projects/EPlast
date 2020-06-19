using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.DTO.Club;
using EPlast.BussinessLayer.Interfaces.Club;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.WebApi.Models.Club;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var clubs = await _clubService.GetAllClubsAsync();

            return Ok(_mapper.Map<IEnumerable<ClubDTO>, IEnumerable<ClubViewModel>>(clubs));
        }

        [HttpGet("Club/{clubId}")]
        public async Task<IActionResult> Club(int clubId)
        {
            try
            {
                var clubProfileDto = await _clubService.GetClubProfileAsync(clubId);
                if (clubProfileDto.Club == null)
                {
                    return NotFound();
                }
                var viewModel = _mapper.Map<ClubProfileDTO, ClubProfileViewModel>(clubProfileDto);

                viewModel = await CheckCurrentUserRoles(viewModel);

                return Ok(viewModel);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
                return StatusCode(500);
            }
        }

        [HttpGet("ClubMembers/{clubId}")]
        public async Task<IActionResult> ClubMembers(int clubId)
        {
            try
            {
                var clubProfileDto = await _clubService.GetClubMembersOrFollowersAsync(clubId, true);
                if (clubProfileDto.Club == null)
                {
                    return NotFound();
                }
                var viewModel = _mapper.Map<ClubProfileDTO, ClubProfileViewModel>(clubProfileDto);
                viewModel = await CheckCurrentUserRoles(viewModel);

                return Ok(viewModel);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
                return StatusCode(500);
            }
        }

        [HttpGet("ClubFollowers/{clubId}")]
        public async Task<IActionResult> ClubFollowers(int clubId)
        {
            try
            {
                var clubProfileDto = await _clubService.GetClubMembersOrFollowersAsync(clubId, false);
                if (clubProfileDto.Club == null)
                {
                    return NotFound();
                }
                var viewModel = _mapper.Map<ClubProfileDTO, ClubProfileViewModel>(clubProfileDto);
                viewModel = await CheckCurrentUserRoles(viewModel);

                return Ok(viewModel);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e.Message}");
                return StatusCode(500);
            }
        }

        [HttpGet("ClubDescription/{clubId}")]
        public async Task<IActionResult> ClubDescription(int clubId)
        {
            try
            {
                var clubDTO = await _clubService.GetClubInfoByIdAsync(clubId);
                if (clubDTO == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<ClubDTO, ClubViewModel>(clubDTO));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return StatusCode(500);
            }
        }

        [HttpPost("Edit")]
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

                return StatusCode(500);
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(ClubViewModel model, [FromForm] IFormFile file)
        {
            try
            {
                var club = await _clubService.CreateAsync(_mapper.Map<ClubViewModel, ClubDTO>(model), file);

                return Ok(club);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return StatusCode(500);
            }
        }

        [HttpGet("clubadmins/{clubId:int}")]
        public async Task<IActionResult> ClubAdmins(int clubId)
        {
            try
            {
                var clubProfileDto = await _clubAdministrationService.GetCurrentClubAdministrationByIDAsync(clubId);

                if (clubProfileDto.Club == null)
                {
                    return NotFound();
                }

                var viewModel = _mapper.Map<ClubProfileDTO, ClubProfileViewModel>(clubProfileDto);
                viewModel = await CheckCurrentUserRoles(viewModel);

                return Ok(viewModel);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        [HttpDelete("deleteadmin")]
        public async Task<IActionResult> DeleteAdmin(int adminId)
        {
            bool isSuccessful = await _clubAdministrationService.DeleteClubAdminAsync(adminId);

            if (isSuccessful)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPut("change-approve-status")]
        public async Task<IActionResult> ChangeApproveStatus(int memberId, int clubIndex)
        {
            try
            {
                await _clubMembersService.ToggleIsApprovedInClubMembersAsync(memberId, clubIndex);

                return Ok(true);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        [HttpPut("changeapprovestatusfollower")]
        public async Task<IActionResult> ChangeApproveStatusFollower(int memberId, int clubIndex)
        {
            try
            {
                await _clubMembersService.ToggleIsApprovedInClubMembersAsync(memberId, clubIndex);

                return Ok(true);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        [HttpPut("changeapprovestatusclub")]
        public async Task<IActionResult> ChangeApproveStatusClub(int memberId, int clubIndex)
        {
            try
            {
                await _clubMembersService.ToggleIsApprovedInClubMembersAsync(memberId, clubIndex);

                return Ok(true);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        [HttpPut("addenddate")]
        public async Task<IActionResult> AddEndDate(AdminEndDateDTO adminEndDate)
        {
            try
            {
                await _clubAdministrationService.SetAdminEndDateAsync(adminEndDate);

                return Ok(true);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return BadRequest();
            }
        }

        [HttpPost("addadmin")]
        public async Task<IActionResult> AddAdmin(ClubAdministrationDTO createdAdmin)
        {
            try
            {
                await _clubAdministrationService.AddClubAdminAsync(createdAdmin);

                return Ok(true);
            }
            catch (Exception)
            {
                return Ok(false);
            }
        }

        [HttpPost("addfollower")]
        public async Task<IActionResult> AddFollower(int clubIndex, string userId)
        {
            userId = User.IsInRole("Admin") ? userId : await _userManagerService.GetUserIdAsync(User);
            await _clubMembersService.AddFollowerAsync(clubIndex, userId);

            return Ok();
        }
    }
}
