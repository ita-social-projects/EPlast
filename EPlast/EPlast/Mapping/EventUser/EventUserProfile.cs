using AutoMapper;
using EPlast.BLL.DTO.EventUser;
using EPlast.ViewModels.EventUser;

namespace EPlast.Mapping.EventUser
{
    public class EventUserProfile : Profile
    {
        public EventUserProfile()
        {
            CreateMap<EventUserDTO, EventUserViewModel>();
        }
    }
}
