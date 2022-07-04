using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.BLL.DTO.EventCalendar;
using EPlast.BLL.Interfaces.EventCalendar;
using EPlast.BLL.Interfaces.EventUser;

namespace EPlast.BLL.Services
{
    /// <inheritdoc/>
    public class EventCalendarService : IEventCalendarService
    {
        private readonly IEventsManager _eventsManager;

        public EventCalendarService(IEventsManager eventsManager)
        {
            _eventsManager = eventsManager;
        }

        public async Task<List<EventCalendarInfoDto>> GetAllActions()
        {
            var events = await _eventsManager.GetActionsAsync();

            return events;
        }

        public async Task<List<EventCalendarInfoDto>> GetAllEducations()
        {
            var events = await _eventsManager.GetEducationsAsync();

            return events;
        }

        public async Task<List<EventCalendarInfoDto>> GetAllCamps()
        {
            var events = await _eventsManager.GetCampsAsync();

            return events;
        }
    }
}
