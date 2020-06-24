using AutoMapper;
using EPlast.Bussiness.DTO.EventUser;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.Bussiness.Mapping.EventUser
{
    public class EventAdministrationEditProfile : Profile
    {
        public EventAdministrationEditProfile()
        {
            CreateMap<EventAdministration, EventAdministrationDTO>().ReverseMap();
        }
    }
}