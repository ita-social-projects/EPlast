using AutoMapper;
using EPlast.BLL.DTO.EventUser;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.BLL.Mapping.EventUser
{
    public class EventAdministrationEditProfile : Profile
    {
        public EventAdministrationEditProfile()
        {
            CreateMap<EventAdministration, EventAdministrationDTO>().ReverseMap();
        }
    }
}