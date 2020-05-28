using AutoMapper;
using EPlast.BussinessLayer.DTO.EventUser;
using EPlast.ViewModels.EventUser;

namespace EPlast.Mapping.EventUser
{
    public class EventGeneralInfoProfile : Profile
    {
        public EventGeneralInfoProfile()
        {
            CreateMap<EventGeneralInfoDTO, EventGeneralInfoViewModel>();
        }
    }
}
