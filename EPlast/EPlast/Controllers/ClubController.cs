using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.DTO.Club;
using EPlast.BussinessLayer.Interfaces.Club;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.ViewModels;
using EPlast.ViewModels.Club;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace EPlast.Controllers
{
    [Authorize]
    public class ClubController : Controller
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

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var clubs = await _clubService.GetAllClubsAsync();
            var viewModels = _mapper.Map<IEnumerable<ClubDTO>, IEnumerable<ClubViewModel>>(clubs);

            return View(viewModels);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Club(int index)
        {
            try
            {
                var clubProfileDto = await _clubService.GetClubProfileAsync(index);
                if (clubProfileDto.Club == null)
                {
                    return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status404NotFound });
                }
                var viewModel = _mapper.Map<ClubProfileDTO, ClubProfileViewModel>(clubProfileDto);
                viewModel = await CheckCurrentUserRoles(viewModel);

                return View(viewModel);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        public async Task<IActionResult> ClubAdmins(int index)
        {
            try
            {
                var viewModel = _mapper.Map<ClubProfileDTO, ClubProfileViewModel>(
                    await _clubAdministrationService.GetCurrentClubAdministrationByIDAsync(index));
                viewModel = await CheckCurrentUserRoles(viewModel);

                return View(viewModel);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> ClubMembers(int index)
        {
            try
            {
                var viewModel = _mapper.Map<ClubProfileDTO, ClubProfileViewModel>(
                        await _clubService.GetClubMembersOrFollowersAsync(index, true));
                viewModel = await CheckCurrentUserRoles(viewModel);

                return View(viewModel);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> ClubFollowers(int index)
        {
            try
            {
                var viewModel = _mapper.Map<ClubProfileDTO, ClubProfileViewModel>(
                        await _clubService.GetClubMembersOrFollowersAsync(index, false));
                viewModel = await CheckCurrentUserRoles(viewModel);

                return View(viewModel);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> ClubDescription(int index)
        {
            try
            {
                var clubDTO = await _clubService.GetClubInfoByIdAsync(index);
                if (clubDTO == null)
                {
                    return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status404NotFound });
                }

                return View(_mapper.Map<ClubDTO, ClubViewModel>(clubDTO));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditClub(int index)
        {
            try
            {
              var clubDto=await _clubService.GetClubInfoByIdAsync(index);
                if (clubDto == null)
                {
                    return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status404NotFound });
                }
                return View(_mapper.Map<ClubDTO, ClubViewModel>(clubDto));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditClub(ClubViewModel model, IFormFile file)
        {
            try
            {
                await _clubService.UpdateAsync(_mapper.Map<ClubViewModel, ClubDTO>(model), file);

                return RedirectToAction("Club", new {index = model.ID});
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ChangeIsApprovedStatus(int index, int clubIndex)
        {
            try
            {
                await _clubMembersService.ToggleIsApprovedInClubMembersAsync(index, clubIndex);

                return RedirectToAction("ClubMembers", new {index = clubIndex});
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ChangeIsApprovedStatusFollowers(int index, int clubIndex)
        {
            try
            {
                await _clubMembersService.ToggleIsApprovedInClubMembersAsync(index, clubIndex);

                return RedirectToAction("ClubFollowers", new {index = clubIndex});
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ChangeIsApprovedStatusClub(int index, int clubIndex)
        {
            try
            {
                await _clubMembersService.ToggleIsApprovedInClubMembersAsync(index, clubIndex);

                return RedirectToAction("Club", new {index = clubIndex});
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteFromAdmins(int adminId, int clubIndex)
        {
            bool isSuccessful = await _clubAdministrationService.DeleteClubAdminAsync(adminId);

            if (isSuccessful)
            {
                return RedirectToAction("ClubAdmins", new {index = clubIndex});
            }

            return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
        }

        [HttpPost]
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

        [HttpPost]
        public async Task<IActionResult> AddToClubAdministration([FromBody] ClubAdministrationDTO createdAdmin)
        {
            try
            {
                await _clubAdministrationService.AddClubAdminAsync(createdAdmin);

                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        public async Task<IActionResult> ChooseAClub(string userId)
        {
            var clubs = _mapper.Map<IEnumerable<ClubDTO>, IEnumerable<ClubViewModel>>(await _clubService.GetAllClubsAsync());
            var model = new ClubChooseAClubViewModel
            {
                Clubs = clubs,
                UserId = userId
            };
            return View(model);
        }

        public async Task<IActionResult> AddAsClubFollower(int clubIndex, string userId)
        {
            userId = User.IsInRole("Admin") ? userId : await _userManagerService.GetUserIdAsync(User);
            await _clubMembersService.AddFollowerAsync(clubIndex, userId);

            return RedirectToAction("UserProfile", "Account", new {userId});
        }

        [HttpGet]
        public IActionResult CreateClub()
        {
            try
            {
                return View(new ClubViewModel());
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateClub(ClubViewModel model, IFormFile file)
        {
            try
            {
                var club = await _clubService.CreateAsync(_mapper.Map<ClubViewModel, ClubDTO>(model), file);

                return RedirectToAction("Club", new {index = club.ID});
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }
    }
}