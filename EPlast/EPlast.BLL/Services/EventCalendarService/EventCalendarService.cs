using EPlast.BLL.DTO.EventCalendar;
using EPlast.BLL.Interfaces.EventCalendar;
using EPlast.BLL.Interfaces.EventUser;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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

        public async Task<List<EventCalendarInfoDTO>> GetAllActions()
        {
            var events = await _eventsManager.GetActionsAsync();

            return events;
        }

        public async Task<List<EventCalendarInfoDTO>> GetAllEducations()
        {
            var events = await _eventsManager.GetEducationsAsync();

            return events;
        }

        public async Task<List<EventCalendarInfoDTO>> GetAllCamps()
        {
            var events = await _eventsManager.GetCampsAsync();

            return events;
        }
    }
}
