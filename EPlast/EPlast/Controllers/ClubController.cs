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

namespace EPlast.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubService _clubService;
        private readonly IClubAdministrationService _clubAdministrationService;
        private readonly IClubMembersService _clubMembersService;
        private readonly IMapper _mapper;
        private readonly ILoggerService<ClubController> _logger;
        private readonly IUserManagerService _userManagerService;

        public ClubController(IClubService clubService, IClubAdministrationService clubAdministrationService, IClubMembersService clubMembersService, IMapper mapper, ILoggerService<ClubController> logger, IUserManagerService userManagerService)
        {
            _clubService = clubService;
            _clubAdministrationService = clubAdministrationService;
            _clubMembersService = clubMembersService;
            _mapper = mapper;
            _logger = logger;
            _userManagerService = userManagerService;
        }
        private void CheckCurrentUserRoles(ref ClubProfileViewModel viewModel)
        {
            viewModel.IsCurrentUserClubAdmin = _userManagerService.GetUserId(User) == viewModel.ClubAdmin?.Id;
            viewModel.IsCurrentUserAdmin = User.IsInRole("Admin");
        }
        public async Task<IActionResult> Index()
        {
            var clubs = await _clubService.GetAllClubsAsync();
            var viewModels = _mapper.Map<IEnumerable<ClubDTO>, IEnumerable<ClubViewModel>>(clubs);

            return View(viewModels);
        }

        public async Task<IActionResult> Club(int index)
        {
            try
            {
                var viewModel = _mapper.Map<ClubProfileDTO, ClubProfileViewModel>(await _clubService.GetClubProfileAsync(index));
                CheckCurrentUserRoles(ref viewModel);

                return View(viewModel);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        public IActionResult ClubAdmins(int index)
        {
            try
            {
                var viewModel = _mapper.Map<ClubProfileDTO, ClubProfileViewModel>(_clubAdministrationService.GetCurrentClubAdministrationByID(index));
                CheckCurrentUserRoles(ref viewModel);

                return View(viewModel);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        public async Task<IActionResult> ClubMembers(int index)
        {
            try
            {
                var viewModel = _mapper.Map<ClubProfileDTO, ClubProfileViewModel>(await _clubService.GetClubMembersOrFollowersAsync(index, true));
                CheckCurrentUserRoles(ref viewModel);

                return View(viewModel);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        public async Task<IActionResult> ClubFollowers(int index)
        {
            try
            {
                var viewModel = _mapper.Map<ClubProfileDTO, ClubProfileViewModel>(await _clubService.GetClubMembersOrFollowersAsync(index, false));
                CheckCurrentUserRoles(ref viewModel);

                return View(viewModel);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        public async Task<IActionResult> ClubDescription(int index)
        {
            try
            {
                var viewModel = _mapper.Map<ClubDTO, ClubViewModel>(await _clubService.GetClubInfoByIdAsync(index));

                return View(viewModel);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditClub(int index)
        {
            try
            {
                var viewModel = _mapper.Map<ClubDTO, ClubViewModel>(await _clubService.GetClubInfoByIdAsync(index));

                return View(viewModel);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditClub(ClubViewModel model, IFormFile file)
        {
            try
            {
                await _clubService.UpdateAsync(_mapper.Map<ClubViewModel, ClubDTO>(model), file);

                return RedirectToAction("Club", new { index = model.ID });
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        [HttpGet]
        public IActionResult ChangeIsApprovedStatus(int index, int clubIndex)
        {
            try
            {
                _clubMembersService.ToggleIsApprovedInClubMembers(index, clubIndex);

                return RedirectToAction("ClubMembers", new { index = clubIndex });
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        [HttpGet]
        public IActionResult ChangeIsApprovedStatusFollowers(int index, int clubIndex)
        {
            try
            {
                _clubMembersService.ToggleIsApprovedInClubMembers(index, clubIndex);

                return RedirectToAction("ClubFollowers", new { index = clubIndex });
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        [HttpGet]
        public IActionResult ChangeIsApprovedStatusClub(int index, int clubIndex)
        {
            try
            {
                _clubMembersService.ToggleIsApprovedInClubMembers(index, clubIndex);

                return RedirectToAction("Club", new { index = clubIndex });
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
        [HttpGet]
        public IActionResult DeleteFromAdmins(int adminId, int clubIndex)
        {
            bool isSuccessfull = _clubAdministrationService.DeleteClubAdmin(adminId);

            if (isSuccessfull)
            {
                return RedirectToAction("ClubAdmins", new { index = clubIndex });
            }
            else
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        [HttpPost]
        public int AddEndDate([FromBody] AdminEndDateDTO adminEndDate)
        {
            try
            {
                _clubAdministrationService.SetAdminEndDate(adminEndDate);

                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        [HttpPost]
        public IActionResult AddToClubAdministration([FromBody] ClubAdministrationDTO createdAdmin)
        {
            try
            {
                _clubAdministrationService.AddClubAdmin(createdAdmin);

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

        public IActionResult AddAsClubFollower(int clubIndex, string userId)
        {
            userId = User.IsInRole("Admin") ? userId : _userManagerService.GetUserId(User);

            _clubMembersService.AddFollower(clubIndex, userId);

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
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateClub(ClubViewModel model, IFormFile file)
        {
            try
            {
                var club = await _clubService.CreateAsync(_mapper.Map<ClubViewModel, ClubDTO>(model), file);

                return RedirectToAction("Club", new { index = club.ID });
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }
    }
}