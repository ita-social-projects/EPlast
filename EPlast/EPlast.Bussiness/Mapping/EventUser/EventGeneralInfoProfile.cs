using AutoMapper;
using EPlast.BusinessLogicLayer.DTO.EventUser;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.BusinessLogicLayer.Mapping.EventUser
{
    public class EventGeneralInfoProfile : Profile
    {
        public EventGeneralInfoProfile()
        {
            CreateMap<Event, EventGeneralInfoDTO>();
        }
    }
}
