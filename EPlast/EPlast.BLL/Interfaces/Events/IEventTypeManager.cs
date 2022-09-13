using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.EventUser;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.BLL.Interfaces.Events
{
    /// <summary>
    ///  Implements  operations for work with event types.
    /// </summary>
    public interface IEventTypeManager
    {
        /// <summary>
        /// Get Id of event type by type name.
        /// </summary>
        /// <returns>The Id of specific event type.</returns>
        /// <param name="typeName">The name of event type</param>
        Task<int> GetTypeIdAsync(string typeName);

        /// <summary>
        /// Get all event types.
        /// </summary>
        /// <returns>List of all event types.</returns>
        Task<IEnumerable<EventTypeDto>> GetEventTypesDTOAsync();

        /// <summary>
        /// Get an information about specific event type.
        /// </summary>
        /// <returns>An information about specific event type.</returns>
        /// <param name="id">The Id of event type</param>
        Task<EventType> GetTypeByIdAsync(int id);
    }
}
