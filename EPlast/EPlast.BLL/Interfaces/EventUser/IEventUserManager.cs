using System.Threading.Tasks;
using EPlast.BLL.DTO.EventUser;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Interfaces.EventUser
{
    /// <summary>
    /// Implement operation for getting events for each user, creating new events, editing existing events
    /// </summary>
    public interface IEventUserManager
    {
        /// <summary>
        /// Get all data for creating event
        /// </summary>
        /// <returns>Array of data for creating event</returns>
        Task<EventCreateDto> InitializeEventCreateDTOAsync();

        /// <summary>
        /// Create a event
        /// </summary>
        /// <returns>A newly created event</returns>
        /// <param name="model">Event DTO</param>
        Task<int> CreateEventAsync(EventCreateDto model);

        /// <summary>
        /// Get event for edit
        /// </summary>
        /// <returns>A edited event and data for editing</returns>
        /// <param name="eventId"></param>
        Task<EventCreateDto> InitializeEventEditDTOAsync(int eventId);

        /// <summary>
        /// Put edited event
        /// </summary>
        /// <returns>A newly edited event</returns>
        /// <param name="model">Event DTO</param>
        Task<bool> EditEventAsync(EventCreateDto model, User currentUser);


        /// <summary>
        /// Put approved event
        /// </summary>
        /// <returns>A newly approved event</returns>
        /// <param name="eventId"></param>
        Task<int> ApproveEventAsync(int id);
    }
}
