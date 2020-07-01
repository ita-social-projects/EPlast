using AutoMapper;
using EPlast.BLL.DTO.Events;
using EPlast.ViewModels.Events;

namespace EPlast.Mapping.Events
{
    public class EventInfoProfile : Profile
    {
        public EventInfoProfile()
        {
            CreateMap<EventInfoDTO, EventInfoViewModel>();
        }
    }
}
