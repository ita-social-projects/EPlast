using AutoMapper;
using EPlast.Bussiness.DTO.Events;
using EPlast.DataAccess.Entities;

namespace EPlast.Bussiness.Mapping.Events
{
    public class EventParticipantProfile : Profile
    {
        public EventParticipantProfile()
        {
            CreateMap<Participant, EventParticipantDTO>()
                .ForMember(d => d.ParticipantId, s => s.MapFrom(f => f.ID))
                .ForMember(d => d.FullName, s => s.MapFrom(f => $"{f.User.FirstName} {f.User.LastName}"))
                .ForMember(d => d.Email, s => s.MapFrom(f => f.User.UserName))
                .ForMember(d => d.UserId, s => s.MapFrom(f => f.UserId))
                .ForMember(d => d.StatusId, s => s.MapFrom(f => f.ParticipantStatusId))
                .ForMember(d => d.Status, s => s.MapFrom(f => f.ParticipantStatus.ParticipantStatusName));
        }
    }
}
