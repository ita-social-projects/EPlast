using EPlast.BLL;
using EPlast.BLL.Commands.Precaution;
using EPlast.BLL.DTO.PrecautionsDTO;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.BLL.Queries.Precaution;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using EPlast.WebApi.Models.Precaution;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Roles = Roles.HeadsAndHeadDeputiesAndAdminPlastunSupporterAndRegisteredUser)]

    public class PrecautionController : ControllerBase
    {
        private readonly IUserPrecautionService _userPrecautionService;
        private readonly UserManager<User> _userManager;
        private readonly IMediator _mediator;

        public PrecautionController(
            IUserPrecautionService userPrecautionService,
            UserManager<User> userManager,
            IMediator mediator)
        {
            _userPrecautionService = userPrecautionService;
            _userManager = userManager;
            _mediator = mediator;
        }

        /// <summary>
        /// Returns the user Precaution by id
        /// </summary>
        /// <param name="id">User Precaution id</param>
        /// <returns> object</returns>
        /// <response code="200">An instance of user Precaution</response>
        /// <response code="404">The user Precaution does not exist</response>
        [HttpGet("UserPrecaution/{id}")]
        [Authorize(Roles = Roles.AdminAndGBAdmin)]
        public async Task<IActionResult> GetUserPrecaution(int id)
        {
            var userPrecaution = await _userPrecautionService.GetUserPrecautionAsync(id);
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
            IEnumerable<UserPrecautionDto> userPrecautions = await _userPrecautionService.GetAllUsersPrecautionAsync();
            return Ok(userPrecautions);
        }

        /// <summary>
        /// Get all Users Precautions
        /// </summary>
        /// <param name="tableSettings">data of table filters, page, search data, page size</param>
        /// <returns>List of UserPrecautionsTableObject</returns>
        /// <response code="200">Successful operation</response>
        [HttpGet("UsersPrecautionsForTable")]
        [Authorize(Roles = Roles.AdminPlastMemberAndSupporter)]
        public async Task<IActionResult> GetUsersPrecautionsForTable([FromQuery] PrecautionTableSettings tableSettings)
        {
            var tableInfo =
                await _userPrecautionService.GetUserPrecautionsForTableAsync(tableSettings);
            return Ok(tableInfo);
        }

        /// <summary>
        /// Returns the Precaution type by id
        /// </summary>
        /// <param name="id">Precaution type id</param>
        /// <returns> object</returns>
        /// <response code="200">An instance of Precaution type</response>
        /// <response code="404">The Precaution type with this id does not exist</response>
        [HttpGet("{id}")]
        [Authorize(Roles = Roles.AdminPlastMemberAndSupporter)]
        public async Task<IActionResult> GetPrecaution(int id)
        {
            var query = new GetPrecautionQuery(id);
            var Precaution = await _mediator.Send(query);
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
        [Authorize(Roles = Roles.AdminPlastMemberAndSupporter)]
        public async Task<IActionResult> GetPrecaution()
        {
            var query = new GetAllPrecautionQuery();
            return Ok(await _mediator.Send(query));
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
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DeletePrecaution(int id)
        {
            try
            {
                var query = new DeletePrecautionCommand(id, await _userManager.GetUserAsync(User));
                await _mediator.Send(query);
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
        [Authorize(Roles = Roles.AdminAndGBAdmin)]
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
        /// <param name="userPrecaution">User Precaution model</param>
        /// <returns> Answer from backend </returns>
        /// <response code="204">User Precaution was successfully created</response>
        /// <response code="404">User does not exist</response>
        /// <response code="400">Model is not valid</response>
        [HttpPost("UserPrecaution/Create")]
        [Authorize(Roles = Roles.AdminAndGBAdmin)]
        public async Task<IActionResult> AddUserPrecaution(UserPrecautionCreateViewModel userPrecaution)
        {
            if (ModelState.IsValid)
            {
                var precaution = await _mediator.Send(new GetPrecautionQuery(userPrecaution.PrecautionId));
                var user = await _userManager.GetUserAsync(User);

                bool isAdded =
                    await _userPrecautionService.AddUserPrecautionAsync(new UserPrecautionDto
                    {
                        Precaution = precaution,
                        PrecautionId = userPrecaution.PrecautionId,
                        Reporter = userPrecaution.Reporter,
                        Reason = userPrecaution.Reason,
                        Status = userPrecaution.Status,
                        Number = userPrecaution.Number,
                        Date = userPrecaution.Date,
                        UserId = userPrecaution.UserId,
                    }, user);

                if (!isAdded)
                {
                    return BadRequest();
                }

                return NoContent();
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
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> AddPrecaution(PrecautionDto PrecautionDTO)
        {
            if (ModelState.IsValid)
            {
                var query = new AddPrecautionCommand(PrecautionDTO, await _userManager.GetUserAsync(User));
                await _mediator.Send(query);
                return NoContent();
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Edit user Precaution
        /// </summary>
        /// <param name="model">User Precaution model</param>
        /// <returns> Answer from backend </returns>
        /// <response code="204">User Precaution was successfully edited</response>
        /// <response code="404">User Precaution does not exist</response>
        /// <response code="400">Model is not valid</response>
        [HttpPut("UserPrecaution/Edit")]
        [Authorize(Roles = Roles.AdminAndGBAdmin)]
        public async Task<IActionResult> EditUserPrecaution(UserPrecautionEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.UserId);

                var userPrecautionDTO = new UserPrecautionDto
                {
                    Id = model.Id,
                    PrecautionId = model.PrecautionId,
                    Reporter = model.Reporter,
                    Reason = model.Reason,
                    Status = model.Status,
                    Number = model.Number,
                    Date = model.Date,
                    UserId = model.UserId,
                };

                bool isFinished
                    = await _userPrecautionService.ChangeUserPrecautionAsync(userPrecautionDTO, user);

                if (!isFinished)
                {
                    return NotFound();
                }

                return NoContent();
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
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> EditPrecaution(PrecautionDto PrecautionDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var query = new ChangePrecautionCommand(PrecautionDTO, await _userManager.GetUserAsync(User));
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
        /// Checks if theres already a Precaution with such number
        /// </summary>
        /// <param name="number">Number which checking</param>
        /// <returns>True if exist</returns>
        /// <returns>False if doesn't exist</returns>
        /// <response code="200">Check was successfull</response>
        [HttpGet("numberExist/{number}")]
        [Authorize(Roles = Roles.AdminAndGBAdmin)]
        public async Task<IActionResult> CheckNumberExisting(int number)
        {
            bool distNumber = await _userPrecautionService.DoesNumberExistAsync(number);
            return Ok(distNumber);
        }

        /// <summary>
        /// Checks if theres already an active Precaution with such type for user
        /// </summary>
        /// <param name="userId">User id which checking</param>
        /// <param name="type">Type which checking</param>
        /// <returns>True if exist</returns>
        /// <returns>False if doesn't exist</returns>
        /// <response code="200">Check was successfull</response>
        [HttpGet("checkUserPrecautionsType/{userId}")]
        [Authorize(Roles = Roles.AdminAndGBAdmin)]
        public async Task<IActionResult> CheckUserPrecautionsType(string userId, string type)
        {
            bool distNumber = await _userPrecautionService.CheckUserPrecautionsType(userId, type);
            return Ok(distNumber);
        }

        /// <summary>
        /// Get an active Precaution with such type for user
        /// </summary>
        /// <param name="userId">User id which checking</param>
        /// <param name="type">Type which checking</param>
        /// <returns>True if exist</returns>
        /// <returns>False if doesn't exist</returns>
        /// <response code="200">Check was successfull</response>
        [HttpGet("getUserActivePrecautionEndDate/{userId}")]
        [Authorize(Roles = Roles.AdminAndGBAdmin)]
        public async Task<IActionResult> GetUserActivePrecautionEndDate(string userId, string type)
        {
            string endDate = (await _userPrecautionService.GetUserActivePrecaution(userId, type)).EndDate.ToShortDateString();
            return Ok(endDate);
        }

        /// <summary>
        /// Find all users without active precautions
        /// </summary>
        /// <returns>Table of users without active precautions</returns>
        /// <response code="200">Table of all users without active precautions</response>
        [HttpGet("usersWithoutPrecautions")]
        public async Task<IActionResult> UsersWithoutPrecautionsTable(string tab)
        {
            var result = await _userPrecautionService.UsersTableWithoutPrecautionAsync();
            return Ok(result);
        }

        [HttpGet("getUsersForPrecaution")]
        [Authorize(Roles = Roles.AdminAndGBAdmin)]
        public async Task<IActionResult> GetUsersForPrecaution()
        {
            var result = await _userPrecautionService.GetUsersForPrecautionAsync(await _userManager.GetUserAsync(User));
            return Ok(result);
        }

    }
}
