using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BussinessLayer.DTO.Events;
using EPlast.ViewModels.Events;

namespace EPlast.Mapping
{
    public class ActionProfile:Profile
    {
        public ActionProfile()
        {
            CreateMap<EventCategoryDTO, EventCategoryViewModel>();
            CreateMap<GeneralEventDTO, GeneralEventViewModel>();
            CreateMap<EventDTO, EventViewModel>()
                .ForMember(d=>d.Event, s=>s.MapFrom(f=>f.Event));
        }
    }


}
