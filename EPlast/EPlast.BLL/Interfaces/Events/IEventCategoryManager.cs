using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Events;

namespace EPlast.BLL.Interfaces.Events
{
    /// <summary>
    ///  Implements  operations for work with event categories.
    /// </summary>
    public interface IEventCategoryManager
    {
        Task<IEnumerable<EventCategoryDto>> GetDTOAsync();

        /// <summary>
        /// Get list of event categories by event type Id.
        /// </summary>
        /// <returns>List of event categories of the appropriate event type.</returns>
        /// <param name="eventTypeId">The Id of event type</param>
        Task<IEnumerable<EventCategoryDto>> GetDTOByEventTypeIdAsync(int eventTypeId);

        /// <summary>
        /// Get list of event categories by event type Id.
        /// </summary>
        /// <returns>List of event categories of the appropriate event type.</returns>
        /// <param name="eventTypeId">The Id of event type</param>
        /// <param name="page">A number of the page</param>
        /// <param name="pageSize">A count of categories to display</param>
        Task<IEnumerable<EventCategoryDto>> GetDTOByEventPageAsync(int eventTypeId, int page, int pageSize, string CategoryName = null);

        /// <summary>
        /// Create a new category
        /// </summary>
        /// <returns>The Id of newly created category</returns>
        /// <param name="eventCategoryCreateDto"></param>
        Task<int> CreateEventCategoryAsync(EventCategoryCreateDto eventCategoryCreateDto);

        /// <summary>
        /// Update a category
        /// </summary>
        /// <returns>No Content</returns>
        /// <param name="eventCategoryUpdateDto"></param>
        Task<bool> UpdateEventCategoryAsync(EventCategoryDto eventCategoryUpdateDto);

        /// <summary>
        /// Delete a category
        /// </summary>
        /// <returns>No Content</returns>
        /// <param name="id"></param>
        Task<bool> DeleteEventCategoryAsync(int id);
    }
}
