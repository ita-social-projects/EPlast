using AutoMapper;
using EPlast.BussinessLayer.DTO.UserProfiles;

namespace EPlast.BussinessLayer.Mapping.User
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
