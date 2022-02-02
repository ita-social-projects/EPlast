using AutoMapper;
using DatabaseEntities = EPlast.DataAccess.Entities;
using EPlast.BLL.DTO.GoverningBody.Announcement;

namespace EPlast.BLL.Mapping.GoverningBody.Announcement
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<DatabaseEntities.User, UserDTO>().ReverseMap();
        }
    }
}
