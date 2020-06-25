using AutoMapper;
using EPlast.BLL.DTO.Events;
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
