using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Events;
using EPlast.BLL.Interfaces.Events;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;

namespace EPlast.BLL.Services.Events
{
    public class EventCategoryManager : IEventCategoryManager
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IEventTypeManager _eventTypeManager;
        private readonly IMapper _mapper;

        public EventCategoryManager(IRepositoryWrapper repoWrapper, IEventTypeManager eventTypeManager, IMapper mapper)
        {
            _repoWrapper = repoWrapper;
            _eventTypeManager = eventTypeManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EventCategoryDto>> GetDTOAsync()
        {
            var eventCategories = await _repoWrapper.EventCategory.GetAllAsync();
            var dto = eventCategories
                .Select(eventCategory => new EventCategoryDto()
                {
                    EventCategoryId = eventCategory.ID,
                    EventCategoryName = eventCategory.EventCategoryName
                });

            return dto;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventCategoryDto>> GetDTOByEventPageAsync(int eventTypeId, int page, int pageSize, string CategoryName = null)
        {
            var eventType = await _eventTypeManager.GetTypeByIdAsync(eventTypeId);
            var dto = eventType.EventCategories
                .Select(eventTypeCategory => new EventCategoryDto()
                {
                    EventCategoryId = eventTypeCategory.EventCategoryId,
                    EventCategoryName = eventTypeCategory.EventCategory.EventCategoryName
                });

            return dto;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventCategoryDto>> GetDTOByEventTypeIdAsync(int eventTypeId)
        {
            var eventType = await _eventTypeManager.GetTypeByIdAsync(eventTypeId);
            var dto = eventType.EventCategories
                .Select(eventTypeCategory => new EventCategoryDto()
                {
                    EventCategoryId = eventTypeCategory.EventCategoryId,
                    EventCategoryName = eventTypeCategory.EventCategory.EventCategoryName,
                    EventSectionId = eventTypeCategory.EventCategory.EventSectionId
                });

            return dto;
        }

        /// <inheritdoc />
        public async Task<int> CreateEventCategoryAsync(EventCategoryCreateDto model)
        {
            var eventCategoryToCreate = _mapper.Map<EventCategoryDto, EventCategory>(model.EventCategory);

            var existingCategory = await _repoWrapper.EventCategory
                .GetFirstOrDefaultAsync(c => c.EventCategoryName == eventCategoryToCreate.EventCategoryName);
            if (existingCategory != null)
            {
                return 0;
            }

            await _repoWrapper.EventCategory.CreateAsync(eventCategoryToCreate);
            await _repoWrapper.EventCategoryType.CreateAsync(new EventCategoryType()
            {
                EventCategory = eventCategoryToCreate,
                EventTypeId = model.EventTypeId
            });

            await _repoWrapper.SaveAsync();

            return eventCategoryToCreate.ID;
        }
    }
}
