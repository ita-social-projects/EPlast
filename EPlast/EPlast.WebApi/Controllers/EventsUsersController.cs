using EPlast.BLL.DTO.EventUser;
using EPlast.BLL.Interfaces.EventUser;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsUsersController : ControllerBase
    {
        private readonly IEventUserManager _eventUserManager;

        public EventsUsersController(IEventUserManager eventUserManager)
        {
            _eventUserManager = eventUserManager;
        }

        /// <summary>
        /// Get all created, planned, visited events for user by id
        /// </summary>
        /// <returns>Array of all created, planned, visited events for user</returns>
        /// /// <param name="userId"></param>
        /// <response code="200">Instance of EventUserDTO</response>
        /// <response code="400">If the array is null or empty</response> 
        [HttpGet("eventsUsers/{userId}")]
        public async Task<IActionResult> GetEventUserByUserId(string userId)
            {
            try
            {
                var eventUserModel = await _eventUserManager.EventUserAsync(userId, User);

                return Ok(eventUserModel);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Get all data for creating event
        /// </summary>
        /// <returns>Array of data for creating event</returns>
        /// <response code="200">Instance of EventCreateDTO</response>
        /// <response code="400">If the array is null or empty</response> 
        [HttpGet("dataForNewEvent")]
        public async Task<IActionResult> GetEventsDataForCreate()
        {
            try
            {
                var eventCreateModel = await _eventUserManager.InitializeEventCreateDTOAsync();

                return Ok(eventCreateModel);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Create a event
        /// </summary>
        /// <returns>A newly created event</returns>
        /// <param name="createDTO"></param>
        /// <response code="200">Instance of EventCreateDTO</response>
        /// <response code="400">If the array is null or empty</response> 
        [HttpPost("newEvent")]
        public async Task<IActionResult> EventCreate([FromBody] EventCreateDTO createDTO)
            {
            try
            {
                await _eventUserManager.CreateEventAsync(createDTO);

                return Created(nameof(GetEventUserByUserId), createDTO);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Get event for edit
        /// </summary>
        /// <returns>A edited event and data for editing</returns>
        /// <param name="eventId"></param>
        /// <response code="200">Instance of EventCreateDTO</response>
        /// <response code="400">If the array is null or empty</response> 
        [HttpGet("editedEvent/{eventId:int}")]
        public async Task<IActionResult> EventEdit(int eventId)
        {
            try
            {
                var eventCreateModel = await _eventUserManager.InitializeEventEditDTOAsync(eventId);

                return Ok(eventCreateModel);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Put edited event
        /// </summary>
        /// <returns>A newly edited event</returns>
        /// <param name="createDTO"></param>
        /// <response code="200">Instance of EventCreateDTO</response>
        /// <response code="400">If the array is null or empty</response> 
        [HttpPut("editedEvent")]
        public async Task<IActionResult> EventEdit([FromBody] EventCreateDTO createDTO)
        {
            try
                {
                await _eventUserManager.EditEventAsync((createDTO));

                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}