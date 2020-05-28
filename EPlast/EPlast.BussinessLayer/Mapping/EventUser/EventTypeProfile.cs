using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EPlast.BussinessLayer.DTO.Events;
using EPlast.BussinessLayer.DTO.EventUser;
using EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Mapping.EventUser
{
    public class EventTypeProfile : Profile
    {
        public EventTypeProfile()
        {
            CreateMap<EventType, EventTypeDTO>();
        }
    }
}
