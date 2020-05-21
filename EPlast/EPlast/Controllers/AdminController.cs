using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.ExtensionMethods;
using EPlast.BussinessLayer.Services;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.DataAccess.Repositories;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.Controllers
{
    [Route("[controller]/[action]")]
    public class AdminController : Controller
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly ILogger _logger;
        private readonly IUserManagerService _userManagerService;
        private readonly IAdminService _adminService;
        private readonly IMapper _mapper;

        public AdminController(IRepositoryWrapper repoWrapper, ILogger<AdminController> logger, IUserManagerService userManagerService, IAdminService adminService, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
            _userManagerService = userManagerService;
            _adminService = adminService;
            _mapper = mapper;
        }

        public async Task<IActionResult> UsersTable()
        {
            try
            {
                var result = _mapper.Map<IEnumerable<UserTableDTO>, IEnumerable<UserTableViewModel>>(await _adminService.UsersTable());
                return View(result);
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

            if (userId != null)
            {
                var user = await _userManagerService.FindById(userId);
                if (user == null)
                {
                    _logger.Log(LogLevel.Error, $"Can`t find the User");
                    return RedirectToAction("HandleError", "Error", new { code = 404 });
                }
                var userRoles = await _userManagerService.GetRoles(user);
                var allRoles = _adminService.GetRolesExceptAdmin();

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

            if (userId != null)
            {
                await _adminService.Edit(userId, roles);
                _logger.LogInformation("Successful role change for {0}", userId);
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
            try
            {
                if (userId != null)
                {
                    await _adminService.DeleteUser(userId);
                    _logger.LogInformation("Successful delete user {0}", userId);
                    return RedirectToAction("UsersTable");
                }
                _logger.Log(LogLevel.Error, $"User, with userId: {userId}, is null");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
            catch
            {
                _logger.Log(LogLevel.Error, "Smth went wrong");
                return RedirectToAction("HandleError", "Error", new { code = 505 });
            }
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