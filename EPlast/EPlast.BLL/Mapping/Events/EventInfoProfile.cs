using System.Linq;
using AutoMapper;
using EPlast.BLL.DTO.Events;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.BLL.Mapping.Events
{
    public class EventInfoProfile : Profile
    {
        public EventInfoProfile()
        {
            CreateMap<Event, EventInfoDto>()
                .ForMember(d => d.EventId, s => s.MapFrom(e => e.ID))
                .ForMember(d => d.EventName, s => s.MapFrom(e => e.EventName))
                .ForMember(d => d.Description, s => s.MapFrom(e => e.Description))
                .ForMember(d => d.EventDateStart, s => s.MapFrom(e => e.EventDateStart.ToString("dd.MM.yyyy HH:mm")))
                .ForMember(d => d.EventDateEnd, s => s.MapFrom(e => e.EventDateEnd.ToString("dd.MM.yyyy HH:mm")))
                .ForMember(d => d.EventLocation, s => s.MapFrom(e => e.Eventlocation))
                .ForMember(d => d.EventTypeId, s => s.MapFrom(e => e.EventType.ID))
                .ForMember(d => d.EventType, s => s.MapFrom(e => e.EventType.EventTypeName))
                .ForMember(d => d.EventCategory, s => s.MapFrom(e => e.EventCategory.ID))
                .ForMember(d => d.EventCategory, s => s.MapFrom(e => e.EventCategory.EventCategoryName))
                .ForMember(d => d.EventStatus, s => s.MapFrom(e => e.EventStatus.EventStatusName))
                .ForMember(d => d.FormOfHolding, s => s.MapFrom(e => e.FormOfHolding))
                .ForMember(d => d.ForWhom, s => s.MapFrom(e => e.ForWhom))
                .ForMember(d => d.Rating, s => s.MapFrom(e => e.Rating))
                .ForMember(d => d.EventAdmins, s => s.MapFrom(e => e.EventAdministrations.ToList()))
                .ForMember(d => d.EventParticipants, s => s.MapFrom(e => e.Participants.ToList()));
        }
    }
}
