using AutoMapper;
using EPlast.BussinessLayer.DTO.City;
using EPlast.BussinessLayer.Interfaces.City;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.ViewModels.City;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace EPlast.Controllers
{
    public class CityController : Controller
    {
        private readonly ILoggerService<CityController> _logger;
        private readonly ICityService _cityService;
        private readonly IMapper _mapper;
        public CityController(ILoggerService<CityController> logger, ICityService cityService, IMapper mapper)
        {
            _cityService = cityService;
            _mapper = mapper;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(_mapper.Map<IEnumerable<CityDTO>, IEnumerable<CityViewModel>>(_cityService.GetAllDTO()));
        }

        public IActionResult CityProfile(int cityId)
        {
            try
            {
                CityProfileDTO cityProfileDto = _cityService.CityProfile(cityId);
                if(cityProfileDto == null)
                    return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status404NotFound });

                return View(_mapper.Map<CityProfileDTO, CityProfileViewModel>(_cityService.CityProfile(cityId)));

            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        public IActionResult CityMembers(int cityId)
        {
            try
            {
                CityProfileDTO cityProfileDto = _cityService.CityMembers(cityId);
                if (cityProfileDto == null)
                    return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status404NotFound });
                return View(_mapper.Map<CityProfileDTO, CityProfileViewModel>(_cityService.CityMembers(cityId)));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        public IActionResult CityFollowers(int cityId)
        {
            try
            {
                CityProfileDTO cityProfile = _cityService.CityFollowers(cityId);
                if(cityProfile == null)
                    return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status404NotFound });
                return View(_mapper.Map<CityProfileDTO, CityProfileViewModel>(cityProfile));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        public IActionResult CityAdmins(int cityId)
        {
            try
            {
                CityProfileDTO cityProfileDto = _cityService.CityAdmins(cityId);
                if(cityProfileDto == null)
                    return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status404NotFound });
                return View(_mapper.Map<CityProfileDTO, CityProfileViewModel>(_cityService.CityAdmins(cityId)));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        [HttpGet]
        public IActionResult Edit(int cityId)
        {
            try
            {
                CityProfileDTO cityProfileDto = _cityService.Edit(cityId);
                if (cityProfileDto == null)
                    return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status404NotFound });
                return View(_mapper.Map<CityProfileDTO, CityProfileViewModel>(_cityService.Edit(cityId)));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        [HttpPost]
        public IActionResult Edit(CityProfileViewModel model, IFormFile file)
        {
            try
            {
                if (!ModelState.IsValid) return View("Edit", model.City.ID);

                _cityService.Edit(_mapper.Map<CityProfileViewModel, CityProfileDTO>(model), file);
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
        public IActionResult Create(CityProfileViewModel model, IFormFile file)
        {
            try
            {
                if (!ModelState.IsValid) return View("Create", model);
                int cityId=_cityService.Create(_mapper.Map<CityProfileViewModel, CityProfileDTO>(model), file);
                return RedirectToAction("CityProfile", "City", new { cityid = cityId });

            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        public IActionResult Details(int cityId)
        {
            try
            {
                CityDTO cityDto = _cityService.GetById(cityId);
                if (cityDto == null)
                    return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status404NotFound });
                return View(_mapper.Map<CityDTO, CityViewModel>(cityDto));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

        public IActionResult CityDocuments(int cityId)
        {
            try
            {
                CityProfileDTO cityProfileDto = _cityService.CityDocuments(cityId);
                if (cityProfileDto == null)
                    return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status404NotFound });
                return View(_mapper.Map<CityProfileDTO, CityProfileViewModel>(cityProfileDto));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = StatusCodes.Status505HttpVersionNotsupported });
            }
        }

    }
}