using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.ViewModels.UserInformation.UserProfile;

namespace EPlast.Mapping
{
    public class UserProfileProfile : Profile
    {
        public UserProfileProfile()
        {
            CreateMap<DataAccess.Entities.UserProfile, UserProfileDTO>().ReverseMap();
            CreateMap<UserProfileViewModel, UserProfileDTO>().ReverseMap();
        }
    }
}
