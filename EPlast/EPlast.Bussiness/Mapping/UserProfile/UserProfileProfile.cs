using AutoMapper;
using EPlast.Bussiness.DTO.UserProfiles;

namespace EPlast.Bussiness.Mapping.UserProfile
{
    public class UserProfileProfile : Profile
    {
        public UserProfileProfile()
        {
            CreateMap<DataAccess.Entities.UserProfile, UserProfileDTO>().ReverseMap();
        }
    }
}
