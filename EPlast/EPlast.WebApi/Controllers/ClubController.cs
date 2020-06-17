using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.DTO.Club;
using EPlast.BussinessLayer.Interfaces.Club;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.WebApi.Models.Club;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubController:ControllerBase
    {
        private readonly IClubService _clubService;
        private readonly IClubAdministrationService _clubAdministrationService;
        private readonly IClubMembersService _clubMembersService;
        private readonly IMapper _mapper;
        private readonly ILoggerService<ClubController> _logger;
        private readonly IUserManagerService _userManagerService;

        public ClubController(IClubService clubService, IClubAdministrationService clubAdministrationService,
            IClubMembersService clubMembersService, IMapper mapper, ILoggerService<ClubController> logger,
            IUserManagerService userManagerService)
        {
            _clubService = clubService;
            _clubAdministrationService = clubAdministrationService;
            _clubMembersService = clubMembersService;
            _mapper = mapper;
            _logger = logger;
            _userManagerService = userManagerService;
        }
        private async Task<ClubProfileViewModel> CheckCurrentUserRoles(ClubProfileViewModel viewModel)
        {
            var userId = await _userManagerService.GetUserIdAsync(User);
            viewModel.IsCurrentUserClubAdmin = userId != null && userId == viewModel.ClubAdmin?.Id;
            viewModel.IsCurrentUserAdmin = User.IsInRole("Admin");

            return viewModel;
        }
        
        [HttpGet("index")]
        public async  Task<IActionResult> Index()
        {
            var clubs = await _clubService.GetAllClubsAsync();

            return Ok(clubs);
        }

        [HttpGet("club/{clubId}")]
        public async Task<IActionResult> Club(int clubId)
        {
            try
            {
                var clubProfileDto = await _clubService.GetClubProfileAsync(clubId);
                if (clubProfileDto.Club == null)
                {
                    return NotFound();
                }
                var viewModel = new ClubProfileViewModel()
                {
                    Club = clubProfileDto.Club,
                    Members = clubProfileDto.Members,
                    Followers = clubProfileDto.Followers,
                    ClubAdministration = clubProfileDto.ClubAdministration.ToList(),
                    ClubAdmin = clubProfileDto.ClubAdmin
                };

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
                var viewModel = new ClubProfileViewModel()
                {
                    Club = clubProfileDto.Club,
                    Members = clubProfileDto.Members,
                    ClubAdmin = clubProfileDto.ClubAdmin
                };
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
                var viewModel = new ClubProfileViewModel()
                {
                    Club = clubProfileDto.Club,
                    Followers = clubProfileDto.Members,
                    ClubAdmin = clubProfileDto.ClubAdmin
                };
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

                return Ok(clubDTO);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return StatusCode(500);
            }
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditClub(ClubDTO club, IFormFile file)
        {
            try
            {
                await _clubService.UpdateAsync(club, file);

                return RedirectToAction("Club", new {index = club.ID});
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return StatusCode(500);
            }
        }

        [HttpGet("clubadmins/{clubID:int}")]
        public async Task<IActionResult> ClubAdmins(int clubID)
        {
            try
            {
                var viewModel = _mapper.Map<ClubProfileDTO, ClubProfileViewModel>(
                    await _clubAdministrationService.GetCurrentClubAdministrationByIDAsync(clubID));
                viewModel = await CheckCurrentUserRoles(viewModel);

                return Ok(viewModel);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return StatusCode(505);
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> ChangeIsApprovedStatus(int memberId, int clubIndex)
        {
            try
            {
                await _clubMembersService.ToggleIsApprovedInClubMembersAsync(memberId, clubIndex);

                return CreatedAtAction(nameof(ClubMembers), clubIndex);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return StatusCode(505);
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> ChangeIsApprovedStatusFollowers(int memberId, int clubIndex)
        {
            try
            {
                await _clubMembersService.ToggleIsApprovedInClubMembersAsync(memberId, clubIndex);

                return CreatedAtAction(nameof(ClubFollowers), clubIndex);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return StatusCode(505);
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> ChangeIsApprovedStatusClub(int memberId, int clubIndex)
        {
            try
            {
                await _clubMembersService.ToggleIsApprovedInClubMembersAsync(memberId, clubIndex);

                return CreatedAtAction(nameof(Club), clubIndex);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return StatusCode(505);
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> DeleteFromAdmins(int adminId, int clubIndex)
        {
            bool isSuccessful = await _clubAdministrationService.DeleteClubAdminAsync(adminId);

            if (isSuccessful)
            {
                return CreatedAtAction(nameof(ClubAdmins), clubIndex);
            }

            return StatusCode(505);
        }

        [HttpPut]
        [Authorize]
        public async Task<int> AddEndDate([FromBody] AdminEndDateDTO adminEndDate)
        {
            try
            {
                await _clubAdministrationService.SetAdminEndDateAsync(adminEndDate);

                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
