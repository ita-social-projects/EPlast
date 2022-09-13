using AutoMapper;
using EPlast.BLL.DTO.EventUser;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.BLL.Mapping.EventUser
{
    public class EventEditAdminProfile : Profile
    {
        public EventEditAdminProfile()
        {
            CreateMap<EventAdmin, CreateEventAdminDto>().ReverseMap();
        }
    }
}
