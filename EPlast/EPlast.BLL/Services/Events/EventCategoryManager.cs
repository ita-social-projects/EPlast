﻿using EPlast.BLL.DTO.Events;
using EPlast.BLL.Interfaces.Events;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.DataAccess.Entities.Event;

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

        public async Task<IEnumerable<EventCategoryDTO>> GetDTOAsync()
        {
            var eventCategories = await _repoWrapper.EventCategory.GetAllAsync();
            var dto = eventCategories
                .Select(eventCategory => new EventCategoryDTO()
                {
                    ID = eventCategory.ID,
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
                    ID = eventTypeCategory.EventCategoryId,
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
                    ID = eventTypeCategory.EventCategoryId,
                    EventCategoryName = eventTypeCategory.EventCategory.EventCategoryName,
                    EventSectionId = eventTypeCategory.EventCategory.EventSectionId
                });

            return dto;
        }


        public async Task<int> CreateEventCategoryAsync(EventCategoryCreateDTO model)
        {
            var eventCategoryToCreate = _mapper.Map<EventCategoryDTO, EventCategory>(model.EventCategory);
            var section = await _repoWrapper.EventSection.GetFirstOrDefaultAsync(s => s.ID == eventCategoryToCreate.EventSectionId);

            eventCategoryToCreate.EventSectionId = section.ID;

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
