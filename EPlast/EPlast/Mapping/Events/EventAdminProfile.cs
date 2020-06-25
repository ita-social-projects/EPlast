using AutoMapper;
using EPlast.BLL.DTO.Events;
using EPlast.ViewModels.Events;

namespace EPlast.Mapping.Events
{
    public class EventAdminProfile : Profile
    {
        public EventAdminProfile()
        {
            CreateMap<EventAdminDTO, EventAdminViewModel>();
        }
    }
}
