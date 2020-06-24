using AutoMapper;
using EPlast.Bussiness.DTO.EventUser;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.Bussiness.Mapping.EventUser
{
    public class EventTypeProfile : Profile
    {
        public EventTypeProfile()
        {
            CreateMap<EventType, EventTypeDTO>();
        }
    }
}
