using EPlast.BLL.DTO.EventUser;
using EPlast.BLL.Interfaces.EventUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class EventsUsersController : ControllerBase
    {
        private readonly IEventUserManager _eventUserManager;

        public EventsUsersController(IEventUserManager eventUserManager)
        {
            _eventUserManager = eventUserManager;
        }

        /// <summary>
        /// Get all created events for user by id which date are expired
        /// </summary>
        /// <returns>Array of all created events for user</returns>
        /// /// <param name="userId"></param>
        /// <response code="200">Instance of EventUserDTO</response>
        /// <response code="400">When the EventUserDTO is null or empty</response> 
        [HttpGet("createArchivedEvents/{userId}")]
        public async Task<IActionResult> GetCreatedArchivedEventsByUserId(string userId)
        {
            var eventUserModel = await _eventUserManager.GetCreatedArchivedEvents(userId, User);

            return Ok(eventUserModel);
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
                var eventUserModel = await _eventUserManager.EventUserAsync(userId, User);

                return Ok(eventUserModel);
        }

        /// <summary>
        /// Get all data for creating event
        /// </summary>
        /// <returns>Array of data for creating event</returns>
        /// <response code="200">Instance of EventCreateDTO</response>
        /// <response code="400">When the EventCreateDTO is null or empty</response> 
        [HttpGet("dataForNewEvent")]
        [Authorize(Roles = "Пластун,Admin")]
        public async Task<IActionResult> GetEventsDataForCreate()
        {
                var eventCreateModel = await _eventUserManager.InitializeEventCreateDTOAsync();

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
        [Authorize(Roles = "Пластун,Admin")]
        public async Task<IActionResult> EventCreate([FromBody] EventCreateDTO createDTO)
        {
                await _eventUserManager.CreateEventAsync(createDTO);

                return Created(nameof(GetEventUserByUserId), createDTO);
        }

        /// <summary>
        /// Get event for edit
        /// </summary>
        /// <returns>A edited event and data for editing</returns>
        /// <param name="eventId"></param>
        /// <response code="200">Instance of EventCreateDTO</response>
        /// <response code="400">When the EventCreateDTO is null or empty</response> 
        [HttpGet("editedEvent/{eventId:int}")]
        [Authorize(Roles = "Пластун,Admin")]
        public async Task<IActionResult> EventEdit(int eventId)
        {
                var eventCreateModel = await _eventUserManager.InitializeEventEditDTOAsync(eventId);

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
        [Authorize(Roles = "Пластун,Admin")]
        public async Task<IActionResult> EventEdit([FromBody] EventCreateDTO createDTO)
        {
                await _eventUserManager.EditEventAsync((createDTO));

                return NoContent();
        }

    }
}