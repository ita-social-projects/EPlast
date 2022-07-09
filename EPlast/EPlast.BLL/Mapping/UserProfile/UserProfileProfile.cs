using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;

namespace EPlast.BLL.Mapping.UserProfile
{
    public class UserProfileProfile : Profile
    {
        public UserProfileProfile()
        {
            CreateMap<DataAccess.Entities.UserProfile, UserProfileDto>().ReverseMap();
        }
    }
}
