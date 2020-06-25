using AutoMapper;
using EPlast.BLL.DTO.Events;
using EPlast.ViewModels.Events;

namespace EPlast.Mapping.Events
{
    public class EventInfoProfile : Profile
    {
        public EventInfoProfile()
        {
            CreateMap<EventInfoDTO, EventInfoViewModel>()
                .ForMember(d => d.EventParticipants, s => s.MapFrom(f => f.EventParticipants))
                .ForMember(d => d.EventGallery, s => s.MapFrom(f => f.EventGallery))
                .ForMember(d => d.EventAdmins, s => s.MapFrom(f => f.EventAdmins));
        }
    }
}
