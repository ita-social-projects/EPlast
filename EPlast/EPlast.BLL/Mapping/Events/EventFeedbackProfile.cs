using AutoMapper;
using EPlast.BLL.DTO.Events;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.BLL.Mapping.Events
{
    public class EventFeedbackProfile : Profile
    {
        public EventFeedbackProfile()
        {
            CreateMap<EventFeedback, EventFeedbackDto>()
                .ForMember(f => f.AuthorName, s => s.MapFrom(d => d.Participant.User.FirstName + ' ' + d.Participant.User.LastName))
                .ForMember(f => f.AuthorAvatarUrl, s => s.MapFrom(d => d.Participant.User.ImagePath))
                .ForMember(f => f.AuthorUserId, s => s.MapFrom(d => d.Participant.UserId));

            CreateMap<EventFeedbackDto, EventFeedback>();
        }
    }
}
