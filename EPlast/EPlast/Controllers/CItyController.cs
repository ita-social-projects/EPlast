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
            //List<CityViewModel> cities = new List<CityViewModel>(
            //    _repoWrapper.City
            //    .FindAll()
            //    .Select(city => new CityViewModel { City = city })
            //    .ToList());
            return View(_mapper.Map<IEnumerable<CityDTO>, IEnumerable<CityViewModel>>(_cityService.GetAllDTO()));
        }

        public IActionResult CityProfile(int cityId)
        {
            try
            {
                return View(_mapper.Map<CityProfileDTO, CityProfileViewModel>(_cityService.CityProfile(cityId)));

            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        public IActionResult CityMembers(int cityId)
        {
            try
            {
                return View(_mapper.Map<CityProfileDTO, CityProfileViewModel>(_cityService.CityMembers(cityId)));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        public IActionResult CityFollowers(int cityId)
        {
            try
            {
                return View(_mapper.Map<CityProfileDTO, CityProfileViewModel>(_cityService.CityFollowers(cityId)));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        public IActionResult CityAdmins(int cityId)
        {
            try
            {
                return View(_mapper.Map<CityProfileDTO, CityProfileViewModel>(_cityService.CityAdmins(cityId)));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        [HttpGet]
        public IActionResult Edit(int cityId)
        {
            try
            {
                return View(_mapper.Map<CityProfileDTO, CityProfileViewModel>(_cityService.Edit(cityId)));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        [HttpPost]
        public IActionResult Edit(CityProfileViewModel model, IFormFile file)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    _cityService.Edit(_mapper.Map<CityProfileViewModel, CityProfileDTO>(model), file);
                    _logger.LogInformation($"City {model.City.Name} was edited profile and saved in the database");
                    return RedirectToAction("CityProfile", "City", new { cityid = model.City.ID });
                }
                else
                {
                    // var city = _repoWrapper.City
                    //.FindByCondition(q => q.ID == model.City.ID)
                    //.Include(c => c.CityAdministration)
                    //.ThenInclude(t => t.AdminType)
                    //.Include(k => k.CityAdministration)
                    //.ThenInclude(a => a.User)
                    //.Include(m => m.CityMembers)
                    //.ThenInclude(u => u.User)
                    //.Include(l => l.CityDocuments)
                    //.ThenInclude(d => d.CityDocumentType)
                    //.FirstOrDefault();

                    // var cityAdmins = city.CityAdministration
                    //                     .Where(a => a.EndDate == null && a.AdminType.AdminTypeName != "Голова Станиці")
                    //                     .ToList();
                    // var members = city.CityMembers.Where(m => m.EndDate == null && m.StartDate != null).ToList();
                    // var followers = city.CityMembers.Where(m => m.EndDate == null && m.StartDate == null).ToList();
                    // CityViewModel oldmodel = new CityViewModel { City = city, CityAdmins = cityAdmins, Members = members, Followers = followers };
                    return View("Edit", model.City.ID);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
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
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        [HttpPost]
        public IActionResult Create(CityProfileViewModel model, IFormFile file)
        {
            try
            {
                //if (file != null && file.Length > 0)
                //{
                //    var img = Image.FromStream(file.OpenReadStream());
                //    var uploads = Path.Combine(_env.WebRootPath, "images\\Cities");

                //    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                //    var filePath = Path.Combine(uploads, fileName);
                //    img.Save(filePath);
                //    model.City.Logo = fileName;
                //}
                //else
                //{
                //    model.City.Logo = "333493fe-9c81-489f-bce3-5d1ba35a8c36.jpg";
                //}
                if (ModelState.IsValid)
                {
                    int cityId=_cityService.Create(_mapper.Map<CityProfileViewModel, CityProfileDTO>(model), file);
                    return RedirectToAction("CityProfile", "City", new { cityid = cityId });
                }
                else
                {
                    return View("Create", model);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        public IActionResult Details(int cityId)
        {
            try
            {
                return View(_mapper.Map<CityDTO, CityViewModel>(_cityService.GetById(cityId)));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        public IActionResult CityDocuments(int cityId)
        {
            try
            {
                return View(_mapper.Map<CityProfileDTO, CityProfileViewModel>(_cityService.CityDocuments(cityId)));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception :{e.Message}");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

    }
}