using AutoMapper;
using EPlast.Bussiness.DTO.EventUser;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.Bussiness.Mapping.EventUser
{
    public class EventEditAdminProfile : Profile
    {
        public EventEditAdminProfile()
        {
            CreateMap<EventAdmin, CreateEventAdminDTO>().ReverseMap();
        }
    }
}
