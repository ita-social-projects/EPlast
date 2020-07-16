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

        [HttpGet("eventsForCalendar")]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _eventCalendarService.GetAllEvents();

            return Ok(events);
        }
    }
}
