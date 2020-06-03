using AutoMapper;
using EPlast.BussinessLayer.DTO.Events;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.BussinessLayer.Mapping.Events
{
    class EventGalleryProfile : Profile
    {
        public EventGalleryProfile()
        {
            CreateMap<EventGallary, EventGalleryDTO>()
                .ForMember(d => d.GalleryId, s => s.MapFrom(f => f.GallaryID))
                .ForMember(d => d.FileName, s => s.MapFrom(f => f.Gallary.GalaryFileName));
        }
    }
}
