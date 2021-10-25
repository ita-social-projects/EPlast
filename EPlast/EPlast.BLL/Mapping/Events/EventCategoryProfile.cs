using AutoMapper;
using EPlast.BLL.DTO.Events;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.BLL.Mapping.Events
{
    public class EventCategoryProfile : Profile
    {
        public EventCategoryProfile()
        {
            CreateMap<EventCategory, EventCategoryDTO>().ReverseMap();
        }
    }
}