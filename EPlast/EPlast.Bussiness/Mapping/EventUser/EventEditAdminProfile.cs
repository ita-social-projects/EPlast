using AutoMapper;
using EPlast.BusinessLogicLayer.DTO.EventUser;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.BusinessLogicLayer.Mapping.EventUser
{
    public class EventEditAdminProfile : Profile
    {
        public EventEditAdminProfile()
        {
            CreateMap<EventAdmin, CreateEventAdminDTO>().ReverseMap();
        }
    }
}
