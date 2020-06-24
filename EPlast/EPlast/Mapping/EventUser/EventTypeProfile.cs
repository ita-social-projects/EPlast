using AutoMapper;
using EPlast.BLL.DTO.Events;
using EPlast.BLL.DTO.EventUser;
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
