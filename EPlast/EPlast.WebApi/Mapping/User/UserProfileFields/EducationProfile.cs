using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.WebApi.Models.UserModels.UserProfileFields;

namespace EPlast.WebApi.Mapping.User.UserProfileFields
{
    public class EducationProfile:Profile
    {
        public EducationProfile()
        {
            CreateMap<EducationDto, EducationViewModel>().ReverseMap();
        }
    }
}
