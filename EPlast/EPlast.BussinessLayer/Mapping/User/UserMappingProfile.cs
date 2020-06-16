using AutoMapper;
using EPlast.BussinessLayer.DTO.UserProfiles;

namespace EPlast.BussinessLayer.Mapping.User
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<DataAccess.Entities.User, UserDTO>().ReverseMap();
        }
    }
}
