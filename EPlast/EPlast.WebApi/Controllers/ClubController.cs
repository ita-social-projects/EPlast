using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BussinessLayer.DTO.Club;
using EPlast.BussinessLayer.Interfaces.Club;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.DataAccess.Entities;
using EPlast.WebApi.Models.Club;
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
            viewModel.IsCurrentUserClubAdmin = userId == viewModel.ClubAdmin?.Id;
            viewModel.IsCurrentUserAdmin = User.IsInRole("Admin");

            return viewModel;
        }
        
        [HttpGet("index")]
        public async  Task<IActionResult> Index()
        {
            var clubs = await _clubService.GetAllClubsAsync();

            return Ok(clubs);
        }
        
        [HttpGet("club/{clubId:int}")]
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
                return BadRequest();
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

        [HttpGet("changeisapprovedstatus/{memberId:int},{clubIndex:int}")]
        public async Task<IActionResult> ChangeIsApprovedStatus(int memberId, int clubIndex)
        {
            try
            {
                await _clubMembersService.ToggleIsApprovedInClubMembersAsync(memberId, clubIndex);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return StatusCode(505);
            }
        }
    }
}
