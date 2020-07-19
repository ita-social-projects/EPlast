using AutoMapper;
using EPlast.BLL.DTO.EventCalendar;
using EPlast.DataAccess.Entities.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.Mapping.EventCalendarProfile
{
    class EventCalendarProfile : Profile
    {
        public EventCalendarProfile()
        {
            CreateMap<Event, EventCalendarInfoDTO>().ReverseMap();
        }
    }
}
