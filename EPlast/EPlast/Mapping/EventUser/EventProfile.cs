using AutoMapper;
using EPlast.BussinessLayer.DTO.EventUser;
using EPlast.ViewModels.EventUser;

namespace EPlast.Mapping.EventUser
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<EventDTO, EventCreationViewModel>().ReverseMap();
        }
    }
}
