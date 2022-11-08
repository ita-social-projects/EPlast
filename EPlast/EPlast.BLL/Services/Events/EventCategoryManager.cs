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
        public async Task<int> CreateEventCategoryAsync(EventCategoryCreateDto eventCategoryCreateDto)
        {
            var eventCategoryToCreate = _mapper.Map<EventCategoryDto, EventCategory>(eventCategoryCreateDto.EventCategory);

            await _repoWrapper.EventCategory.CreateAsync(eventCategoryToCreate);
            await _repoWrapper.EventCategoryType.CreateAsync(new EventCategoryType()
            {
                EventCategory = eventCategoryToCreate,
                EventTypeId = eventCategoryCreateDto.EventTypeId
            });

            await _repoWrapper.SaveAsync();

            return eventCategoryToCreate.ID;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateEventCategoryAsync(EventCategoryDto eventCategoryUpdateDto)
        {
            var eventCategoryToUpdate = await _repoWrapper.EventCategory.GetSingleOrDefaultAsync(c => c.ID == eventCategoryUpdateDto.EventCategoryId);
            if (eventCategoryToUpdate == null) return false;
            
            var updatedEventCategory = _mapper.Map<EventCategoryDto, EventCategory>(eventCategoryUpdateDto);

            _repoWrapper.EventCategory.Update(updatedEventCategory);
            await _repoWrapper.SaveAsync();

            return true;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteEventCategoryAsync(int id)
        {
            var eventCategoryToDelete = await _repoWrapper.EventCategory.GetSingleOrDefaultAsync(c => c.ID == id);
            if (eventCategoryToDelete == null) return false;

            _repoWrapper.EventCategory.Delete(eventCategoryToDelete);
            await _repoWrapper.SaveAsync();

            return true;
        }
    }
}
