using EPlast.BLL;
using EPlast.BLL.DTO.Distinction;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using EPlast.BLL.Queries.Distinction;
using EPlast.BLL.Commands.Distinction;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Roles = Roles.HeadsAndHeadDeputiesAndAdminPlastunSupporterAndRegisteredUser)]

    public class DistinctionController : ControllerBase
    {
        private readonly IUserDistinctionService _userDistinctionService;
        private readonly UserManager<User> _userManager;
        private readonly IMediator _mediator;

        public DistinctionController( 
            IUserDistinctionService userDistinctionService,
            UserManager<User> userManager,
            IMediator mediator)
        {
            _userDistinctionService = userDistinctionService;
            _userManager = userManager;
            _mediator = mediator;
        }

        /// <summary>
        /// Returns the user distinction by id
        /// </summary>
        /// <param name="id">User distinction id</param>
        /// <returns> object</returns>
        /// <response code="200">An instance of user distinction</response>
        /// <response code="404">The user distinction does not exist</response>
        [HttpGet("UserDistinction/{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetUserDistinction(int id)
        {
            UserDistinctionDTO userDistinction = await _userDistinctionService.GetUserDistinctionAsync(id);
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
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.HeadsAndHeadDeputiesAndAdminPlastunAndSupporter)]
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDistinction(int id)
        {
            var query = new GetDistinctionQuery(id);
            var distinction = await _mediator.Send(query);
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
            var query = new GetAllDistinctionQuery();
            var distinctions = await _mediator.Send(query);
            return Ok(distinctions);
        }

        /// <summary>
        /// Get distinction of user with this id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns> All distinctions of user </returns>
        /// <response code="200"> Array of user distinctions</response>
        /// <response code="404">User does not exist</response>
        [HttpGet("User/Distinctions/{id}")]
        public async Task<IActionResult> GetDistinctionOfGivenUser(string id)
        {
            var userDistinctions = await _userDistinctionService.GetUserDistinctionsOfUserAsync(id);            
            if (userDistinctions == null)
                return NotFound();
            return Ok(userDistinctions);
        }

        /// <summary>
        /// Get all Users Distinctions
        /// </summary>
        /// <returns>List of UserDistinctionsTableObject</returns>
        /// <response code="200">Successful operation</response>
        [HttpGet("UsersDistinctionsForTable")]
        public async Task<IActionResult> GetUsersDistinctionsForTable([FromQuery] DistictionTableSettings tableSettings)
        {
            var query = new GetUsersDistinctionsForTableQuery(tableSettings);
            var distinctionsTuple = await _mediator.Send(query);
            var allInfoDistinctions = distinctionsTuple.Item1.ToList();
            allInfoDistinctions.ForEach(u => u.Total = distinctionsTuple.Item2);
            return Ok(allInfoDistinctions);            
        }

        /// <summary>
        /// Delete distinction type by id
        /// </summary>
        /// <param name="id">Distinction type id</param>
        /// <returns> Answer from backend </returns>
        /// <response code="204">Distinction type was successfully deleted</response>
        /// <response code="404">Distinction type does not exist</response>
        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DeleteDistinction(int id)
        {
            try
            {
                var query = new DeleteDistinctionCommand(id, await _userManager.GetUserAsync(User));
                await _mediator.Send(query);
                return NoContent();
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
        /// <response code="204">User distinction was successfully deleted</response>
        /// <response code="404">User distinction does not exist</response>
        [HttpDelete("UserDistinction/Delete/{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DeleteUserDistinction(int id)
        {
            try
            {
                await _userDistinctionService.DeleteUserDistinctionAsync(id, await _userManager.GetUserAsync(User));
                return NoContent();
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
        /// <response code="204">User distinction was successfully created</response>
        /// <response code="404">User does not exist</response>
        /// <response code="400">Model is not valid</response>
        [HttpPost("UserDistinction/Create/{userId}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> AddUserDistinction(UserDistinctionDTO userDistinctionDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _userDistinctionService.AddUserDistinctionAsync(userDistinctionDTO, await _userManager.GetUserAsync(User));
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
        /// Add distinction type
        /// </summary>
        /// <param name="distinctionDTO">Distinction model</param>
        /// <returns> Answer from backend </returns>
        /// <response code="204">Distinction type was successfully created</response>
        /// <response code="400">Model is not valid</response>
        [HttpPost("Create")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> AddDistinction(DistinctionDTO distinctionDTO)
        {
            if (ModelState.IsValid)
            {
                var query = new AddDistinctionCommand(distinctionDTO, await _userManager.GetUserAsync(User));
                await _mediator.Send(query);
                return NoContent();
            }
            return BadRequest(ModelState);
        }
        /// <summary>
        /// Edit user distinction
        /// </summary>
        /// <param name="userDistinctionDTO">User Distinction model</param>
        /// <returns> Answer from backend </returns>
        /// <response code="204">User distinction was successfully edited</response>
        /// <response code="404">User distinction does not exist</response>
        /// <response code="400">Model is not valid</response>
        [HttpPut("UserDistinction/Edit/{userDistinctionId}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> EditUserDistinction(UserDistinctionDTO userDistinctionDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _userDistinctionService.ChangeUserDistinctionAsync(userDistinctionDTO, await _userManager.GetUserAsync(User));
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
        /// Edit distinction type
        /// </summary>
        /// <param name="distinctionDTO">Distinction type model</param>
        /// <returns> Answer from backend </returns>
        /// <response code="204">Distinction type was successfully edited</response>
        /// <response code="404">Distinction type does not exist</response>
        /// <response code="400">Model is not valid</response>
        [HttpPut("Edit/{distinctionId}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> EditDistinction(DistinctionDTO distinctionDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var query = new ChangeDistinctionCommand(distinctionDTO, await _userManager.GetUserAsync(User));
                    await _mediator.Send(query);
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
        /// Checks if theres already a Distinction with such number
        /// </summary>
        /// <param name="number">Number which checking</param>
        /// <returns>True if exist</returns>
        /// <returns>False if doesn't exist</returns>
        /// <response code="200">Check was successfull</response>
        [HttpGet("numberExist/{number}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> CheckNumberExisting(int number)
        {
            bool distNumber = await _userDistinctionService.IsNumberExistAsync(number);
            return Ok(distNumber);
        }

    }
}