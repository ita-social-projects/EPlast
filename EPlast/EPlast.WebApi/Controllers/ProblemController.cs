using EPlast.BLL.DTO.Account;
using EPlast.BLL.Interfaces;
using EPlast.BLL.Interfaces.Resources;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProblemController : ControllerBase
    {
        private readonly IHomeService _homeService;
        private readonly IResources _resources;
        private readonly UserManager<User> _userManager;
        private readonly IAuthService _authService;

        public ProblemController(
            IHomeService homeService,
            UserManager<User> userManager,
            IResources resources,
            IAuthService authService
            )
        {
            _homeService = homeService;
            _resources = resources;
            _userManager = userManager;
            _authService = authService;
        }

        /// <summary>
        /// Method for sending question to admin in system
        /// </summary>
        /// <param name="contactsDto">Contacts model(dto)</param>
        /// <returns>Answer from backend sending question to admin in system</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Problems with sending question</response>
        [HttpPost("sendQuestion")]
        public async Task<IActionResult> SendContacts([FromBody] ContactsDto contactsDto)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Дані введені неправильно");
                return BadRequest(_resources.ResourceForErrors["ModelIsNotValid"]);
            }
            await _homeService.SendEmailAdmin(contactsDto);

            return Ok(_resources.ResourceForErrors["Feedback-Sended"]);
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
                _authService.RefreshSignInAsync(userDto); //тут
                return Ok(_resources.ResourceForErrors["ChangePasswordConfirmation"]);
            }
            else
            {
                return BadRequest(_resources.ResourceForErrors["ModelIsNotValid"]);
            }
        }
    }
}
