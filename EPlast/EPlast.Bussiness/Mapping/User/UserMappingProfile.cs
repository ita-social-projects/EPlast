using AutoMapper;
using EPlast.Bussiness.DTO.UserProfiles;

namespace EPlast.Bussiness.Mapping.User
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<DataAccess.Entities.User, UserDTO>().ReverseMap();
        }
    }
}
