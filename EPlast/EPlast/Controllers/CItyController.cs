using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Services.Interfaces;
using EPlast.ViewModels.City;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.Controllers
{
    public class CityController : Controller
    {
        private readonly ILoggerService<CityController> _logger;
        private readonly ICityService _cityService;
        private readonly ICityMembersService _cityMembersService;
        private readonly IMapper _mapper;

        public CityController(ILoggerService<CityController> logger,
            ICityService cityService,
            ICityMembersService cityMembersService,
            IMapper mapper)
        {
            _logger = logger;
            _cityService = cityService;
            _cityMembersService = cityMembersService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<CityDTO>, IEnumerable<CityViewModel>>(await _cityService.GetAllDTOAsync()));
        }

        public async Task<IActionResult> CityProfile(int cityId)
        {
            try
            {
                CityProfileDTO cityProfileDto = await _cityService.GetCityProfileAsync(cityId);
                if (cityProfileDto == null)
                {
                    return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status404NotFound });
                }

                return View(_mapper.Map<CityProfileDTO, CityProfileViewModel>(cityProfileDto));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        public async Task<IActionResult> CityMembers(int cityId)
        {
            try
            {
                CityProfileDTO cityProfileDto = await _cityService.GetCityMembersAsync(cityId);
                if (cityProfileDto == null)
                {
                    return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status404NotFound });
                }

                return View(_mapper.Map<CityProfileDTO, CityProfileViewModel>(cityProfileDto));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        public async Task<IActionResult> CityAdmins(int cityId)
        {
            try
            {
                CityProfileDTO cityProfileDto = await _cityService.GetCityAdminsAsync(cityId);
                if (cityProfileDto == null)
                {
                    return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status404NotFound });
                }

                return View(_mapper.Map<CityProfileDTO, CityProfileViewModel>(cityProfileDto));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        public async Task<IActionResult> CityDocuments(int cityId)
        {
            try
            {
                CityProfileDTO cityProfileDto = await _cityService.GetCityDocumentsAsync(cityId);
                if (cityProfileDto == null)
                {
                    return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status404NotFound });
                }

                return View(_mapper.Map<CityProfileDTO, CityProfileViewModel>(cityProfileDto));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }


        public async Task<IActionResult> CityFollowers(int cityId)
        {
            try
            {
                CityProfileDTO cityProfile = await _cityService.GetCityFollowersAsync(cityId);
                if (cityProfile == null)
                {
                    return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status404NotFound });
                }

                return View(_mapper.Map<CityProfileDTO, CityProfileViewModel>(cityProfile));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        public async Task<IActionResult> Details(int cityId)
        {
            try
            {
                CityDTO cityDto = await _cityService.GetByIdAsync(cityId);
                if (cityDto == null)
                {
                    return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status404NotFound });
                }

                return View(_mapper.Map<CityDTO, CityViewModel>(cityDto));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int cityId)
        {
            try
            {
                CityProfileDTO cityProfileDto = await _cityService.EditAsync(cityId);
                if (cityProfileDto == null)
                {
                    return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status404NotFound });
                }

                return View(_mapper.Map<CityProfileDTO, CityProfileViewModel>(cityProfileDto));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CityProfileViewModel model, IFormFile file)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Edit", model);
                }

                await _cityService.EditAsync(_mapper.Map<CityProfileViewModel, CityProfileDTO>(model), file);
                _logger.LogInformation($"City {model.City.Name} was edited profile and saved in the database");

                return RedirectToAction("CityProfile", "City", new { cityid = model.City.ID });
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
                return View(new CityProfileViewModel());
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CityProfileViewModel model, IFormFile file)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Create", model);
                }

                int cityId = await _cityService.CreateAsync(_mapper.Map<CityProfileViewModel, CityProfileDTO>(model), file);
                _logger.LogInformation($"City {model.City.Name} was created profile and saved in the database");

                return RedirectToAction("CityProfile", "City", new { cityid = cityId });
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        public async Task<IActionResult> AddCityFollower(int cityId, string userId)
        {
            try
            {
                await _cityMembersService.AddFollowerAsync(cityId, userId);
                _logger.LogInformation($"User {userId} became a follower of city with id {cityId}.");

                return RedirectToAction("CityProfile", "City", new { cityid = cityId });
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        public async Task<IActionResult> RemoveCityFollower(int cityId, string userId)
        {
            try
            {
                await _cityMembersService.RemoveMemberAsync(userId);
                _logger.LogInformation($"Follower with id {userId} was removed.");

                return RedirectToAction("CityFollowers", "City", new { cityid = cityId });
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        public async Task<IActionResult> AddCityMember(int cityId, string userId)
        {
            try
            {
                await _cityMembersService.ToggleApproveStatusAsync(userId);
                _logger.LogInformation($"Status of user {userId} was changed in city with id {cityId}.");

                return RedirectToAction("CityFollowers", "City", new { cityid = cityId });
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        public async Task<IActionResult> RemoveCityMember(int cityId, string userId)
        {
            try
            {
                await _cityMembersService.ToggleApproveStatusAsync(userId);
                _logger.LogInformation($"Status of user {userId} was changed in city with id {cityId}.");

                return RedirectToAction("CityMembers", "City", new { cityid = cityId });
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");

                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }
    }
}