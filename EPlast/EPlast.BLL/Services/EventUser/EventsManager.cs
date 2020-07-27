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

        public async Task<List<EventCalendarInfoDTO>> GetActionsAsync()
        {
            var events = (await _repoWrapper.Event.GetAllAsync(predicate: i => i.EventTypeID == 1)).ToList();
            var dto = events
                .Select(events => new EventCalendarInfoDTO()
                {
                    ID = events.ID,
                    Title = events.EventName,
                    Start = events.EventDateStart,
                    End = events.EventDateEnd,
                    Description = events.Description,
                    Eventlocation = events.Eventlocation,
                    EventTypeID = events.EventTypeID,
                })
                .ToList();

            return dto;
        }

        public async Task<List<EventCalendarInfoDTO>> GetEducationsAsync()
        {
            var events = (await _repoWrapper.Event.GetAllAsync(predicate: i => i.EventTypeID == 2)).ToList();
            var dto = events
                .Select(events => new EventCalendarInfoDTO()
                {
                    ID = events.ID,
                    Title = events.EventName,
                    Start = events.EventDateStart,
                    End = events.EventDateEnd,
                    Description = events.Description,
                    Eventlocation = events.Eventlocation,
                    EventTypeID = events.EventTypeID,
                })
                .ToList();

            return dto;
        }

        public async Task<List<EventCalendarInfoDTO>> GetCampsAsync()
        {
            var events = (await _repoWrapper.Event.GetAllAsync(predicate: i => i.EventTypeID == 3)).ToList();
            var dto = events
                .Select(events => new EventCalendarInfoDTO()
                {
                    ID = events.ID,
                    Title = events.EventName,
                    Start = events.EventDateStart,
                    End = events.EventDateEnd,
                    Description = events.Description,
                    Eventlocation = events.Eventlocation,
                    EventTypeID = events.EventTypeID,
                })
                .ToList();

            return dto;
        }
    }
}
