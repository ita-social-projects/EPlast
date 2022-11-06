using System.Threading.Tasks;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Interfaces.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Roles = Roles.AdminRegionBoardHeadOkrugaCityHeadAndDeputy)]
    public class UserRenewalController : ControllerBase
    {
        private readonly IUserRenewalService _userRenewalService;
        private readonly UserManager<User> _userManager;

        public UserRenewalController(
            IUserRenewalService userRenewalService,
            UserManager<User> userManager)
        {
            _userRenewalService = userRenewalService;
            _userManager = userManager;
        }

        /// <summary>
        /// Get all UserRenewals
        /// </summary>
        /// <param name="searchedData">Searched Data</param>
        /// <param name="page">Current page on pagination</param>
        /// <param name="pageSize">Number of records per page</param>
        ///  /// <param name="filter">Number of records per page</param>
        /// <returns>List of UserRenewalTableObject</returns>
        /// <response code="200">Successful operation</response>
        [HttpGet("UserRenewalsForTable")]
        public IActionResult GetUserRenewalsForTable(string searchedData, int page, int pageSize, string filter)
        {
            var userRenewals = _userRenewalService.GetUserRenewalsTableObject(searchedData, page, pageSize, filter);
            return Ok(userRenewals);
        }

        /// <summary>
        /// Check if user is Former-Member
        /// </summary>
        /// <param name="email">Email of Former-Member</param>
        /// <returns>Former-Member Id to make the renewal request</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">NotFound</response>
        [HttpPost("FormerCheck/{email}")]
        [AllowAnonymous]
        public async Task<IActionResult> FormerCheck(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null && await _userManager.IsInRoleAsync(user, Roles.FormerPlastMember))
            {
                return Ok(user.Id);
            }
            return BadRequest("Not Former-Member");
        }

        /// <summary>
        /// Create UserRenewal request
        /// </summary>
        /// <param name="userRenewal">UserRenewal</param>
        /// <response code="204">Successful operation</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">NotFound</response>
        [HttpPost("CreateRenewal")]
        [AllowAnonymous]
        public async Task<IActionResult> AddUserRenewalAsync(UserRenewalDto userRenewal)
        {
            if (!ModelState.IsValid || !await _userRenewalService.IsValidUserRenewalAsync(userRenewal))
                return BadRequest("Cannot create renewal");
            try
            {
                await _userRenewalService.AddUserRenewalAsync(userRenewal);
                return NoContent();
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Renew Former-Member
        /// </summary>
        /// <param name="userRenewal">UserRenewal</param>
        /// <returns>Renewed user info as a CityMember</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        [HttpPut("RenewUser")]
        public async Task<IActionResult> RenewUser(UserRenewalDto userRenewal)
        {
            var userId = userRenewal.UserId;
            var cityId = userRenewal.CityId;
            var admin = await _userManager.GetUserAsync(User);
            var user = await _userManager.FindByIdAsync(userId);

            if (!ModelState.IsValid || user == null || !await _userManager.IsInRoleAsync(user, Roles.FormerPlastMember))
                return BadRequest("Unable to apply the renewal");

            if (!await _userRenewalService.IsValidAdminAsync(admin, cityId))
                return Unauthorized();
            try
            {
                var newUser = await _userRenewalService.RenewFormerMemberUserAsync(userRenewal);
                await _userRenewalService.SendRenewalConfirmationEmailAsync(userId, cityId);
                return Ok(newUser);
            }
            catch
            {
                return BadRequest("Error handling the request status");
            }
        }
    }
}