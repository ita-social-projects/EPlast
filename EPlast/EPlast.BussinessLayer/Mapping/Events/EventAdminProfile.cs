using AutoMapper;
using EPlast.BussinessLayer.DTO.Events;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.BussinessLayer.Mapping.Events
{
    public class EventAdminProfile : Profile
    {
        public EventAdminProfile()
        {
            CreateMap<EventAdmin, EventAdminDTO>()
                .ForMember(d => d.UserId, s => s.MapFrom(f => f.UserID))
                .ForMember(d => d.Email, s => s.MapFrom(f => f.User.UserName))
                .ForMember(d => d.FullName, s => s.MapFrom(f => $"{f.User.FirstName} {f.User.LastName}"));

        }
    }
}
