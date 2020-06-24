using AutoMapper;
using EPlast.BusinessLogicLayer.DTO.EventUser;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.BusinessLogicLayer.Mapping.EventUser
{
    public class EventAdministrationEditProfile : Profile
    {
        public EventAdministrationEditProfile()
        {
            CreateMap<EventAdministration, EventAdministrationDTO>().ReverseMap();
        }
    }
}