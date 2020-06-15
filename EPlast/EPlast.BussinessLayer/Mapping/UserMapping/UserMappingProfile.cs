using AutoMapper;
using EPlast.BussinessLayer.DTO.UserProfiles;
using EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Mapping.UserMapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
