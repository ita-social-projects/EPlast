using AutoMapper;
using EPlast.BusinessLogicLayer.DTO.UserProfiles;

namespace EPlast.BusinessLogicLayer.Mapping.User
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<DataAccess.Entities.User, UserDTO>().ReverseMap();
        }
    }
}
