using AutoMapper;
using EPlast.BussinessLayer.DTO.Events;
using EPlast.ViewModels.Events;

namespace EPlast.Mapping.Events
{
    public class EventGalleryProfile : Profile
    {
        public EventGalleryProfile()
        {
            CreateMap<EventGalleryDTO, EventGalleryViewModel>();
        }
    }
}
