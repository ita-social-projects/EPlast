using AutoMapper;
using EPlast.BLL.DTO.EventUser;
using EPlast.ViewModels.EventUser;

namespace EPlast.Mapping.EventUser
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<EventCreationDTO, EventCreationViewModel>().ReverseMap();
        }
    }
}
