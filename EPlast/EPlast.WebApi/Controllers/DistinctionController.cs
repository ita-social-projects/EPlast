using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL;
using EPlast.BLL.Interfaces.Logging;
using Microsoft.AspNetCore.Authorization;
using EPlast.DataAccess.Entities.UserEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistinctionController : ControllerBase
    {
        private readonly IDistinctionService _distinctionService;
        private readonly IUserDistinctionService _userDistinctionService;
        private readonly IMapper _mapper;
        private readonly ILoggerService<DistinctionController> _loggerService;


        public DistinctionController(IDistinctionService distinctionService, 
            IUserDistinctionService userDistinctionService,
            IMapper mapper, 
            ILoggerService<DistinctionController> loggerService)
        {
            _distinctionService = distinctionService;
            _userDistinctionService = userDistinctionService;
            _mapper = mapper;
            _loggerService = loggerService;
        }

        /// <summary>
        /// Returns the user distinction by id
        /// </summary>
        /// <param name="id">User distinction id</param>
        /// <returns> object</returns>
        /// <response code="200">An instance of user distinction</response>
        /// <response code="404">The user distinction does not exist</response>
        [HttpGet("UserDistinction/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserDistinction(int id)
        {
            UserDistinctionDTO userDistinction = await _userDistinctionService.GetUserDistinction(id);
            if (userDistinction == null)
                return NotFound();
            return Ok(userDistinction);
        }
        /// <summary>
        /// Returns all user distinctions
        /// </summary>
        /// <returns>All user distinctions</returns>
        /// <response code="200">Array of all user distinction</response>
        [HttpGet("UserDistinctions")]
        public async Task<IActionResult> GetUserDistinction()
        {
            IEnumerable<UserDistinctionDTO> userDistinctions = await _userDistinctionService.GetAllUsersDistinctionAsync();
            return Ok(userDistinctions);
        }
        /// <summary>
        /// Returns the distinction type by id
        /// </summary>
        /// <param name="id">Distinction type id</param>
        /// <returns> object</returns>
        /// <response code="200">An instance of distinction type</response>
        /// <response code="404">The distinction type with this id does not exist</response>
        [HttpGet("Distinction/{id:int}")]
        public async Task<IActionResult> GetDistinction(int id)
        {
            DistinctionDTO distinction = await _distinctionService.GetDistinctionAsync(id);
            if (distinction == null)
                return NotFound();
            return Ok(distinction);
        }
        /// <summary>
        /// Returns all distinction types
        /// </summary>
        /// <returns>All distinction types</returns>
        /// <response code="200">Array of all distinction types</response>
        [HttpGet("Distinctions")] 
        public async Task<IActionResult> GetDistinction()
        {
            IEnumerable<DistinctionDTO> distinctions = await _distinctionService.GetAllDistinctionAsync();
            return Ok(distinctions);
        }
        /// <summary>
        /// Delete distinction type by id
        /// </summary>
        /// <param name="id">Distinction type id</param>
        /// <returns> Answer from backend </returns>
        /// <response code="200">Distinction type was successfully deleted</response>
        /// <response code="404">Distinction type does not exist</response>
        [HttpDelete("Delete/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDistinction(int id)
        {
            try
            {
                await _distinctionService.DeleteDistinction(id, User);
                return Ok();
            }
            catch (NullReferenceException) 
            {
                return NotFound();
            }
        }
        /// <summary>
        /// Delete user distinction by id
        /// </summary>
        /// <param name="id">User distinction id</param>
        /// <returns> Answer from backend </returns>
        /// <response code="200">User distinction was successfully deleted</response>
        /// <response code="404">User distinction does not exist</response>
        [HttpDelete("Delete/Distinction/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserDistinction(int id)
        {
            try
            {
                await _userDistinctionService.DeleteUserDistinction(id, User);
                return Ok();
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }
        /// <summary>
        /// Add user distinction
        /// </summary>
        /// <param name="userDistinctionDTO">User Distinction model</param>
        /// <returns> Answer from backend </returns>
        /// <response code="200">User distinction was successfully created</response>
        /// <response code="404">User does not exist</response>
        /// <response code="400">Model is not valid</response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUserDistinction(UserDistinctionDTO userDistinctionDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _userDistinctionService.AddUserDistinction(userDistinctionDTO, User);
                    return Ok();
                }
                catch (NullReferenceException)
                {
                    return NotFound();
                }
            }
            return BadRequest(ModelState);
        }
        /// <summary>
        /// Add distinction type
        /// </summary>
        /// <param name="distinctionDTO">Distinction model</param>
        /// <returns> Answer from backend </returns>
        /// <response code="200">Distinction type was successfully created</response>
        /// <response code="400">Model is not valid</response>
        [HttpPost("Distinction/Create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddDistinction(DistinctionDTO distinctionDTO)
        {
            if (ModelState.IsValid)
            {
                await _distinctionService.AddDistinction(distinctionDTO, User);
                return Ok();
            }
            return BadRequest(ModelState);
        }
        /// <summary>
        /// Edit user distinction
        /// </summary>
        /// <param name="userDistinctionDTO">User Distinction model</param>
        /// <returns> Answer from backend </returns>
        /// <response code="200">User distinction was successfully edited</response>
        /// <response code="404">User distinction does not exist</response>
        /// <response code="400">Model is not valid</response>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUserDistinction(UserDistinctionDTO userDistinctionDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _userDistinctionService.ChangeUserDistinction(userDistinctionDTO, User);
                    return Ok();
                }
                catch (NullReferenceException)
                {
                    return NotFound();
                }
            }
            return BadRequest(ModelState);
        }
        /// <summary>
        /// Edit distinction type
        /// </summary>
        /// <param name="distinctionDTO">Distinction type model</param>
        /// <returns> Answer from backend </returns>
        /// <response code="200">Distinction type was successfully edited</response>
        /// <response code="404">Distinction type does not exist</response>
        /// <response code="400">Model is not valid</response>
        [HttpPut("Distinction/Edit")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditDistinction(DistinctionDTO distinctionDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _distinctionService.ChangeDistinction(distinctionDTO, User);
                    return Ok();
                }
                catch (NullReferenceException)
                {
                    return NotFound();
                }
            }
            return BadRequest(ModelState);
        }
        /// <summary>
        /// Get distinction of user with this id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns> All distinctions of user </returns>
        /// <response code="200"> Array of user distinctions</response>
        /// <response code="404">User does not exist</response>
        [HttpGet("User/Distinctions/{id:string}")]
        public async Task<IActionResult> GetDistinctionOfGivenUser(string id)
        {
            var userDistinctions = await _userDistinctionService.GetUserDistinctionsOfGivenUser(id);
            if (userDistinctions == null)
                return NotFound();
            return Ok(userDistinctions);
        }
    }
}