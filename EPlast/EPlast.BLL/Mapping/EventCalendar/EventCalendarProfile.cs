using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EPlast.BLL.DTO.EventCalendar;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.BLL.Mapping.EventCalendarProfile
{
    class EventCalendarProfile : Profile
    {
        public EventCalendarProfile()
        {
            CreateMap<Event, EventCalendarInfoDto>()
                .ForMember(e => e.Title, s => s.MapFrom(f => f.EventName))
                .ForMember(e => e.Start, s => s.MapFrom(f => f.EventDateStart))
                .ForMember(e => e.End, s => s.MapFrom(f => f.EventDateEnd)).ReverseMap();
        }
    }
}
