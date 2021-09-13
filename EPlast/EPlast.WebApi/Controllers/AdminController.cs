using EPlast.BLL.DTO.Admin;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Services.Interfaces;
using EPlast.Resources;
using EPlast.WebApi.Models.Admin;
using EPlast.WebApi.Models.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Roles = Roles.HeadsAndHeadDeputiesAndAdminAndPlastunAndGBHeadAndGBSectorHead)]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        private readonly ICityParticipantsService _cityAdministrationService;

        private readonly ICityService _cityService;

        private readonly ILoggerService<AdminController> _loggerService;

        private readonly IUserManagerService _userManagerService;

        public AdminController(ILoggerService<AdminController> logger,
                                                    IUserManagerService userManagerService,
            IAdminService adminService,
            ICityService cityService,
            ICityParticipantsService cityAdministrationService)
        {
            _loggerService = logger;
            _userManagerService = userManagerService;
            _adminService = adminService;
            _cityService = cityService;
            _cityAdministrationService = cityAdministrationService;
        }

        /// <summary>
        /// Change current user role
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="role">The new current role of user</param>
        /// <response code="201">Successful operation</response>
        /// <response code="404">User not found</response>
        [HttpPut("changeRole/{userId}/{role}")]
        public async Task<IActionResult> ChangeCurrentUserRole(string userId, string role)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                await _adminService.ChangeCurrentRoleAsync(userId, role);
                _loggerService.LogInformation($"Successful change role for {userId}");
                return NoContent();
            }
            _loggerService.LogError("User id is null");
            return NotFound();
        }

        /// <summary>
        /// Change user role to expired
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">User not found</response>
        [HttpPut("changeRole/{userId}")]
        public async Task<IActionResult> ChangeUserRoleToExpired(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                await _adminService.ChangeAsync(userId);
                _loggerService.LogInformation($"Successful change role for {userId}");
                return NoContent();
            }
            _loggerService.LogError("User id is null");
            return NotFound();
        }

        /// <summary>
        /// Confirmation of delete a user
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>The id of the user</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">User id is null</response>
        [HttpGet("confirmDelete/{userId}")]
        [Authorize(Roles = Roles.Admin)]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _loggerService.LogError("User id is null");
                return BadRequest();
            }
            return Ok(userId);
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="userId">The id of the user, which must be deleted</param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">User id is null</response>
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("deleteUser/{userId}")]
        public async Task<IActionResult> Delete(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                await _adminService.DeleteUserAsync(userId);
                _loggerService.LogInformation($"Successful delete user {userId}");

                return Ok();
            }
            _loggerService.LogError("User id is null");
            return BadRequest();
        }

        /// <summary>
        /// Get specify model for edit roles for selected user
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <returns>A data of roles for editing user roles</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">User not found</response>
        [HttpGet("editRole/{userId}")]
        public async Task<IActionResult> Edit(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var user = await _userManagerService.FindByIdAsync(userId);
                if (user == null)
                {
                    _loggerService.LogError("User id is null");
                    return NotFound();
                }
                var userRoles = await _userManagerService.GetRolesAsync(user);
                var allRoles = _adminService.GetRolesExceptAdmin();

                RoleViewModel model = new RoleViewModel
                {
                    UserID = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };

                return Ok(model);
            }
            _loggerService.LogError("User id is null");
            return NotFound();
        }

        /// <summary>
        /// Edit user roles
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="roles">List of new user roles</param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">User not found</response>
        [HttpPut("editedRole/{userId}")]
        public async Task<IActionResult> Edit(string userId, [FromBody] List<string> roles)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                await _adminService.EditAsync(userId, roles);
                _loggerService.LogInformation($"Successful change role for {userId}");
                return Ok();
            }
            _loggerService.LogError("User id is null");
            return NotFound();
        }

        /// <summary>
        /// Get administration of selected city
        /// </summary>
        /// <returns>All administartion of selected city</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">City id is 0</response>
        [HttpGet("cityAdmins/{cityId}")]
        public async Task<IActionResult> GetAdmins(int cityId)
        {
            if (cityId != 0)
            {
                return Ok(await _cityAdministrationService.GetAdministrationByIdAsync(cityId));
            }
            _loggerService.LogError("City id is 0");
            return BadRequest();
        }

        /// <summary>
        /// Get City and Region Admins by userId of user which contained cityMembers
        /// </summary>
        /// <returns>User object and CityDTO, which contains CityAdministration, region => RegionAdministration</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="500">
        /// userId is empty/null or user not contained in database
        /// </response>
        [HttpGet("CityRegionAdmins/{userId}")]
        public async Task<IActionResult> GetCityAndRegionAdminsOfUser(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var user = await _userManagerService.FindByIdAsync(userId);
                if (user != null)
                {
                    var result = await _adminService.GetCityRegionAdminsOfUserAsync(userId);
                    return Ok(new { result, user });
                }
            }
            return BadRequest();
        }

        /// <summary>
        /// Get a specific number of users
        /// </summary>
        /// <param name="tableFilterParameters">Items to filter</param>
        /// <returns>A specific number of users</returns>
        [HttpGet("Profiles")]
        public async Task<IActionResult> GetUsersTable([FromQuery] TableFilterParameters tableFilterParameters)
        {
            var tuple = await _adminService.GetUsersTableAsync(tableFilterParameters);
            var users = tuple.Item1;
            var usersCount = tuple.Item2;

            return StatusCode(StatusCodes.Status200OK, new {users=users, total=usersCount});
        }

        /// <summary>
        /// Get all users with additional information
        /// </summary>
        /// <returns>Specify model with all users</returns>
        /// <response code="200">Successful operation</response>
        [HttpGet("usersTable")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _adminService.GetUsersAsync());
        }

        /// <summary>
        /// Get all users that match search string
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <returns>Short information about searched users</returns>
        /// <response code="200">Successful operation</response>
        [HttpGet("ShortUsersInfo/{searchString}")]
        public async Task<IActionResult> GetShortUsersInfo(string searchString)
        {
            return Ok(await _adminService.GetShortUserInfoAsync(searchString));
        }

        /// <summary>
        /// Get all cities
        /// </summary>
        /// <returns>All cities in specify model</returns>
        /// <response code="200">Successful operation</response>
        [HttpGet("regionsAdmins")]
        public async Task<IActionResult> RegionsAdmins()
        {
            var model = new CitiesAdminsViewModel()
            {
                Cities = await _cityService.GetAllCitiesAsync()
            };

            return Ok(model);
        }
    }
}
