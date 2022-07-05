using AutoMapper;
using EPlast.BLL.DTO.GoverningBody.Announcement;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.GoverningBody.Announcement
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<DatabaseEntities.User, UserDto>().ReverseMap();
        }
    }
}
