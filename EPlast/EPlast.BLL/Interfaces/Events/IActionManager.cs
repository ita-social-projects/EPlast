using EPlast.BLL.DTO.Events;
using EPlast.BLL.DTO.EventUser;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.Events
{
    /// <summary>
    ///  Implements  operations for work with events.
    /// </summary>
    public interface IActionManager
    {
        /// <summary>
        /// Get all event types.
        /// </summary>
        /// <returns>List of all event types.</returns>
        Task<IEnumerable<EventTypeDTO>> GetEventTypesAsync();

        /// <summary>
        /// Get all event categories.
        /// </summary>
        /// <returns>List of all event categories.</returns>
        Task<IEnumerable<EventCategoryDTO>> GetActionCategoriesAsync();

        /// <summary>
        /// Get all events sections.
        /// </summary>
        /// <returns>List of event sections.</returns>
        Task<IEnumerable<EventSectionDTO>> GetEventSectionsAsync();

        /// <summary>
        /// Get list of event categories by event type Id.
        /// </summary>
        /// <returns>List of event categories of the appropriate event type.</returns>
        /// <param name="eventTypeId">The Id of event type</param>
        Task<IEnumerable<EventCategoryDTO>> GetCategoriesByTypeIdAsync(int eventTypeId);

        /// <summary>
        /// Get events  by event category Id and event type Id.
        /// </summary>
        /// <returns>List of events of the appropriate event type and event category.</returns>
        /// <param name="eventTypeId">The Id of event type</param>
        /// <param name="categoryId">The Id of event category</param>
        /// <param name="user">ClaimsPrincipal of logged in user</param>
        Task<IEnumerable<GeneralEventDTO>> GetEventsAsync(int categoryId, int eventTypeId, User user);

        /// <summary>
        /// Get detailed information about event by event Id.
        /// </summary>
        /// <returns>A detailed information about specific event.</returns>
        /// <param name="id">The Id of event</param>
        /// <param name="user">ClaimsPrincipal of logged in user</param>
        Task<EventDTO> GetEventInfoAsync(int id, User user);

        /// <summary>
        /// Get pictures in Base64 format by event Id.
        /// </summary>
        /// <returns>List of pictures in Base64 format.</returns>
        /// <param name="id">The Id of event</param>
        Task<IEnumerable<EventGalleryDTO>> GetPicturesAsync(int id);

        /// <summary>
        /// Delete event by Id.
        /// </summary>
        /// <returns>Status code of the event deleting operation.</returns>
        /// <param name="id">The Id of event</param>
        Task<int> DeleteEventAsync(int id);

        /// <summary>
        /// Create new event participant.
        /// </summary>
        /// <returns>Status code of the subscribing on event operation.</returns>
        /// <param name="id">The Id of event</param>
        /// <param name="user">User object</param>
        Task<int> SubscribeOnEventAsync(int id, User user);

        /// <summary>
        /// Delete event participant by event id.
        /// </summary>
        /// <returns>Status code of the unsubscribing on event operation.</returns>
        /// <param name="id">The Id of event</param>
        /// <param name="user">User object</param>
        Task<int> UnSubscribeOnEventAsync(int id, User user);

        /// <summary>
        /// Set an estimate of the participant's event.
        /// </summary>
        /// <returns>Status code of the setting an estimate of the participant's event operation.</returns>
        /// <param name="eventId">The Id of event</param>
        /// <param name="user">User object</param>
        /// <param name="estimate">The value of estimate</param>
        Task<int> EstimateEventAsync(int eventId, User user, double estimate);

        /// <summary>
        /// Change event participant status to approved.
        /// </summary>
        /// <returns>Status code of the changing event participant status operation.</returns>
        /// <param name="id">The Id of event participant</param>
        Task<int> ApproveParticipantAsync(int id);

        /// <summary>
        /// Change event participant status to under reviewed.
        /// </summary>
        /// <returns>Status code of the changing event participant status operation.</returns>
        /// <param name="id">The Id of event participant</param>
        Task<int> UnderReviewParticipantAsync(int id);

        /// <summary>
        /// Change event participant status to rejected.
        /// </summary>
        /// <returns>Status code of the changing event participant status operation.</returns>
        /// <param name="id">The Id of event participant</param>
        Task<int> RejectParticipantAsync(int id);

        /// <summary>
        /// Add pictures to gallery of specific event by event Id.
        /// </summary>
        /// <returns>List of added pictures.</returns>
        /// <param name="id">The Id of event</param>
        /// <param name="files">List of uploaded pictures</param>
        Task<IEnumerable<EventGalleryDTO>> FillEventGalleryAsync(int id, IList<IFormFile> files);

        /// <summary>
        /// Delete picture by Id.
        /// </summary>
        /// <returns>Status code of the picture deleting operation.</returns>
        /// <param name="id">The Id of picture</param>
        Task<int> DeletePictureAsync(int id);

        Task CheckEventsStatusesAsync();

        Task<IEnumerable<GeneralEventDTO>> GetEventsByStatusAsync(int categoryId, int typeId, int status, User user);
    }
}
