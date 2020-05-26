using EPlast.BussinessLayer.DTO.Events;
using EPlast.BussinessLayer.Interfaces.Events;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace EPlast.BussinessLayer.Services.Events
{
    public class EventCategoryManager : IEventCategoryManager
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public EventCategoryManager(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }
        public List<EventCategoryDTO> GetDTO()
        {
            List<EventCategoryDTO> dto = _repoWrapper.EventCategory.FindAll()
                .Select(eventCategory => new EventCategoryDTO()
                {
                    EventCategoryId = eventCategory.ID,
                    EventCategoryName = eventCategory.EventCategoryName
                })
                .ToList();
            return dto;
        }
    }
}
