﻿using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.BussinessLayer.DTO.City;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.ViewModels;
using EPlast.ViewModels.City;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.Controllers
{
    [Route("[controller]/[action]")]
    public class AdminController : Controller
    {
        private readonly ILogger _logger;
        private readonly IUserManagerService _userManagerService;
        private readonly IAdminService _adminService;
        private readonly IMapper _mapper;
        private readonly ICityService _cityService;
        private readonly ICItyAdministrationService _cityAdministrationService;

        public AdminController(ILogger<AdminController> logger,
            IUserManagerService userManagerService,
            IAdminService adminService,
            IMapper mapper,
            ICityService cityService,
            ICItyAdministrationService cityAdministrationService)
        {
            _logger = logger;
            _userManagerService = userManagerService;
            _adminService = adminService;
            _mapper = mapper;
            _cityService = cityService;
            _cityAdministrationService = cityAdministrationService;
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

            if (!string.IsNullOrEmpty(userId))
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

            if (!string.IsNullOrEmpty(userId))
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
                if (!string.IsNullOrEmpty(userId))
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
            var cities = _cityService.GetAllDTO();
            var model = new RegionsAdminsViewModel
            {
                Cities = _mapper.Map<IEnumerable<CityDTO>, IEnumerable<CityViewModel>>(cities)
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult GetAdmins(int cityId)
        {
            var res = _mapper.Map<IEnumerable<CityAdministrationDTO>, IEnumerable<CityAdministrationViewModel>>(_cityAdministrationService.GetByCityId(cityId));
            return PartialView(res);
        }
    }
}