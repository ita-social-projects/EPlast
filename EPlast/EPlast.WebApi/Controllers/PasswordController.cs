using EPlast.BLL.DTO.Account;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.Resources;
using EPlast.DataAccess.Entities;
using EPlast.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Web;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private readonly IAuthEmailService _authEmailServices;

        private readonly IAuthService _authService;

        private readonly IResources _resources;

        private readonly UserManager<User> _userManager;

        public PasswordController(
                                            IAuthService authService,
            IResources resources,
            IAuthEmailService authEmailService,
            UserManager<User> userManager
            )
        {
            _authService = authService;
            _resources = resources;
            _authEmailServices = authEmailService;
            _userManager = userManager;
        }

        /// <summary>
        /// Method for changing password in system
        /// </summary>
        /// <param name="changepasswordDto">ChangePassword model(dto)</param>
        /// <returns>Answer from backend for changing password method</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Problems with changing password</response>
        [HttpPost("changePassword")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changepasswordDto)
        {
            if (ModelState.IsValid)
            {
                var userDto = _authService.GetUser(await _userManager.GetUserAsync(User));
                if (userDto == null)
                {
                    return BadRequest();
                }
                var result = await _authService.ChangePasswordAsync(userDto.Id, changepasswordDto);
                if (!result.Succeeded)
                {
                    return BadRequest(_resources.ResourceForErrors["Change-PasswordProblems"]);
                }
                var refreshResult = await _authService.RefreshSignInAsync(userDto);
                if (refreshResult)
                    return Ok(_resources.ResourceForErrors["ChangePasswordConfirmation"]);
                else
                    return BadRequest();
            }
            else
            {
                return BadRequest(_resources.ResourceForErrors["ModelIsNotValid"]);
            }
        }

        /// <summary>
        /// Method for forgotting password in system
        /// </summary>
        /// <param name="forgotpasswordDto">Forgot Password model(dto)</param>
        /// <returns>Answer from backend for forgoting password email method</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Problems with forgotting password</response>
        [HttpPost("forgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotpasswordDto)
        {
            if (ModelState.IsValid)
            {
                var userDto = await _authService.FindByEmailAsync(forgotpasswordDto.Email);
                if (userDto == null || !(await _authService.IsEmailConfirmedAsync(userDto)))
                {
                    return BadRequest(_resources.ResourceForErrors["Forgot-NotRegisteredUser"]);
                }
                string token = await _authService.GenerateResetTokenAsync(userDto);
                var confirmationLink = Request.GetFrontEndResetPasswordURL(token);
                await _authEmailServices.SendEmailResetingAsync(confirmationLink, forgotpasswordDto);
                return Ok(_resources.ResourceForErrors["ForgotPasswordConfirmation"]);
            }
            return BadRequest(_resources.ResourceForErrors["ModelIsNotValid"]);
        }

        /// <summary>
        /// Method for resetting password in system
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <param name="token">Token for reseting password</param>
        /// <returns>Answer from backend for resetting password method</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Problems with resetting password</response>
        [HttpGet("resetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromQuery(Name = "userId")] string userId, [FromQuery(Name = "token")] string token)
        {
            var userDto = await _authService.FindByIdAsync(userId);
            var model = new ResetPasswordDto { Code = token, Email = userDto.Email };
            int totalTime = _authService.GetTimeAfterReset(userDto);
            if (totalTime < 180)
            {
                if (string.IsNullOrWhiteSpace(token))
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(model);
                }
            }
            else
            {
                return Ok(_resources.ResourceForErrors["ResetPasswordNotAllowed"]);
            }
        }

        /// <summary>
        /// Method for resetting password in system
        /// </summary>
        /// <param name="resetpasswordDto">ResetPassword model(dto)</param>
        /// <returns>Answer from backend for resetting password method</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Problems with resetting password</response>
        [HttpPost("resetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetpasswordDto) //+
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(_resources.ResourceForErrors["ModelIsNotValid"]);
            }
            var userDto = await _authService.FindByEmailAsync(resetpasswordDto.Email);
            if (userDto == null)
            {
                return BadRequest(_resources.ResourceForErrors["Reset-NotRegisteredUser"]);
            }
            var result = await _authService.ResetPasswordAsync(userDto.Id, resetpasswordDto);
            if (result.Succeeded)
            {
                await _authService.CheckingForLocking(userDto);
                return Ok(_resources.ResourceForErrors["ResetPasswordConfirmation"]);
            }
            else
            {
                return BadRequest(_resources.ResourceForErrors["Reset-PasswordProblems"]);
            }
        }
    }
}
