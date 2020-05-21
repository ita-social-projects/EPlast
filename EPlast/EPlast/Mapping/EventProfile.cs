using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BussinessLayer.DTO.Events;
using EPlast.ViewModels.Events;

namespace EPlast.Mapping
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<EventAdminDTO, EventAdminViewModel>();
            CreateMap<EventGalleryDTO, EventGalleryViewModel>();
            CreateMap<EventParticipantDTO, EventParticipantViewModel>();
            CreateMap<EventInfoDTO, EventInfoViewModel>()
                .ForMember(d => d.EventParticipants, s => s.MapFrom(f => f.EventParticipants))
                .ForMember(d => d.EventGallery, s => s.MapFrom(f => f.EventGallery))
                .ForMember(d => d.EventAdmins, s => s.MapFrom(f => f.EventAdmins));

        }
    }
}
