using AutoMapper;
using EPlast.BussinessLayer.DTO.EventUser;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.BussinessLayer.Mapping.EventUser
{
    public class EventAdministrationEditProfile : Profile
    {
        public EventAdministrationEditProfile()
        {
            CreateMap<EventAdministration, EventAdministrationDTO>().ReverseMap();
        }
    }
}