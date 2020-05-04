using EPlast.BussinessLayer.ExtensionMethods;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.Controllers
{
    [Route("[controller]/[action]")]
    public class AdminController : Controller
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;
        private readonly ILogger _logger;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IRepositoryWrapper repoWrapper, ILogger<AdminController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _repoWrapper = repoWrapper;
            _logger = logger;
        }

        public async Task<IActionResult> UsersTable()
        {
            try
            {
                var users = _repoWrapper.User
                    .Include(x => x.UserProfile, x => x.UserPlastDegrees,x=>x.UserProfile.Gender)
                    .ToList();

                var cities = _repoWrapper.City
                    .Include(x => x.Region)
                    .ToList();
                var clubMembers = _repoWrapper.ClubMembers.Include(x => x.Club)
                                                          .ToList();
                var cityMembers = _repoWrapper.CityMembers.Include(x => x.City)
                                                          .ToList();
                List<UserTableViewModel> userTableViewModels = new List<UserTableViewModel>();
                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var cityName = cityMembers.Where(x => x.User.Id.Equals(user.Id) && x.EndDate == null)
                                              .Select(x => x.City.Name)
                                              .LastOrDefault() ?? string.Empty;

                    userTableViewModels.Add(new UserTableViewModel
                    {
                        User = user,
                        ClubName = clubMembers.Where(x => x.UserId.Equals(user.Id) && x.IsApproved == true)
                                              .Select(x => x.Club.ClubName).LastOrDefault() ?? string.Empty,
                        CityName = cityName,
                        RegionName = !cityName.Equals(string.Empty) ? cities.Where(x => x.Name.Equals(cityName))
                                           .FirstOrDefault()
                                           .Region
                                           .RegionName : string.Empty,
                        UserPlastDegreeName = user.UserPlastDegrees.Count != 0 ? user.UserPlastDegrees.Where(x => x.UserId == user.Id && x.DateFinish == null)
                                                                   .FirstOrDefault()
                                                                   .UserPlastDegreeType
                                                                   .GetDescription() : string.Empty,
                        UserRoles = string.Join(", ", roles)
                    });
                }

                return View(userTableViewModels);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, $"Exception: {e.Message}");
                return RedirectToAction("HandleError", "Error");
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var admin = _roleManager.Roles.Where(i => i.Name == "Admin");
                var allRoles = _roleManager.Roles.Except(admin).OrderBy(i => i.Name).ToList();
                RoleViewModel model = new RoleViewModel
                {
                    UserID = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return PartialView(model);
            }
            _logger.Log(LogLevel.Error, $"User, with userId: {userId}, is null");
            return RedirectToAction("HandleError", "Error", new { code = 404 });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var addedRoles = roles.Except(userRoles);
                var removedRoles = userRoles.Except(roles).Except(new List<string> { "Admin"});
                await _userManager.AddToRolesAsync(user, addedRoles);
                await _userManager.RemoveFromRolesAsync(user, removedRoles);
                var currentRoles = await _userManager.GetRolesAsync(user);
                if (currentRoles.Count == 0)
                {
                    await _userManager.AddToRoleAsync(user, "Прихильник");
                }
                _logger.LogInformation("Successful role change for {0} {1}/{2}", user.FirstName, user.LastName, user.Id);
                return RedirectToAction("UsersTable");
            }
            _logger.Log(LogLevel.Error, $"User, with userId: {userId}, is null");
            return RedirectToAction("HandleError", "Error", new { code = 404 });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string userId)
        {
            ViewBag.userId = userId;
            return PartialView();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string userId)
        {
            if (userId != null)
            {
                User user = _repoWrapper.User.FindByCondition(i => i.Id == userId).FirstOrDefault();
                var roles = await _userManager.GetRolesAsync(user);
                if (user != null && !roles.Contains("Admin"))
                {
                    _repoWrapper.User.Delete(user);
                    _repoWrapper.Save();
                    _logger.LogInformation("Successful delete user {0} {1}/{2}", user.FirstName, user.LastName, user.Id);
                    return RedirectToAction("UsersTable");
                }
                _logger.LogError("Cannot find user or admin cannot be deleted. ID:{0}", userId);
            }
            _logger.Log(LogLevel.Error, $"User, with userId: {userId}, is null");
            return RedirectToAction("HandleError", "Error", new { code = 505 });
        }

        [HttpGet]
        public IActionResult RegionsAdmins()
        {
            var cities = _repoWrapper.City.FindAll();
            var model = new RegionsAdmins();
            model.Cities = cities;
            return View(model);
        }

        [HttpGet]
        public IActionResult GetAdmins(int cityId)
        {
            var res = _repoWrapper.CityAdministration.FindByCondition(x => x.CityId == cityId).Include(i => i.User).Include(i => i.AdminType);
            return PartialView(res);
        }
    }
}