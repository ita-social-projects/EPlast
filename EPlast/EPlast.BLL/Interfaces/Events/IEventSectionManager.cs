using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Events;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.BLL.Interfaces.Events
{
    /// <summary>
    ///  Implements  operations for work with event sections.
    /// </summary>
    public interface IEventSectionManager
    {
        /// <summary>
        /// Get Id of event section by section name.
        /// </summary>
        /// <returns>The Id of specific event section.</returns>
        /// <param name="sectionName">The name of event section</param>
        Task<int> GetSectionIdAsync(string sectionName);

        /// <summary>
        /// Get all event sections.
        /// </summary>
        /// <returns>List of all event sections.</returns>
        Task<IEnumerable<EventSectionDTO>> GetEventSectionsDTOAsync();

        /// <summary>
        /// Get an information about specific event section.
        /// </summary>
        /// <returns>An information about specific event section.</returns>
        /// <param name="id">The Id of event section</param>
        Task<EventSection> GetSectionByIdAsync(int id);
    }
}