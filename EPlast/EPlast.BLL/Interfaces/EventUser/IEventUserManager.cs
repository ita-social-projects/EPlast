using EPlast.BLL.DTO.EventUser;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.EventUser
{
    /// <summary>
    /// Implement operation for getting events for each user, creating new events, editing existing events
    /// </summary>
    public interface IEventUserManager
    {
        /// <summary>
        /// Get all created, planned, visited events for user by id
        /// </summary>
        /// <returns>Array of all created, planned, visited events for user</returns>
        /// /// <param name="userId"></param>
        /// /// <param name="user"></param>
        Task<EventUserDTO> EventUserAsync(string userId, ClaimsPrincipal user);

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
    }
}
