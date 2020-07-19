using AutoMapper;
using EPlast.BLL.DTO.EventCalendar;
using EPlast.BLL.Interfaces.EventCalendar;
using EPlast.BLL.Interfaces.EventUser;
using EPlast.DataAccess.Entities.Event;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.BLL.Services
{
    public class EventCalendarService : IEventCalendarService
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IMapper _mapper;
        private readonly IEventsManager _eventsManager;

        public EventCalendarService(IRepositoryWrapper repoWrapper, IMapper mapper, IEventsManager eventsManager)
        {
            _repoWrapper = repoWrapper;
            _mapper = mapper;
            _eventsManager = eventsManager;
        }

        public async Task<List<EventCalendarInfoDTO>> GetAllEvents()
        {
            var events = await _eventsManager.GetEventsAsync();

            return events;

        }
    }
}
