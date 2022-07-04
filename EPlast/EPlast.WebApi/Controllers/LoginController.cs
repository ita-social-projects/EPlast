using System;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Account;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.ActiveMembership;
using EPlast.BLL.Interfaces.Jwt;
using EPlast.BLL.Interfaces.Logging;
using EPlast.BLL.Interfaces.Resources;
using EPlast.BLL.Models;
using EPlast.BLL.Services.Interfaces;
using EPlast.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog.Extensions.Logging;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;
        private readonly ILoggerService _loggerService;
        private readonly IResources _resources;
        private readonly IUserDatesService _userDatesService;
        private readonly IUserManagerService _userManagerService;

        public LoginController(
            IAuthService authService,
            IResources resources,
            IJwtService jwtService,
            ILoggerService loggerService,
            IUserDatesService userDatesService,
            IUserManagerService userManagerService
            )
        {
            _authService = authService;
            _resources = resources;
            _jwtService = jwtService;
            _loggerService = loggerService;
            _userDatesService = userDatesService;
            _userManagerService = userManagerService;
        }

        [HttpPost("signin/facebook")]
        [AllowAnonymous]
        public async Task<IActionResult> FacebookLogin([FromBody] FacebookUserInfo userInfo)
        {
            try
            {
                string newGender = _resources.ResourceForGender[userInfo.Gender].Value;
                userInfo.Gender = newGender;
                var user = await _authService.FacebookLoginAsync(userInfo);
                if (user == null)
                {
                    return BadRequest();
                }

                if (await _userManagerService.IsInRoleAsync(user, Roles.FormerPlastMember))
                {
                    return BadRequest(_resources.ResourceForErrors["User-FormerMember"]);
                }

                await AddEntryMembershipDateAsync(user.Id);

                var generatedToken = await _jwtService.GenerateJWTTokenAsync(user);
                return Ok(new { token = generatedToken });
            }
            catch (Exception exc)
            {
                _loggerService.LogError(exc.Message);
            }

            return BadRequest();
        }

        [HttpGet("FacebookAppId")]
        [AllowAnonymous]
        public IActionResult GetFacebookAppId()
        {
            return Ok(new { id = ConfigSettingLayoutRenderer.DefaultConfiguration.GetSection("FacebookAuthentication")["FacebookAppId"] });
        }

        [HttpGet("GoogleClientId")]
        [AllowAnonymous]
        public IActionResult GetGoogleClientId()
        {
            return Ok(new { id = ConfigSettingLayoutRenderer.DefaultConfiguration.GetSection("GoogleAuthentication")["GoogleClientId"] });
        }

        [HttpPost("signin/google")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleLogin(string googleToken)
        {
            try
            {
                var user = await _authService.GetGoogleUserAsync(googleToken);
                if (user == null)
                {
                    return BadRequest();
                }

                if (await _userManagerService.IsInRoleAsync(user, Roles.FormerPlastMember))
                {
                    return BadRequest(_resources.ResourceForErrors["User-FormerMember"]);
                }

                await AddEntryMembershipDateAsync(user.Id);
                var generatedToken = await _jwtService.GenerateJWTTokenAsync(user);

                return Ok(new { token = generatedToken });
            }
            catch (Exception exc)
            {
                _loggerService.LogError(exc.Message);
            }

            return BadRequest();
        }

        /// <summary>
        /// Method for logining in system
        /// </summary>
        /// <param name="loginDto">Login model(dto)</param>
        /// <returns>Answer from backend for login method</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Problems with logining</response>
        [HttpPost("signin")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _authService.FindByEmailAsync(loginDto.Email);
                if (user == null)
                {
                    return BadRequest(_resources.ResourceForErrors["Login-NotRegistered"]);
                }
                else
                {
                    if (!await _authService.IsEmailConfirmedAsync(user))
                    {
                        return BadRequest(_resources.ResourceForErrors["Login-NotConfirmed"]);
                    }

                    if (await _userManagerService.IsInRoleAsync(user, Roles.FormerPlastMember))
                    {
                        return BadRequest(_resources.ResourceForErrors["User-FormerMember"]);
                    }
                }
                var result = await _authService.SignInAsync(loginDto);
                if (result.IsLockedOut)
                {
                    return BadRequest(_resources.ResourceForErrors["Account-Locked"]);
                }
                if (result.Succeeded)
                {
                    var generatedToken = await _jwtService.GenerateJWTTokenAsync(user);
                    return Ok(new { token = generatedToken });
                }
                else
                {
                    return BadRequest(_resources.ResourceForErrors["Login-InCorrectPassword"]);
                }
            }
            return Ok(_resources.ResourceForErrors["ModelIsNotValid"]);
        }

        [HttpGet("logout")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult Logout()
        {
            _authService.SignOutAsync();
            return Ok();
        }

        private async Task AddEntryMembershipDateAsync(string userId)
        {
            if (!(await _userDatesService.UserHasMembership(userId)))
            {
                await _userDatesService.AddDateEntryAsync(userId);
            }
        }
    }
}
