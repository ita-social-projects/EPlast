using EPlast.BLL.DTO.EventUser;
using System.Threading.Tasks;

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
        Task<EventCreateDTO> InitializeEventCreateDTOAsync();

        /// <summary>
        /// Create a event
        /// </summary>
        /// <returns>A newly created event</returns>
        /// <param name="createDTO"></param>
        Task<int> CreateEventAsync(EventCreateDTO model);

        /// <summary>
        /// Get event for edit
        /// </summary>
        /// <returns>A edited event and data for editing</returns>
        /// <param name="eventId"></param>
        Task<EventCreateDTO> InitializeEventEditDTOAsync(int eventId);

        /// <summary>
        /// Put edited event
        /// </summary>
        /// <returns>A newly edited event</returns>
        /// <param name="createDTO"></param>
        Task EditEventAsync(EventCreateDTO model);


        /// <summary>
        /// Put approved event
        /// </summary>
        /// <returns>A newly approved event</returns>
        /// <param name="eventId"></param>
        Task<int> ApproveEventAsync(int id);
    }
}
