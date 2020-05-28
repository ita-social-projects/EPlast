using AutoMapper;
using EPlast.BussinessLayer.DTO.EventUser;
using EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Mapping.EventUser
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<EventDTO, Event>().ReverseMap();
        }
    }
}
