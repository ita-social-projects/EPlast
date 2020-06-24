using EPlast.Bussiness.DTO.Events;
using EPlast.Bussiness.Interfaces.Events;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.Bussiness.Services.Events
{
    public class EventCategoryManager : IEventCategoryManager
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public EventCategoryManager(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }
        public async Task<List<EventCategoryDTO>> GetDTOAsync()
        {
            var eventCategories = (await _repoWrapper.EventCategory.GetAllAsync()).ToList();
            var dto = eventCategories
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
