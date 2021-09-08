using EPlast.BLL.DTO.EventUser;
using EPlast.BLL.Interfaces.Events;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EPlast.BLL.Services.Events
{
    public class EventTypeManager : IEventTypeManager
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public EventTypeManager(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        /// <inheritdoc />
        public async Task<int> GetTypeIdAsync(string typeName)
        {
            var type = await _repoWrapper.EventType
                .GetFirstAsync(predicate: et => et.EventTypeName == typeName);

            return type.ID;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<EventTypeDTO>> GetDTOAsync()
        {
            var eventTypes = await _repoWrapper.EventType.GetAllAsync();
            var dto =  eventTypes
                .Select(eventType => new EventTypeDTO()
                {
                    ID = eventType.ID,
                    EventTypeName = eventType.EventTypeName
                });

            return  dto;
        }

        /// <inheritdoc />
        public async Task<EventType> GetTypeByIdAsync(int id)
        {
            var eventType = await _repoWrapper.EventType
                .GetFirstAsync(
                    et => et.ID == id,
                    source => source
                        .Include(et => et.EventCategories)
                        .ThenInclude(ct => ct.EventCategory)
                    );

            return eventType;
        }
    }
}
