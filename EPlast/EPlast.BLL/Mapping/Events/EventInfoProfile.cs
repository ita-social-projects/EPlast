using AutoMapper;
using EPlast.BLL.DTO.Events;
using EPlast.DataAccess.Entities.Event;
using System.Linq;

namespace EPlast.BLL.Mapping.Events
{
    public class EventInfoProfile : Profile
    {
        public EventInfoProfile()
        {
            CreateMap<Event, EventInfoDTO>()
                .ForMember(d => d.EventId, s => s.MapFrom(e => e.ID))
                .ForMember(d => d.EventName, s => s.MapFrom(e => e.EventName))
                .ForMember(d => d.Description, s => s.MapFrom(e => e.Description))
                .ForMember(d => d.EventDateStart, s => s.MapFrom(e => e.EventDateStart.ToString("dd-MM-yyyy")))
                .ForMember(d => d.EventDateEnd, s => s.MapFrom(e => e.EventDateEnd.ToString("dd-MM-yyyy")))
                .ForMember(d => d.EventLocation, s => s.MapFrom(e => e.Eventlocation))
                .ForMember(d => d.EventType, s => s.MapFrom(e => e.EventType.EventTypeName))
                .ForMember(d => d.EventCategory, s => s.MapFrom(e => e.EventCategory.EventCategoryName))
                .ForMember(d => d.EventStatus, s => s.MapFrom(e => e.EventStatus.EventStatusName))
                .ForMember(d => d.FormOfHolding, s => s.MapFrom(e => e.FormOfHolding))
                .ForMember(d => d.ForWhom, s => s.MapFrom(e => e.ForWhom))
                .ForMember(d => d.NumberOfParticipants, s => s.MapFrom(e => e.NumberOfPartisipants))
                .ForMember(d => d.EventAdministrations, s => s.MapFrom(e => e.EventAdministrations.ToList()))
                .ForMember(d => d.EventParticipants, s => s.MapFrom(e => e.Participants.ToList()))
                .ForMember(d => d.EventGallery, s => s.MapFrom(e => e.EventGallarys.ToList()));
        }
    }
}
