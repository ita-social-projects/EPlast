using AutoMapper;
using EPlast.BussinessLayer.DTO.EventUser;
using EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Mapping.EventUser
{
    public class EventGeneralInfoProfile : Profile
    {
        public EventGeneralInfoProfile()
        {
            CreateMap<Event, EventGeneralInfoDTO>();
        }
    }
}
