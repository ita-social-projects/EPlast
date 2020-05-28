using AutoMapper;
using EPlast.BussinessLayer.DTO.Events;
using EPlast.BussinessLayer.DTO.EventUser;
using EPlast.ViewModels.EventUser;

namespace EPlast.Mapping.EventUser
{
    public class EventTypeProfile : Profile
    {
        public EventTypeProfile()
        {
            CreateMap<EventTypeDTO, EventTypeViewModel>().ReverseMap();
        }
    }
}
