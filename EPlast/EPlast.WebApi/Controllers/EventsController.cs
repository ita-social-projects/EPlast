﻿using EPlast.BLL.Interfaces.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public EventsController(IActionManager actionManager)
        {
            _actionManager = actionManager;
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
        /// Get events of the appropriate event type and event category.
        /// </summary>
        /// <returns>List of events of the appropriate event type and event category.</returns>
        /// <param name="typeId">The Id of event type</param>
        /// <param name="categoryId">The Id of event category</param>
        /// <response code="200">List of events</response>
        /// <response code="400">Server could not understand the request due to invalid syntax</response> 
        /// <response code="404">Events don't exist</response> 
        [HttpGet("~/api/types/{typeId:int}/categories/{categoryId:int}/events")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetEvents(int typeId, int categoryId)
        {
            return Ok(await _actionManager.GetEventsAsync(categoryId, typeId, User));
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
            return Ok(await _actionManager.GetEventInfoAsync(id, User));
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
            var dto = await _actionManager.GetPicturesAsync(eventId);
            
            return Ok(dto);
        }

        /// <summary>
        /// Delete event by Id.
        /// </summary>
        /// <returns>Status code of the event deleting operation.</returns>
        /// <param name="id">The Id of event</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response> 
        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
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
            return StatusCode(await _actionManager.SubscribeOnEventAsync(eventId, User));
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
            return StatusCode(await _actionManager.UnSubscribeOnEventAsync(eventId, User));
        }

        /// <summary>
        /// Change event participant status to approved.
        /// </summary>
        /// <returns>Status code of the changing event participant status operation.</returns>
        /// <param name="participantId">The Id of event participant</param>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response> 
        [HttpPut("participants/{participantId:int}/status/approved")]
        [Authorize(AuthenticationSchemes = "Bearer")]
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
        [Authorize(AuthenticationSchemes = "Bearer")]
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
        [Authorize(AuthenticationSchemes = "Bearer")]
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
