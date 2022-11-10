using AutoMapper;
using EPlast.BLL.DTO.Events;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.WebApi.Mapping.Event
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<EventCategoryDto, EventCategory>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.EventCategoryId));
        }
    }
}
