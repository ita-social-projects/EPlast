using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Club;
using EPlast.BLL.Interfaces.Club;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Services.Interfaces;
using EPlast.ViewModels;
using EPlast.ViewModels.Club;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

            return View(_mapper.Map<IEnumerable<ClubDTO>, IEnumerable<ClubViewModel>>(clubs));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Club(int index)
        {
            try
            {
                var clubProfileDto = await _clubService.GetClubProfileAsync(index);
                var viewModel = _mapper.Map<ClubProfileDTO, ClubProfileViewModel>(clubProfileDto);
                viewModel = await CheckCurrentUserRoles(viewModel);

                return View(viewModel);
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status404NotFound });
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error",
                    new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        public async Task<IActionResult> ClubAdmins(int index)
        {
            try
            {
                var viewModel = _mapper.Map<ClubProfileDTO, ClubProfileViewModel>(
                    await _clubAdministrationService.GetClubAdministrationByIdAsync(index));
                viewModel = await CheckCurrentUserRoles(viewModel);

                return View(viewModel);
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status404NotFound });
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error",
                    new { code = StatusCodes.Status505HttpVersionNotsupported });
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
                return RedirectToAction("HandleError", "Error",
                    new { code = StatusCodes.Status505HttpVersionNotsupported });
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
                return RedirectToAction("HandleError", "Error",
                    new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> ClubDescription(int index)
        {
            try
            {
                var clubDto = await _clubService.GetClubInfoByIdAsync(index);
                if (clubDto == null)
                {
                    return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status404NotFound });
                }

                return View(_mapper.Map<ClubDTO, ClubViewModel>(clubDto));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error",
                    new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditClub(int index)
        {
            try
            {
                var clubDto = await _clubService.GetClubInfoByIdAsync(index);
                if (clubDto == null)
                {
                    return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status404NotFound });
                }

                return View(_mapper.Map<ClubDTO, ClubViewModel>(clubDto));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error",
                    new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditClub(ClubViewModel model)
        {
            try
            {
                await _clubService.UpdateAsync(_mapper.Map<ClubViewModel, ClubDTO>(model));

                return RedirectToAction("Club", new { index = model.ID });
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error",
                    new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ChangeIsApprovedStatus(int index, int clubIndex)
        {
            try
            {
                return (await _clubMembersService.ToggleIsApprovedInClubMembersAsync(index, clubIndex)).IsApproved
                    ? RedirectToAction("ClubFollowers", new { index = clubIndex })
                    : RedirectToAction("ClubMembers", new { index = clubIndex });
            }
            catch (NullReferenceException e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error",
                    new { code = StatusCodes.Status404NotFound });
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error",
                    new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ChangeIsApprovedStatusClub(int index, int clubIndex)
        {
            try
            {
                await _clubMembersService.ToggleIsApprovedInClubMembersAsync(index, clubIndex);

                return RedirectToAction("Club", new { index = clubIndex });
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status404NotFound });
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error",
                    new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteFromAdmins(int adminId, int clubIndex)
        {
            bool isSuccessful = await _clubAdministrationService.DeleteClubAdminAsync(adminId);

            if (isSuccessful)
            {
                return RedirectToAction("ClubAdmins", new { index = clubIndex });
            }

            return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
        }

        [HttpPost]
        public async Task<int> AddEndDate(int clubAdministrationId, [FromBody] DateTime endDate)
        {
            try
            {
                await _clubAdministrationService.SetAdminEndDateAsync(clubAdministrationId, endDate);

                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToClubAdministration(int clubId,
            [FromBody] ClubAdministrationDTO createdAdmin)
        {
            try
            {
                await _clubService.GetClubInfoByIdAsync(clubId);
                createdAdmin.ClubId = clubId;
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
            var clubs =
                _mapper.Map<IEnumerable<ClubDTO>, IEnumerable<ClubViewModel>>(await _clubService.GetAllClubsAsync());
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

            return RedirectToAction("UserProfile", "Account", new { userId });
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

                return RedirectToAction("HandleError", "Error",
                    new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateClub(ClubViewModel model)
        {
            try
            {
                var club = await _clubService.CreateAsync(_mapper.Map<ClubViewModel, ClubDTO>(model));

                return RedirectToAction("Club", new { index = club.ID });
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error",
                    new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }
    }
}