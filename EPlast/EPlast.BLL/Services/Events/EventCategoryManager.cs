using EPlast.BLL.DTO.Events;
using EPlast.BLL.Interfaces.Events;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Events
{
    public class EventCategoryManager : IEventCategoryManager
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IEventTypeManager _eventTypeManager;

        public EventCategoryManager(IRepositoryWrapper repoWrapper, IEventTypeManager eventTypeManager)
        {
            _repoWrapper = repoWrapper;
            _eventTypeManager = eventTypeManager;
        }

        public async Task<IEnumerable<EventCategoryDTO>> GetDTOAsync()
        {
            var eventCategories = await _repoWrapper.EventCategory.GetAllAsync();
            var dto = eventCategories
                .Select(eventCategory => new EventCategoryDTO()
                {
                    EventCategoryId = eventCategory.ID,
                    EventCategoryName = eventCategory.EventCategoryName
                });

            return dto;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventCategoryDTO>> GetDTOByEventPageAsync(int eventTypeId, int page, int pageSize, string CategoryName = null)
        {
            var eventType = await _eventTypeManager.GetTypeByIdAsync(eventTypeId);
            var dto = eventType.EventCategories
                .Select(eventTypeCategory => new EventCategoryDTO()
                {
                    EventCategoryId = eventTypeCategory.EventCategoryId,
                    EventCategoryName = eventTypeCategory.EventCategory.EventCategoryName
                });

            return dto;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventCategoryDTO>> GetDTOByEventTypeIdAsync(int eventTypeId)
        {
            var eventType = await _eventTypeManager.GetTypeByIdAsync(eventTypeId);
            var dto = eventType.EventCategories
                .Select(eventTypeCategory => new EventCategoryDTO()
                {
                    EventCategoryId = eventTypeCategory.EventCategoryId,
                    EventCategoryName = eventTypeCategory.EventCategory.EventCategoryName
                });

            return dto;
        }
    }
}
