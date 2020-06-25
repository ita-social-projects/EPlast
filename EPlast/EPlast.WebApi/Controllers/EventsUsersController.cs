using EPlast.BLL.DTO.EventUser;
using EPlast.BLL.Interfaces.EventUser;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

//namespace EPlast.WebApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class EventsUsersController : ControllerBase
//    {
//        private readonly IEventUserManager _eventUserManager;

//        public EventsUsersController(IEventUserManager eventUserManager)
//        {
//            _eventUserManager = eventUserManager;
//        }

//        [HttpGet("eventsUsers/{userId}")]
//        public async Task<IActionResult> GetEventUserByUserId(string userId)
//        {
//            try
//            {
//                var eventUserModel = await _eventUserManager.EventUserAsync(userId, User);

//                return Ok(eventUserModel);
//            }
//            catch
//            {
//                return BadRequest();
//            }
//        }

//        [HttpGet("dataForNewEvent")]
//        public async Task<IActionResult> GetEventsDataForCreate()
//        {
//            try
//            {
//                var eventCreateModel = await _eventUserManager.InitializeEventCreateDTOAsync();

//                return Ok(eventCreateModel);
//            }
//            catch
//            {
//                return BadRequest();
//            }
//        }

//        [HttpPost("newEvent")]
//        public async Task<IActionResult> EventCreate(EventCreateDTO createDTO)
//        {
//            try
//            {
//                await _eventUserManager.CreateEventAsync(createDTO);

//                return CreatedAtAction(nameof(SetAdministrationByEventId), new { id = createDTO.Event.ID }, createDTO);
//            }
//            catch
//            {
//                return BadRequest();
//            }
//        }

//        [HttpGet("newAdministration/{eventId:int}")]
//        public async Task<IActionResult> SetAdministrationByEventId(int eventId)
//        {
//            try
//            {
//                var eventCreateModel = await _eventUserManager.InitializeEventCreateDTOAsync(eventId);

//                return Ok(eventCreateModel);
//            }
//            catch
//            {
//                return BadRequest();
//            }
//        }

//        [HttpPost("newAdministration")]
//        public async Task<IActionResult> SetAdministration(EventCreateDTO createDTO)
//        {
//            try
//            {
//                    await _eventUserManager.SetAdministrationAsync(createDTO);

//                    return CreatedAtAction("EventInfo", "Action", new { id = createDTO.Event.ID }, createDTO);
//            }
//            catch
//            {
//                return BadRequest();
//            }
//        }

//        [HttpGet("editedEvent/{eventId:int}")]
//        public async Task<IActionResult> EventEdit(int eventId)
//        {
//            try
//            {
//                var eventCreateModel = await _eventUserManager.InitializeEventEditDTOAsync(eventId);

//                return Ok(eventCreateModel);
//            }
//            catch
//            {
//                return BadRequest();
//            }
//        }

//        [HttpPut("editedEvent")]
//        public async Task<IActionResult> EventEdit([FromBody] EventCreateDTO createDTO)
//        {
//            try
//            {
//                    await _eventUserManager.EditEventAsync((createDTO));

//                    return CreatedAtAction(nameof(GetEventUserByUserId), new { id = createDTO.Event.ID }, createDTO);
//            }
//            catch
//            {
//                return BadRequest();
//            }
//        }

//    }
//}