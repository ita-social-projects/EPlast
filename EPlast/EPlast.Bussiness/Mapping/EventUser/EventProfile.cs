﻿using AutoMapper;
using EPlast.BusinessLogicLayer.DTO.EventUser;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.BusinessLogicLayer.Mapping.EventUser
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<EventCreationDTO, Event>().ReverseMap();
        }
    }
}
