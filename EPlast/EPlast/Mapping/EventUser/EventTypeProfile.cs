using AutoMapper;
using EPlast.BusinessLogicLayer.DTO.Events;
using EPlast.BusinessLogicLayer.DTO.EventUser;
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
