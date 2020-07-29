using EPlast.BLL.Interfaces.EventCalendar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsCalendarController : ControllerBase
    {
        private readonly IEventCalendarService _eventCalendarService;

        public EventsCalendarController(IEventCalendarService eventCalendarService)
        {
            _eventCalendarService = eventCalendarService;
        }

        /// <summary>
        /// Get all events of actions type
        /// </summary>
        /// <returns>Array of events of actions type</returns>
        /// <response code="200">Instance of EventCalendarInfoDTO</response>
        /// <response code="400">When the EventCalendarInfoDTO is null or empty</response> 
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("actionsForCalendar")]
        public async Task<IActionResult> GetActions()
        {
            var events = await _eventCalendarService.GetAllActions();

            return Ok(events);
        }

        /// <summary>
        /// Get all events of education type
        /// </summary>
        /// <returns>Array of events of education type</returns>
        /// <response code="200">Instance of EventCalendarInfoDTO</response>
        /// <response code="400">When the EventCalendarInfoDTO is null or empty</response> 
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("educationsForCalendar")]
        public async Task<IActionResult> GetEducations()
        {
            var events = await _eventCalendarService.GetAllEducations();

            return Ok(events);
        }

        /// <summary>
        /// Get all events of camps type
        /// </summary>
        /// <returns>Array of events of camps type</returns>
        /// <response code="200">Instance of EventCalendarInfoDTO</response>
        /// <response code="400">When the EventCalendarInfoDTO is null or empty</response> 
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("campsForCalendar")]
        public async Task<IActionResult> GetCamps()
        {
            var events = await _eventCalendarService.GetAllCamps();

            return Ok(events);
        }
    }
}
