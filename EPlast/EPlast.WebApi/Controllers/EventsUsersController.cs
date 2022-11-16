using System;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BLL.DTO.EventUser;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class EventsUsersController : ControllerBase
    {
        private readonly IEventUserManager eventUserManager;
        private readonly IEventUserService eventUserService;
        private readonly UserManager<User> _userManager;

        public EventsUsersController(IEventUserManager eventUserManager, IEventUserService eventUserService, UserManager<User> userManager)
        {
            this.eventUserManager = eventUserManager;
            this.eventUserService = eventUserService;
            _userManager = userManager;
        }

        private async Task<bool> HasAccessAsync()
        {
            var roles = await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User));
            var role = roles.FirstOrDefault(x => Roles.HeadsAndHeadDeputiesAndAdminAndPlastMemberAndSupporter.Contains(x));
            if (role != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get all created, planned, visited events for user by id
        /// </summary>
        /// <returns>Array of all created, planned, visited events for user</returns>
        /// /// <param name="userId"></param>
        /// <response code="200">Instance of EventUserDTO</response>
        /// <response code="400">When the EventUserDTO is null or empty</response> 
        [HttpGet("eventsUsers/{userId}")]
        public async Task<IActionResult> GetEventUserByUserId(string userId)
        {
            var currentUserId = _userManager.GetUserId(User);
            if (currentUserId != userId && !(await HasAccessAsync()))
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            var eventUserModel = await eventUserService.EventUserAsync(userId, await _userManager.GetUserAsync(User));
            return Ok(eventUserModel);
        }

        /// <summary>
        /// Get all data for creating event
        /// </summary>
        /// <returns>Array of data for creating event</returns>
        /// <response code="200">Instance of EventCreateDTO</response>
        /// <response code="400">When the EventCreateDTO is null or empty</response> 
        [HttpGet("dataForNewEvent")]
        public async Task<IActionResult> GetEventsDataForCreate()
        {
            var eventCreateModel = await eventUserManager.InitializeEventCreateDTOAsync();

            return Ok(eventCreateModel);
        }

        /// <summary>
        /// Create a new event
        /// </summary>
        /// <returns>A newly created event</returns>
        /// <param name="createDTO"></param>
        /// <response code="201">Instance of EventCreateDTO</response>
        /// <response code="400">When the EventCreateDTO is null or empty</response> 
        [HttpPost("newEvent")]
        [Authorize(Roles = Roles.HeadsAndHeadDeputiesAndAdminAndPlastun)]
        public async Task<IActionResult> EventCreate([FromBody] EventCreateDto createDTO)
        {
            try
            {
                createDTO.Event.ID = await eventUserManager.CreateEventAsync(createDTO);

                return Created(nameof(GetEventUserByUserId), createDTO);
            }
            catch (InvalidOperationException error)
            {
                return StatusCode(StatusCodes.Status400BadRequest, error.Message);
            }
        }

        /// <summary>
        /// Get event for edit
        /// </summary>
        /// <returns>A edited event and data for editing</returns>
        /// <param name="eventId"></param>
        /// <response code="200">Instance of EventCreateDTO</response>
        /// <response code="400">When the EventCreateDTO is null or empty</response> 
        [HttpGet("editedEvent/{eventId:int}")]
        public async Task<IActionResult> EventEdit(int eventId)
        {
            var eventCreateModel = await eventUserManager.InitializeEventEditDTOAsync(eventId);

            return Ok(eventCreateModel);
        }

        /// <summary>
        /// Put edited event
        /// </summary>
        /// <returns>A newly edited event</returns>
        /// <param name="createDTO"></param>
        /// <response code="204">Resource updated successfully</response>
        /// <response code="400">When the EventCreateDTO is null or empty</response>
        [HttpPut("editedEvent")]
        [Authorize(Roles = Roles.HeadsAndHeadDeputiesAndAdminAndPlastun)]
        public async Task<IActionResult> EventEdit([FromBody] EventCreateDto createDTO)
        {
            var successful = await eventUserManager.EditEventAsync(createDTO, await _userManager.GetUserAsync(User));

            if (!successful) return Forbid();
            return NoContent();
        }

        /// <summary>
        /// Put approved event
        /// </summary>
        /// <returns>An approved event</returns>
        /// <param name="eventId"></param>
        /// <response code="204">Resource updated successfully</response>
        /// <response code="400">When the Event is not approved</response>
        [HttpPut("approveEvent/{eventId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.AdminAndGBAdmin)]
        public async Task<IActionResult> ApproveEvent(int eventId)
        {
            var eventApproved = await eventUserManager.ApproveEventAsync(eventId);

            return Ok(eventApproved);
        }

    }
}
