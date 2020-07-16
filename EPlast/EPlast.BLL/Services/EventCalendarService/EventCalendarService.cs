using AutoMapper;
using EPlast.BLL.DTO.EventCalendar;
using EPlast.BLL.Interfaces.EventCalendar;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Services
{
    public class EventCalendarService : IEventCalendarService
    {
        private readonly IEventsManager _eventsManager;

        public EventCalendarService(IEventsManager eventsManager)
        {
            _eventsManager = eventsManager;
        }

        public async Task<List<EventCalendarInfoDTO>> GetAllEvents()
        {
            var events = await _eventsManager.GetEventsAsync();

            return events;

        }
    }
}
