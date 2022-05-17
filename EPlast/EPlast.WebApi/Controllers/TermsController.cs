using System;
using System.Threading.Tasks;
using EPlast.BLL.Commands.TermsOfUse;
using EPlast.BLL.DTO.Terms;
using EPlast.BLL.Queries.TermsOfUse;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Roles=Roles.Admin)]
    
    public class TermsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMediator _mediator;

        public TermsController( UserManager<User> userManager, IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        /// <summary>
        /// Get first record from database
        /// </summary>
        /// <returns>First record</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Data not found</response>
        [HttpGet("Data")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFirstTermsOfUse()
        {
            var query = new GetFirstRecordQuery();
            var termsDto = await _mediator.Send(query);
            if (termsDto == null)
                return NotFound();
            return Ok(termsDto); 
        }

        /// <summary>
        /// Get all users id
        /// </summary>
        /// <returns>All users id</returns>
        /// <response code="200">Successful operation</response>
        [HttpGet("UsersId")]
        public async Task<IActionResult> GetAllUsersId()
        {
            var query = new GetAllUsersIdWithoutSenderQuery(await _userManager.GetUserAsync(User));
            var usersId = await _mediator.Send(query);
            return Ok(usersId);
        }

        /// <summary>
        /// Edit current terms of use
        /// </summary>
        /// <param name="termsDTO">Terms model(dto)</param>
        /// <returns>Answer from backend</returns>
        /// <response code="204">Terms was updated</response>
        /// <response code="404">Terms not found</response>
        /// <response code="400">Terms model is not valid</response>
        [HttpPut("Data/{id}")]
        public async Task<IActionResult> EditTerms(TermsDTO termsDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var query = new ChangeTermsCommand(termsDTO, await _userManager.GetUserAsync(User));
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
        /// Creates new terms by id
        /// </summary>
        /// <param name="termsDTO">Terms model(dto)</param>
        /// <returns>Answer from backend</returns>
        /// <response code="204">Terms was created</response>
        /// <response code="404">Terms not found</response>
        /// <response code="400">Terms model is not valid</response>
        [HttpPost("Data/{id}")]
        public async Task<IActionResult> AddTerms(TermsDTO termsDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var query = new AddTermsCommand(termsDTO, await _userManager.GetUserAsync(User));
                    await _mediator.Send(query);
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