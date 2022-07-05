using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;

namespace EPlast.BLL.Mapping.User
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<DataAccess.Entities.User, UserDto>().ReverseMap();
        }
    }
}
