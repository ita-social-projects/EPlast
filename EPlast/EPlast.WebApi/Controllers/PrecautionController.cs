﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Roles = "Admin, Голова Округу, Голова Станиці, Голова Куреня, Пластун, Прихильник")]

    public class PrecautionController : ControllerBase
    {
        private readonly IPrecautionService _precautionService;
        private readonly IUserPrecautionService _userPrecautionService;
        private readonly UserManager<User> _userManager;


        public PrecautionController(
            IPrecautionService PrecautionService,
            IUserPrecautionService userPrecautionService,
            UserManager<User> userManager)
        {
            _precautionService = PrecautionService;
            _userPrecautionService = userPrecautionService;
            _userManager = userManager;
        }

        /// <summary>
        /// Returns the user Precaution by id
        /// </summary>
        /// <param name="id">User Precaution id</param>
        /// <returns> object</returns>
        /// <response code="200">An instance of user Precaution</response>
        /// <response code="404">The user Precaution does not exist</response>
        [HttpGet("UserPrecaution/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserPrecaution(int id)
        {
            UserPrecautionDTO userPrecaution = await _userPrecautionService.GetUserPrecautionAsync(id);
            if (userPrecaution == null)
                return NotFound();
            return Ok(userPrecaution);
        }
        /// <summary>
        /// Returns all user Precautions
        /// </summary>
        /// <returns>All user Precautions</returns>
        /// <response code="200">Array of all user Precaution</response>
        [HttpGet("UserPrecautions")]
        public async Task<IActionResult> GetUserPrecaution()
        {
            IEnumerable<UserPrecautionDTO> userPrecautions = await _userPrecautionService.GetAllUsersPrecautionAsync();
            return Ok(userPrecautions);
        }
        /// <summary>
        /// Returns the Precaution type by id
        /// </summary>
        /// <param name="id">Precaution type id</param>
        /// <returns> object</returns>
        /// <response code="200">An instance of Precaution type</response>
        /// <response code="404">The Precaution type with this id does not exist</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPrecaution(int id)
        {
            PrecautionDTO Precaution = await _precautionService.GetPrecautionAsync(id);
            if (Precaution == null)
                return NotFound();
            return Ok(Precaution);
        }
        /// <summary>
        /// Returns all Precaution types
        /// </summary>
        /// <returns>All Precaution types</returns>
        /// <response code="200">Array of all Precaution types</response>
        [HttpGet("Precautions")]
        public async Task<IActionResult> GetPrecaution()
        {
             return Ok(await _precautionService.GetAllPrecautionAsync());
        }

        /// <summary>
        /// Get Precaution of user with this id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns> All Precautions of user </returns>
        /// <response code="200"> Array of user Precautions</response>
        /// <response code="404">User does not exist</response>
        [HttpGet("User/Precautions/{id}")]
        public async Task<IActionResult> GetPrecautionOfGivenUser(string id)
        {
            var userPrecautions = await _userPrecautionService.GetUserPrecautionsOfUserAsync(id);
            if (userPrecautions == null)
                return NotFound();
            return Ok(userPrecautions);
        }
        /// <summary>
        /// Delete Precaution type by id
        /// </summary>
        /// <param name="id">Precaution type id</param>
        /// <returns> Answer from backend </returns>
        /// <response code="204">Precaution type was successfully deleted</response>
        /// <response code="404">Precaution type does not exist</response>
        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePrecaution(int id)
        {
            try
            {
                await _precautionService.DeletePrecautionAsync(id, await _userManager.GetUserAsync(User));
                return NoContent();
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }
        /// <summary>
        /// Delete user Precaution by id
        /// </summary>
        /// <param name="id">User Precaution id</param>
        /// <returns> Answer from backend </returns>
        /// <response code="204">User Precaution was successfully deleted</response>
        /// <response code="404">User Precaution does not exist</response>
        [HttpDelete("UserPrecaution/Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserPrecaution(int id)
        {
            try
            {
                await _userPrecautionService.DeleteUserPrecautionAsync(id, await _userManager.GetUserAsync(User));
                return NoContent();
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }
        /// <summary>
        /// Add user Precaution
        /// </summary>
        /// <param name="userPrecautionDTO">User Precaution model</param>
        /// <returns> Answer from backend </returns>
        /// <response code="204">User Precaution was successfully created</response>
        /// <response code="404">User does not exist</response>
        /// <response code="400">Model is not valid</response>
        [HttpPost("UserPrecaution/Create/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUserPrecaution(UserPrecautionDTO userPrecautionDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _userPrecautionService.AddUserPrecautionAsync(userPrecautionDTO, await _userManager.GetUserAsync(User));
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
        /// Add Precaution type
        /// </summary>
        /// <param name="PrecautionDTO">Precaution model</param>
        /// <returns> Answer from backend </returns>
        /// <response code="204">Precaution type was successfully created</response>
        /// <response code="400">Model is not valid</response>
        [HttpPost("Create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddPrecaution(PrecautionDTO PrecautionDTO)
        {
            if (ModelState.IsValid)
            {
                await _precautionService.AddPrecautionAsync(PrecautionDTO, await _userManager.GetUserAsync(User));
                return NoContent();
            }
            return BadRequest(ModelState);
        }
        /// <summary>
        /// Edit user Precaution
        /// </summary>
        /// <param name="userPrecautionDTO">User Precaution model</param>
        /// <returns> Answer from backend </returns>
        /// <response code="204">User Precaution was successfully edited</response>
        /// <response code="404">User Precaution does not exist</response>
        /// <response code="400">Model is not valid</response>
        [HttpPut("UserPrecaution/Edit/{userPrecautionId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUserPrecaution(UserPrecautionDTO userPrecautionDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _userPrecautionService.ChangeUserPrecautionAsync(userPrecautionDTO, await _userManager.GetUserAsync(User));
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
        /// Edit Precaution type
        /// </summary>
        /// <param name="PrecautionDTO">Precaution type model</param>
        /// <returns> Answer from backend </returns>
        /// <response code="204">Precaution type was successfully edited</response>
        /// <response code="404">Precaution type does not exist</response>
        /// <response code="400">Model is not valid</response>
        [HttpPut("Edit/{PrecautionId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditPrecaution(PrecautionDTO PrecautionDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _precautionService.ChangePrecautionAsync(PrecautionDTO, await _userManager.GetUserAsync(User));
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
        /// Checks if theres already a Precaution with such number
        /// </summary>
        /// <param name="number">Number which checking</param>
        /// <returns>True if exist</returns>
        /// <returns>False if doesn't exist</returns>
        /// <response code="200">Check was successfull</response>
        [HttpGet("numberExist/{number}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CheckNumberExisting(int number)
        {
            bool distNumber = await _userPrecautionService.IsNumberExistAsync(number);
            return Ok(distNumber);
        }
    }
}
