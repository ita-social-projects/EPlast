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
        private readonly ILoggerService<ClubController> _logger;
        private readonly IClubService _ClubService;
        private readonly IClubMembersService _ClubMembersService;
        private readonly IMapper _mapper;

        public ClubController(ILoggerService<ClubController> logger,
            IClubService ClubService,
            IClubMembersService ClubMembersService,
            IMapper mapper)
        {
            _logger = logger;
            _ClubService = ClubService;
            _ClubMembersService = ClubMembersService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ClubDTO>, IEnumerable<ClubViewModel>>(await _ClubService.GetAllDTOAsync(null)));
        }

        public async Task<IActionResult> ClubProfile(int ClubId)
        {
            try
            {
                ClubProfileDTO ClubProfileDto = await _ClubService.GetClubProfileAsync(ClubId);
                if (ClubProfileDto == null)
                {
                    return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status404NotFound });
                }

                return View(_mapper.Map<ClubProfileDTO, ClubProfileViewModel>(ClubProfileDto));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        public async Task<IActionResult> ClubMembers(int ClubId)
        {
            try
            {
                ClubProfileDTO ClubProfileDto = await _ClubService.GetClubMembersAsync(ClubId);
                if (ClubProfileDto == null)
                {
                    return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status404NotFound });
                }

                return View(_mapper.Map<ClubProfileDTO, ClubProfileViewModel>(ClubProfileDto));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        public async Task<IActionResult> ClubAdmins(int ClubId)
        {
            try
            {
                ClubProfileDTO ClubProfileDto = await _ClubService.GetClubAdminsAsync(ClubId);
                if (ClubProfileDto == null)
                {
                    return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status404NotFound });
                }

                return View(_mapper.Map<ClubProfileDTO, ClubProfileViewModel>(ClubProfileDto));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        public async Task<IActionResult> ClubDocuments(int ClubId)
        {
            try
            {
                ClubProfileDTO ClubProfileDto = await _ClubService.GetClubDocumentsAsync(ClubId);
                if (ClubProfileDto == null)
                {
                    return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status404NotFound });
                }

                return View(_mapper.Map<ClubProfileDTO, ClubProfileViewModel>(ClubProfileDto));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }


        public async Task<IActionResult> ClubFollowers(int ClubId)
        {
            try
            {
                ClubProfileDTO ClubProfile = await _ClubService.GetClubFollowersAsync(ClubId);
                if (ClubProfile == null)
                {
                    return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status404NotFound });
                }

                return View(_mapper.Map<ClubProfileDTO, ClubProfileViewModel>(ClubProfile));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        public async Task<IActionResult> Details(int ClubId)
        {
            try
            {
                ClubDTO ClubDto = await _ClubService.GetByIdAsync(ClubId);
                if (ClubDto == null)
                {
                    return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status404NotFound });
                }

                return View(_mapper.Map<ClubDTO, ClubViewModel>(ClubDto));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int ClubId)
        {
            try
            {
                ClubProfileDTO ClubProfileDto = await _ClubService.EditAsync(ClubId);
                if (ClubProfileDto == null)
                {
                    return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status404NotFound });
                }

                return View(_mapper.Map<ClubProfileDTO, ClubProfileViewModel>(ClubProfileDto));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ClubProfileViewModel model, IFormFile file)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Edit", model);
                }

                await _ClubService.EditAsync(_mapper.Map<ClubProfileViewModel, ClubProfileDTO>(model), file);
                _logger.LogInformation($"Club {model.Club.Name} was edited profile and saved in the database");

                return RedirectToAction("ClubProfile", "Club", new { Clubid = model.Club.ID });
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            try
            {
                return View(new ClubProfileViewModel());
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(ClubProfileViewModel model, IFormFile file)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Create", model);
                }

                int ClubId = await _ClubService.CreateAsync(_mapper.Map<ClubProfileViewModel, ClubProfileDTO>(model), file);
                _logger.LogInformation($"Club {model.Club.Name} was created profile and saved in the database");

                return RedirectToAction("ClubProfile", "Club", new { Clubid = ClubId });
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        public async Task<IActionResult> AddClubFollower(int ClubId, string userId)
        {
            try
            {
                await _ClubMembersService.AddFollowerAsync(ClubId, userId);
                _logger.LogInformation($"User {userId} became a follower of Club with id {ClubId}.");

                return RedirectToAction("ClubProfile", "Club", new { Clubid = ClubId });
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        public async Task<IActionResult> RemoveClubFollower(int ClubId, int followerId)
        {
            try
            {
                await _ClubMembersService.RemoveFollowerAsync(followerId);
                _logger.LogInformation($"Follower with id {followerId} was removed.");

                return RedirectToAction("ClubFollowers", "Club", new { Clubid = ClubId });
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        public async Task<IActionResult> AddClubMember(int ClubId, int memberId)
        {
            try
            {
                await _ClubMembersService.ToggleApproveStatusAsync(memberId);
                _logger.LogInformation($"Status of user {memberId} was changed in Club with id {ClubId}.");

                return RedirectToAction("ClubFollowers", "Club", new { Clubid = ClubId });
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        public async Task<IActionResult> RemoveClubMember(int ClubId, int memberId)
        {
            try
            {
                await _ClubMembersService.ToggleApproveStatusAsync(memberId);
                _logger.LogInformation($"Status of user {memberId} was changed in Club with id {ClubId}.");

                return RedirectToAction("ClubMembers", "Club", new { Clubid = ClubId });
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }
    }
}