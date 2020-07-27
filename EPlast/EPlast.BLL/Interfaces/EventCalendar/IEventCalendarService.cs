using EPlast.BLL.DTO.EventCalendar;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Interfaces.EventCalendar
{
    /// <summary>
    /// Implement operation for getting events for each type
    /// </summary>
    public interface IEventCalendarService
    {
        /// <summary>
        /// Get all events of actions type
        /// </summary>
        /// <returns>Array of events of actions type</returns>
        Task<List<EventCalendarInfoDTO>> GetAllActions();


        /// <summary>
        /// Get all events of education type
        /// </summary>
        /// <returns>Array of events of education type</returns>
        Task<List<EventCalendarInfoDTO>> GetAllEducations();

        /// <summary>
        /// Get all events of camps type
        /// </summary>
        /// <returns>Array of events of camps type</returns>
        Task<List<EventCalendarInfoDTO>> GetAllCamps();
    }
}
