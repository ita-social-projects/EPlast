using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EPlast.BussinessLayer.DTO.Events;
using EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Mapping
{
    public class EventParticipantProfile:Profile
    {
        public EventParticipantProfile()
        {
            CreateMap<Participant, EventParticipantDTO>()
                .ForMember(d => d.ParticipantId, s => s.MapFrom(f => f.ID))
                .ForMember(d => d.FullName, s => s.MapFrom(f => $"{f.User.FirstName} {f.User.LastName}"))
                .ForMember(d => d.Email, s => s.MapFrom(f => f.User.UserName))
                .ForMember(d => d.Status, s => s.MapFrom(f => f.ParticipantStatus.ParticipantStatusName));
        }
    }
}
