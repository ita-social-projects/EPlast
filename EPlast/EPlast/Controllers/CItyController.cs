using EPlast.DataAccess.Entities;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.Controllers
{
    public class CityController : Controller
    {
        private readonly DataAccess.Repositories.IRepositoryWrapper _repoWrapper;
        private readonly IHostingEnvironment _env;
        private readonly ILogger _logger;
        private UserManager<User> _userManager;
        public CityController(DataAccess.Repositories.IRepositoryWrapper repoWrapper, UserManager<User> userManager, ILogger<AccountController> logger, IHostingEnvironment env)
        {
            _userManager = userManager;
            _repoWrapper = repoWrapper;
            _logger = logger;
            _env = env;
        }

        public IActionResult Index()
        {
            List<CityViewModel> cities = new List<CityViewModel>(
                _repoWrapper.City
                .FindAll()
                .Select(city => new CityViewModel { City = city })
                .ToList());

            return View(cities);
        }

        public IActionResult CityProfile(int cityId)
        {
            try
            {
                var city = _repoWrapper.City
                    .FindByCondition(q => q.ID == cityId)
                    .Include(c => c.CityAdministration)
                    .ThenInclude(t => t.AdminType)
                    .Include(k => k.CityAdministration)
                    .ThenInclude(a => a.User)
                    .Include(m => m.CityMembers)
                    .ThenInclude(u => u.User)
                    .Include(l=>l.CityDocuments)
                    .ThenInclude(d=>d.CityDocumentType)
                    .FirstOrDefault();

                var cityHead = city.CityAdministration
                    .Where(a => a.EndDate == null && a.AdminType.AdminTypeName == "Голова Станиці")
                    .FirstOrDefault();

                var cityAdmins = city.CityAdministration
                    .Where(a => a.EndDate == null && a.AdminType.AdminTypeName != "Голова Станиці")
                    .ToList();

                var members = city.CityMembers.Where(m => m.EndDate == null && m.StartDate!=null).Take(6).ToList();
                var followers = city.CityMembers.Where(m => m.EndDate == null && m.StartDate == null).Take(6).ToList();
                
                

                

                var cityDoc = city.CityDocuments.Take(4).ToList();

                ViewBag.usermanager = _userManager;
                return View(new CityViewModel { City = city, CityHead = cityHead, Members = members, Followers = followers , CityAdmins = cityAdmins , CityDoc = cityDoc});

            }
            catch (Exception e)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        public IActionResult CityMembers(int cityid)
        {
            try {
                var city = _repoWrapper.City
                    .FindByCondition(q => q.ID == cityid)
                    .Include(c => c.CityAdministration)
                    .ThenInclude(t => t.AdminType)
                    .Include(k => k.CityAdministration)
                    .ThenInclude(a => a.User)
                    .Include(m => m.CityMembers)
                    .ThenInclude(u => u.User)
                    .Include(l => l.CityDocuments)
                    .ThenInclude(d => d.CityDocumentType)
                    .FirstOrDefault();

                var members = city.CityMembers.Where(m => m.EndDate == null && m.StartDate != null).ToList();

                return View(new CityViewModel { City = city, Members = members });
            }
            catch (Exception e)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        public IActionResult CityFollowers(int cityid)
        {
            try
            {
                var city = _repoWrapper.City
                    .FindByCondition(q => q.ID == cityid)
                    .Include(c => c.CityAdministration)
                    .ThenInclude(t => t.AdminType)
                    .Include(k => k.CityAdministration)
                    .ThenInclude(a => a.User)
                    .Include(m => m.CityMembers)
                    .ThenInclude(u => u.User)
                    .Include(l => l.CityDocuments)
                    .ThenInclude(d => d.CityDocumentType)
                    .FirstOrDefault();

                var followers = city.CityMembers.Where(m => m.EndDate == null && m.StartDate == null).ToList();

                return View(new CityViewModel { City = city, Followers = followers });
            }
            catch (Exception e)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        public IActionResult CityAdmins(int cityid)
        {
            try
            {
                var city = _repoWrapper.City
                    .FindByCondition(q => q.ID == cityid)
                    .Include(c => c.CityAdministration)
                    .ThenInclude(t => t.AdminType)
                    .Include(k => k.CityAdministration)
                    .ThenInclude(a => a.User)
                    .Include(m => m.CityMembers)
                    .ThenInclude(u => u.User)
                    .Include(l => l.CityDocuments)
                    .ThenInclude(d => d.CityDocumentType)
                    .FirstOrDefault();

                var cityAdmins = city.CityAdministration
                                    .Where(a => a.EndDate == null && a.AdminType.AdminTypeName != "Голова Станиці")
                                    .ToList();

                return View(new CityViewModel { City = city, CityAdmins = cityAdmins });
            }
            catch (Exception e)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        [HttpGet]
        public IActionResult Edit(int cityid)
        {
            try
            {
                var city = _repoWrapper.City
                    .FindByCondition(q => q.ID == cityid)
                    .Include(c => c.CityAdministration)
                    .ThenInclude(t => t.AdminType)
                    .Include(k => k.CityAdministration)
                    .ThenInclude(a => a.User)
                    .Include(m => m.CityMembers)
                    .ThenInclude(u => u.User)
                    .Include(l => l.CityDocuments)
                    .ThenInclude(d => d.CityDocumentType)
                    .FirstOrDefault();

                var cityAdmins = city.CityAdministration.Where(a => a.EndDate == null).ToList();
                var members = city.CityMembers.Where(p=> cityAdmins.All(a=>a.UserId!=p.UserId)).Where(m => m.EndDate == null && m.StartDate != null).ToList();
                var followers = city.CityMembers.Where(m => m.EndDate == null && m.StartDate == null).ToList();

                return View(new CityViewModel { City = city, CityAdmins = cityAdmins, Members = members, Followers = followers });
            }
            catch (Exception e)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        [HttpPost]
        public IActionResult Edit(CityViewModel model, IFormFile file)
        {
            try
            {
                var oldImageName = _repoWrapper.City.FindByCondition(i => i.ID == model.City.ID).FirstOrDefault().Logo;
                if (file != null && file.Length > 0)
                {
                    var img = Image.FromStream(file.OpenReadStream());
                    var uploads = Path.Combine(_env.WebRootPath, "images\\Cities");
                    if (!string.IsNullOrEmpty(oldImageName) && !string.Equals(oldImageName, "default.png"))
                    {
                        var oldPath = Path.Combine(uploads, oldImageName);
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }

                    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(uploads, fileName);
                    img.Save(filePath);
                    model.City.Logo = fileName;
                }
                else
                {
                    model.City.Logo = oldImageName;
                }
                if (ModelState.IsValid)
                {
                    _repoWrapper.City.Update(model.City);
                    _repoWrapper.Save();
                    _logger.LogInformation("City {0} was edited profile and saved in the database", model.City.Name);

                    return RedirectToAction("CityProfile", "City", new { cityid = model.City.ID });
                }
                else
                {
                    var city = _repoWrapper.City
                   .FindByCondition(q => q.ID == model.City.ID)
                   .Include(c => c.CityAdministration)
                   .ThenInclude(t => t.AdminType)
                   .Include(k => k.CityAdministration)
                   .ThenInclude(a => a.User)
                   .Include(m => m.CityMembers)
                   .ThenInclude(u => u.User)
                   .Include(l => l.CityDocuments)
                   .ThenInclude(d => d.CityDocumentType)
                   .FirstOrDefault();

                    var cityAdmins = city.CityAdministration
                                        .Where(a => a.EndDate == null && a.AdminType.AdminTypeName != "Голова Станиці")
                                        .ToList();
                    var members = city.CityMembers.Where(m => m.EndDate == null && m.StartDate != null).ToList();
                    var followers = city.CityMembers.Where(m => m.EndDate == null && m.StartDate == null).ToList();
                    CityViewModel oldmodel = new CityViewModel { City = city, CityAdmins = cityAdmins, Members = members, Followers = followers };
                    return View("Edit",oldmodel);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Exception: {0}", e.Message);
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }

        }

        [HttpGet]
        public IActionResult Create()
        {
            try
            {
                return View(new CityViewModel());
            }
            catch (Exception e)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        [HttpPost]
        public IActionResult Create(CityViewModel model, IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    var img = Image.FromStream(file.OpenReadStream());
                    var uploads = Path.Combine(_env.WebRootPath, "images\\Cities");

                    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(uploads, fileName);
                    img.Save(filePath);
                    model.City.Logo = fileName;
                }
                else
                {
                    model.City.Logo = "333493fe-9c81-489f-bce3-5d1ba35a8c36.jpg";
                }
                if (ModelState.IsValid)
                {
                    _repoWrapper.City.Create(model.City);
                    _repoWrapper.Save();

                    return RedirectToAction("CityProfile","City", new { cityid = model.City.ID });
                }
                else
                {
                    return View("Create", model);
                }
            }
            catch (Exception e)
            {

                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        public IActionResult Details(int cityid)
        {
            try
            {
                var city = _repoWrapper.City
                    .FindByCondition(q => q.ID == cityid)
                    .Include(c => c.CityAdministration)
                    .ThenInclude(t => t.AdminType)
                    .Include(k => k.CityAdministration)
                    .ThenInclude(a => a.User)
                    .Include(m => m.CityMembers)
                    .ThenInclude(u => u.User)
                    .Include(l => l.CityDocuments)
                    .ThenInclude(d => d.CityDocumentType)
                    .FirstOrDefault();

                var cityAdmins = city.CityAdministration
                                    .Where(a => a.EndDate == null && a.AdminType.AdminTypeName != "Голова Станиці")
                                    .ToList();

                return View(new CityViewModel { City = city });
            }
            catch (Exception e)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

        public IActionResult CityDocuments(int cityid)
        {
            try
            {
                var city = _repoWrapper.City
                    .FindByCondition(q => q.ID == cityid)
                    .Include(c => c.CityAdministration)
                    .ThenInclude(t => t.AdminType)
                    .Include(k => k.CityAdministration)
                    .ThenInclude(a => a.User)
                    .Include(m => m.CityMembers)
                    .ThenInclude(u => u.User)
                    .Include(l => l.CityDocuments)
                    .ThenInclude(d => d.CityDocumentType)
                    .FirstOrDefault();

                var cityDoc = city.CityDocuments.ToList();

                return View(new CityViewModel { City = city, CityDoc = cityDoc });
            }
            catch (Exception e)
            {
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
        }

    }
}