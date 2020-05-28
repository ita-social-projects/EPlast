using AutoMapper;
using EPlast.BussinessLayer.DTO.EventUser;
using EPlast.ViewModels.EventUser;

namespace EPlast.Mapping.EventUser
{
    public class EventAdministrationProfile : Profile
    {
        public EventAdministrationProfile()
        {
            CreateMap<EventAdministrationDTO, EventAdministrationViewModel>().ReverseMap();
        }
    }
}
