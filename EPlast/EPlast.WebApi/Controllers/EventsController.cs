using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Events;
using EPlast.BLL.Interfaces.Events;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using EPlast.WebApi.Models.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EPlast.WebApi.Controllers
{
    /// <summary>
    /// Implements all business logic related with events.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IActionManager _actionManager;
        private readonly UserManager<User> _userManager;
        private readonly IEventStatusManager _eventStatusManager;
        private readonly IEventCategoryManager _eventCategoryManager;

        public EventsController(IActionManager actionManager, UserManager<User> userManager, IEventStatusManager eventStatusManager, IEventCategoryManager eventCategoryManager)
        {
            _actionManager = actionManager;
            _userManager = userManager;
            _eventStatusManager = eventStatusManager;
            _eventCategoryManager = eventCategoryManager;
        }

        /// <summary>
        /// Get all event types.
        /// </summary>
        /// <returns>List of all event types.</returns>
        /// <response code="200">List of all event types</response>
        /// <response code="400">Server could not understand the request due to invalid syntax</response> 
        [HttpGet("types")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetTypes()
        {
            return Ok(await _actionManager.GetEventTypesAsync());
        }

        /// <summary>
        /// Get event categories of the appropriate event type.
        /// </summary>
        /// <returns>List of event categories of the appropriate event type.</returns>
        /// <param name="typeId">The Id of event type</param>
        /// <response code="200">List of event categories</response>
        /// <response code="400">Server could not understand the request due to invalid syntax</response> 
        /// <response code="404">Events does not exist</response> 
        [HttpGet("types/{typeId:int}/categories")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetCategories(int typeId)
        {
            return Ok(await _actionManager.GetCategoriesByTypeIdAsync(typeId));
        }

        /// <summary>
        /// Get all event sections
        /// </summary>
        /// <returns>Array of data for creating event</returns>
        /// <response code="200">Instance of EventCreateDTO</response>
        /// <response code="400">When the EventCreateDTO is null or empty</response> 
        [HttpGet("sections")]
        public async Task<IActionResult> GetSections()
        {
            var eventSections = await _actionManager.GetEventSectionsAsync();

            return Ok(eventSections);
        }

        /// <summary>
        /// Get event categories of the appropriate event type. If type is Акція - get all event categories.
        /// </summary>
        /// <returns>List of event categories of the appropriate event type.</returns>
        /// <param name="typeId">The Id of event type</param>
        /// <param name="page">A number of the page</param>
        /// <param name="pageSize">A count of categories to display</param>
        /// <response code="200">List of event categories</response>
        /// <response code="400">Server could not understand the request due to invalid syntax</response> 
        /// <response code="404">Events does not exist</response> 
        [HttpGet("types/{typeId:int}/categories/{page:int}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetCategoriesByTypeAndPageAsync(int typeId, int page, int pageSize)
        {
            IEnumerable<BLL.DTO.Events.EventCategoryDto> categories;
            if (typeId == 1)
            {
                categories = await _actionManager.GetActionCategoriesAsync();
            }
            else
            {
                categories = await _actionManager.GetCategoriesByTypeIdAsync(typeId);
            }
            var categoriesViewModel = new EventsCategoryViewModel(page, pageSize, categories);

            return Ok(categoriesViewModel);
        }

        /// <summary>
        /// Get event category by ID.
        /// </summary>
        /// <returns>List of event categories of the appropriate event type.</returns>
        /// <param name="id">The Id of event type</param>
        /// <response code="200">List of event categories</response>
        /// <response code="400">Server could not understand the request due to invalid syntax</response> 
        /// <response code="404">Events does not exist</response> 
        [HttpGet("categories/{id:int}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _actionManager.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        /// <summary>
        /// Create a new category
        /// </summary>
        /// <returns>A newly created category</returns>
        /// <param name="createDTO"></param>
        /// <response code="201">Instance of EventCategoryCreateDTO</response>
        /// <response code="400">When the EventCategoryCreateDTO is null or empty</response> 
        [HttpPost("newCategory")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CreateEventCategory([FromBody] EventCategoryCreateDto createDTO)
        {
            createDTO.EventCategory.EventCategoryId = await _eventCategoryManager.CreateEventCategoryAsync(createDTO);

            return Ok(createDTO);
        }

        /// <summary>
        /// Get events of the appropriate event type and event category.
        /// </summary>
        /// <returns>List of events of the appropriate event type and event category.</returns>
        /// <param name="typeId">The Id of event type</param>
        /// <param name="categoryId">The Id of event category</param>
        /// <response code="200">List of events</response>
        /// <response code="400">Server could not understand the request due to i
        /// nvalid syntax</response> 
        /// <response code="404">Events don't exist</response> 
        [HttpGet("~/api/types/{typeId:int}/categories/{categoryId:int}/events")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetEvents(int typeId, int categoryId)
        {
            return Ok(await _actionManager.GetEventsAsync(categoryId, typeId, await _userManager.GetUserAsync(User)));
        }


        /// <summary>
        /// Get events of the appropriate event type, event category and event status.
        /// </summary>
        /// <returns>List of events of the appropriate event type, event category and event status.</returns>
        /// <param name="typeId">The Id of event type</param>
        /// <param name="categoryId">The Id of event category</param>
        /// <param name="status">The status of event</param>
        /// <response code="200">List of events</response>
        /// <response code="400">Server could not understand the request due to i
        /// nvalid syntax</response> 
        /// <response code="404">Events don't exist</response> 

        [HttpGet("~/api/types/{typeId:int}/categories/{categoryId:int}/events/{status}")]
        public async Task<IActionResult> GetEventsByCategoryAndStatus(int typeId, int categoryId, int status)
        {
            return Ok(await _actionManager.GetEventsByStatusAsync(categoryId, typeId, status, await _userManager.GetUserAsync(User)));
        }



        /// <summary>
        /// Get detailed information about specific event.
        /// </summary>
        /// <returns>A detailed information about specific event.</returns>
        /// <param name="id">The Id of event</param>
        /// <response code="200">An instance of event</response>
        /// <response code="400">Server could not understand the request due to invalid syntax</response> 
        /// <response code="404">Event does not exist</response> 
        [HttpGet("{id:int}/details")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetEventDetail(int id)
        {
            var eventInfo = await _actionManager.GetEventInfoAsync(id, await _userManager.GetUserAsync(User));

            if (eventInfo == null) return NotFound();
            return Ok(eventInfo);
        }

        /// <summary>
        /// Get status id of event.
        /// </summary>
        /// <returns>A detailed information about specific event.</returns>
        /// <param name="status">The status</param>
        [HttpGet("{status}/statusId")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetEventStatusId(string status)
        {
            return Ok(await _eventStatusManager.GetStatusIdAsync(status));
        }

        /// <summary>
        /// Add a feedback for an event.
        /// </summary>
        /// <returns>Status code of sending a feedback of the participant's event operation.</returns>  
        /// <param name="id">The Id of event</param>
        /// <param name="feedback">Feedback DTO</param>
        /// <response code="200">OK</response>
        /// <response code="403">The user was not present at an event</response>
        /// <response code="404">The event was not found</response>
        /// <response code="400">Bad Request</response>  
        [HttpPut("{id:int}/feedbacks")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> LeaveFeedback(int id, EventFeedbackDto feedback)
        {
            var result = await _actionManager.LeaveFeedbackAsync(id, feedback, await _userManager.GetUserAsync(User));

            return result switch
            {
                StatusCodes.Status200OK => Ok(),
                StatusCodes.Status403Forbidden => Forbid(),
                StatusCodes.Status404NotFound => NotFound(),
                _ => BadRequest()
            };
        }

        /// <summary>
        /// Add a feedback for an event.
        /// </summary>
        /// <returns>Status code of sending a feedback of the participant's event operation.</returns>  
        /// <param name="id">The Id of event</param>
        /// <param name="fId">Feedback ID</param>
        /// <response code="200">OK</response>
        /// <response code="403">The user was not present at an event</response>
        /// <response code="404">The event was not found</response>
        /// <response code="400">Bad Request</response>  
        [HttpDelete("{id:int}/feedbacks/{fId:int}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteFeedback(int id, int fId)
        {
            var result = await _actionManager.DeleteFeedbackAsync(id, fId, await _userManager.GetUserAsync(User));

            return result switch
            {
                StatusCodes.Status200OK => Ok(),
                StatusCodes.Status403Forbidden => Forbid(),
                StatusCodes.Status404NotFound => NotFound(),
                _ => BadRequest()
            };
        }

        /// <summary>
        /// Change present status of the participant's event.
        /// </summary>
        /// <returns>Status code of the changing a present  status of the participant's event operation.</returns>  
        /// <param name="id">The Id of participant</param>
        /// <response code="204">No content</response>
        /// <response code="404">Not found a participant</response>
        [HttpGet("participant/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> ChangeUserPresentStatus(int id)
        {
            try
            {
                await _actionManager.ChangeUsersPresentStatusAsync(id);
                return NoContent();
            }
            catch(KeyNotFoundException)
            {
                return NotFound();
            }
            
        }

        /// <summary>
        /// Get pictures in Base64 format by event Id.
        /// </summary>
        /// <returns>List of pictures in Base64 format.</returns>
        /// <param name="eventId">The Id of event</param>
        /// <response code="200">List of pictures</response>
        /// <response code="400">Server could not understand the request due to invalid syntax</response> 
        [HttpGet("{eventId:int}/pictures")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetPictures(int eventId)
        {
            var pictures = await _actionManager.GetPicturesAsync(eventId);

            return Ok(pictures);
        }

        /// <summary>
        /// Get a picture in Base64 format by its' Id.
        /// </summary>
        /// <returns>Picture data in Base64 format.</returns>
        /// <param name="pictureId">The Id of the picture</param>
        /// <response code="200">Picture data</response>
        /// <response code="404">Picture wasn't found</response> 
        [HttpGet("pictures/{pictureId:int}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetPicture(int pictureId)
        {
            var picture = await _actionManager.GetPictureAsync(pictureId);

            if (picture == null) return NotFound();
            return Ok(picture);
        }

        /// <summary>
        /// Delete event by Id.
        /// </summary>
        /// <returns>Status code of the event deleting operation.</returns>
        /// <param name="id">The Id of event</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response> 
        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.HeadsAndHeadDeputiesAndAdminAndPlastun)]
        public async Task<IActionResult> Delete(int id)
        {
            return StatusCode(await _actionManager.DeleteEventAsync(id));
        }

        /// <summary>
        /// Delete picture by Id.
        /// </summary>
        /// <returns>Status code of the picture deleting operation.</returns>
        /// <param name="pictureId">The Id of picture</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response> 
        /// <response code="404">Not Found</response> 
        [HttpDelete("pictures/{pictureId:int}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeletePicture(int pictureId)
        {
            return StatusCode(await _actionManager.DeletePictureAsync(pictureId));
        }

        /// <summary>
        /// Create new event participant.
        /// </summary>
        /// <returns>Status code of the subscribing on event operation.</returns>
        /// <param name="eventId">The Id of event</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response> 
        [HttpPost("{eventId:int}/participants")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> SubscribeOnEvent(int eventId)
        {
            if ((await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User))).Contains(Roles.RegisteredUser))
                return StatusCode(StatusCodes.Status403Forbidden);
            return StatusCode(await _actionManager.SubscribeOnEventAsync(eventId, await _userManager.GetUserAsync(User)));
        }

        /// <summary>
        /// Delete event participant by event id.
        /// </summary>
        /// <returns>Status code of the unsubscribing on event operation.</returns>
        /// <param name="eventId">The Id of event</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response> 
        [HttpDelete("{eventId:int}/participants")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UnSubscribeOnEvent(int eventId)
        {
            return StatusCode(await _actionManager.UnSubscribeOnEventAsync(eventId, await _userManager.GetUserAsync(User)));
        }

        /// <summary>
        /// Change event participant status to approved.
        /// </summary>
        /// <returns>Status code of the changing event participant status operation.</returns>
        /// <param name="participantId">The Id of event participant</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response> 
        [HttpPut("participants/{participantId:int}/status/approved")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.HeadsAndHeadDeputiesAndAdminAndPlastun)]
        public async Task<IActionResult> ApproveParticipant(int participantId)
        {
            return StatusCode(await _actionManager.ApproveParticipantAsync(participantId));
        }

        /// <summary>
        /// Change event participant status to under reviewed.
        /// </summary>
        /// <returns>Status code of the changing event participant status operation.</returns>
        /// <param name="participantId">The Id of event participant</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response> 
        [HttpPut("participants/{participantId:int}/status/underReviewed")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.HeadsAndHeadDeputiesAndAdminAndPlastun)]
        public async Task<IActionResult> UnderReviewParticipant(int participantId)
        {
            return StatusCode(await _actionManager.UnderReviewParticipantAsync(participantId));
        }

        /// <summary>
        /// Change event participant status to rejected.
        /// </summary>
        /// <returns>Status code of the changing event participant status operation.</returns>
        /// <param name="participantId">The Id of event participant</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response> 
        [HttpPut("participants/{participantId:int}/status/rejected")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.HeadsAndHeadDeputiesAndAdminAndPlastun)]
        public async Task<IActionResult> RejectParticipant(int participantId)
        {
            return StatusCode(await _actionManager.RejectParticipantAsync(participantId));
        }

        /// <summary>
        /// Add pictures to gallery of specific event by event Id.
        /// </summary>
        /// <returns>List of added pictures.</returns>
        /// <param name="eventId">The Id of event</param>
        /// <param name="files">List of uploaded pictures</param>
        /// <response code="200">List of added pictures</response>
        /// <response code="400">Server could not understand the request due to invalid syntax</response> 
        [HttpPost("{eventId:int}/eventGallery")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> FillEventGallery(int eventId, [FromForm] IList<IFormFile> files)
        {
            return Ok(await _actionManager.FillEventGalleryAsync(eventId, files));
        }
    }
}
