using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EPlast.BussinessLayer.DTO.Events;
using EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Mapping
{
    class EventGalleryProfile:Profile
    {
        public EventGalleryProfile()
        {
            CreateMap<EventGallary, EventGalleryDTO>()
                .ForMember(d => d.GalleryId, s => s.MapFrom(f => f.GallaryID))
                .ForMember(d => d.FileName, s => s.MapFrom(f => f.Gallary.GalaryFileName));
        }
    }
}
