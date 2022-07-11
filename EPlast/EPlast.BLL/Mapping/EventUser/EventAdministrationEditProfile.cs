using AutoMapper;
using EPlast.BLL.DTO.EventUser;
using EPlast.DataAccess.Entities.Event;

namespace EPlast.BLL.Mapping.EventUser
{
    public class EventAdministrationEditProfile : Profile
    {
        public EventAdministrationEditProfile()
        {
            CreateMap<EventAdministration, EventAdministrationDto>()
                .ForMember(d => d.UserId, s => s.MapFrom(f => f.User.Id))
                .ForMember(d => d.Email, s => s.MapFrom(f => f.User.UserName))
                .ForMember(d => d.FullName, s => s.MapFrom(f => $"{f.User.FirstName} {f.User.LastName}")).ReverseMap();
        }
    }
}