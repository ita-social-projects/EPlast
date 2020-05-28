using AutoMapper;
using EPlast.BussinessLayer.DTO.EventUser;
using EPlast.ViewModels.EventUser;

namespace EPlast.Mapping.EventUser
{
    public class EventCreateProfile : Profile
    {
        public EventCreateProfile()
        {
            CreateMap<EventCreateDTO, EventCreateViewModel>().ReverseMap();
        }
    }
}
