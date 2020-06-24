using AutoMapper;
using EPlast.BusinessLogicLayer.DTO.Events;
using EPlast.ViewModels.Events;

namespace EPlast.Mapping.Events
{
    public class EventParticipantProfile : Profile
    {
        public EventParticipantProfile()
        {
            CreateMap<EventParticipantDTO, EventParticipantViewModel>();
        }
    }
}
