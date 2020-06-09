using EPlast.BussinessLayer.DTO.Events;
using EPlast.BussinessLayer.Interfaces.Events;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BussinessLayer.Services.Events
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
            List<EventCategoryDTO> dto = await _repoWrapper.EventCategory.FindAll()
                .Select(eventCategory => new EventCategoryDTO()
                {
                    EventCategoryId = eventCategory.ID,
                    EventCategoryName = eventCategory.EventCategoryName
                })
                .ToListAsync();
            return dto;
        }
    }
}
