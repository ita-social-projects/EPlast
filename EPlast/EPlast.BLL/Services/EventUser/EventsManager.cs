using EPlast.BLL.DTO.EventCalendar;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.EventUser
{
    public class EventsManager : IEventsManager
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public EventsManager(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }
        public async Task<List<EventCalendarInfoDTO>> GetEventsAsync()
        {
            var events = (await _repoWrapper.Event.GetAllAsync()).ToList();
            var dto = events
                .Select(events => new EventCalendarInfoDTO()
                {
                    ID = events.ID,
                    EventName = events.EventName,
                    EventDateStart = events.EventDateStart,
                    EventDateEnd = events.EventDateEnd,
                    Description = events.Description
                })
                .ToList();

            return dto;
        }
    }
}
