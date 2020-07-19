using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Services.Interfaces;
using EPlast.WebApi.Models.Admin;
using EPlast.WebApi.Models.Role;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IUserManagerService _userManagerService;
        private readonly IAdminService _adminService;
        private readonly ICityService _cityService;
        private readonly ICityAdministrationService _cityAdministrationService;

        public AdminController(ILogger<AdminController> logger,
            IUserManagerService userManagerService,
            IAdminService adminService,
            ICityService cityService,
            ICityAdministrationService cityAdministrationService)
        {
            _logger = logger;
            _userManagerService = userManagerService;
            _adminService = adminService;
            _cityService = cityService;
            _cityAdministrationService = cityAdministrationService;
        }
        // [Authorize]
        [HttpGet("usersTable")]
        public async Task<IActionResult> UsersTable()
        {
            try
            {
                var result = await _adminService.UsersTableAsync();
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, $"Exception: {e.Message}");
                return BadRequest();
            }
        }

        // [Authorize(Roles = "Admin")]
        [HttpGet("editRole/{userId}")]
        public async Task<IActionResult> Edit(string userId)
        {
            try
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    var user = await _userManagerService.FindByIdAsync(userId);
                    if (user == null)
                    {
                        _logger.Log(LogLevel.Error, $"Can`t find the User");
                        return BadRequest();
                    }
                    var userRoles = await _userManagerService.GetRolesAsync(user);
                    var allRoles = await _adminService.GetRolesExceptAdminAsync();

                    RoleViewModel model = new RoleViewModel
                    {
                        UserID = user.Id,
                        UserEmail = user.Email,
                        UserRoles = userRoles,
                        AllRoles = allRoles
                    };

                    return Ok(model);
                }
                throw new ArgumentException("User id is null");
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, $"Exception: {e.Message}");
                return BadRequest();
            }
        }

        // [Authorize(Roles = "Admin")]
        [HttpPut("editRole")]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            try
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    await _adminService.EditAsync(userId, roles);
                    _logger.LogInformation($"Successful change role for {userId}");
                    return Ok();
                }
                throw new ArgumentException("User id is null");
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, $"Exception: {e.Message}");
                return BadRequest();
            }
        }

        [HttpGet("confirmDelete/{userId}")]
        //  [Authorize(Roles = "Admin")]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string userId)
        {
            return Ok(userId);
        }

        //  [Authorize(Roles = "Admin")]
        [HttpDelete("deleteUser/{userId}")]
        public async Task<IActionResult> Delete(string userId)
        {
            try
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    await _adminService.DeleteUserAsync(userId);
                    _logger.LogInformation("Successful delete user {0}", userId);

                    return Ok();
                }
                throw new ArgumentException("User id is null");
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, $"Smth went wrong {e.Message}");
                return BadRequest();
            }
        }

        [HttpGet("regionsAdmins")]
        public async Task<IActionResult> RegionsAdmins()
        {
            var model = new CitiesAdminsViewModel()
            {
                Cities = await _cityService.GetAllDTOAsync()
            };

            return Ok(model);
        }

        [HttpGet("cityAdmins/{cityId}")]
        public async Task<IActionResult> GetAdmins(int cityId)
        {
            return Ok(await _cityAdministrationService.GetByCityIdAsync(cityId));
        }
    }
}
