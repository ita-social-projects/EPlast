using EPlast.BLL.Interfaces.EventCalendar;
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

        [HttpGet("actionsForCalendar")]
        public async Task<IActionResult> GetActions()
        {
            var events = await _eventCalendarService.GetAllActions();

            return Ok(events);
        }

        [HttpGet("educationsForCalendar")]
        public async Task<IActionResult> GetEducations()
        {
            var events = await _eventCalendarService.GetAllEducations();

            return Ok(events);
        }

        [HttpGet("campsForCalendar")]
        public async Task<IActionResult> GetCamps()
        {
            var events = await _eventCalendarService.GetAllCamps();

            return Ok(events);
        }
    }
}
