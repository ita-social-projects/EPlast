using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.ViewModels.UserInformation.UserProfile;

namespace EPlast.Mapping.User
{
    public class ShortUserInformationProfile : Profile
    {
        public ShortUserInformationProfile()
        {
            CreateMap<UserViewModel, ShortUserInformationDTO>().
                ForMember(x => x.Birthday, q => q.MapFrom(w => w.UserProfile.Birthday)).
                ReverseMap();
        }
    }
}
