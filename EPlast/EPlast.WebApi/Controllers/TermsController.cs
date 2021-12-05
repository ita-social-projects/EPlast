﻿using EPlast.BLL.DTO.Terms;
using EPlast.BLL.Interfaces.Terms;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using EPlast.Resources;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Roles=Roles.Admin)]
    
    public class TermsController : ControllerBase
    {
        private readonly ITermsService _termsOfUse;
        private readonly UserManager<User> _userManager;

        public TermsController(ITermsService termsOfUse, UserManager<User> userManager)
        {
            _termsOfUse = termsOfUse;
            _userManager = userManager;
        }

        /// <summary>
        /// Get first record from database
        /// </summary>
        /// <returns>First record</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">User not found</response>
        [HttpGet("Data")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFirstTermsOfUse()
        {
            TermsDTO termsDTO = await _termsOfUse.GetFirstRecordAsync();
            if (termsDTO == null)
                return NotFound();
            return Ok(termsDTO); 
        }

        /// <summary>
        /// Get all users id
        /// </summary>
        /// <returns>All users id</returns>
        /// <response code="200">Successful operation</response>
        [HttpGet("UsersId")]
        public async Task<IActionResult> GetAllUsersId()
        {
            var usersId = await _termsOfUse.GetAllUsersIdAsync(await _userManager.GetUserAsync(User));
            return Ok(usersId);
        }

        /// <summary>
        /// Edit current terms of use
        /// </summary>
        /// <param name="termsDTO">The id of the user</param>
        /// <returns>Answer from backend</returns>
        /// <response code="204">An instance of decision was created</response>
        /// <response code="404">User not found</response>
        /// <response code="400">The id and decision id are not same</response>
        [HttpPut("Data/{id}")]
        public async Task<IActionResult> EditTerms(TermsDTO termsDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _termsOfUse.ChangeTermsAsync(termsDTO, await _userManager.GetUserAsync(User));
                    return NoContent();
                }
                catch (NullReferenceException)
                {
                    return NotFound();
                }
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Creates new terms by id
        /// </summary>
        /// <param name="termsDTO">Decision wrapper</param>
        /// <returns>Answer from backend</returns>
        /// <response code="204">An instance of decision was created</response>
        /// <response code="404">User not found</response>
        /// <response code="400">The id and decision id are not same</response>
        [HttpPost("Data/{id}")]
        public async Task<IActionResult> AddTerms(TermsDTO termsDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _termsOfUse.AddTermsAsync(termsDTO, await _userManager.GetUserAsync(User));
                    return NoContent();
                }
                catch
                {
                    return NotFound();
                }
            }
            return BadRequest(ModelState);
        }
    } 
}