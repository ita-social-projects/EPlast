using AutoMapper;
using EPlast.BLL.DTO.EventUser;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.BLL.Mapping.EventUser
{
    public class EventTypeProfile : Profile
    {
        public EventTypeProfile()
        {
            CreateMap<EventType, EventTypeDto>();
        }
    }
}
