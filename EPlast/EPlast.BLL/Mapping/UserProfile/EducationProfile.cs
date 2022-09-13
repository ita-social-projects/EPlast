using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.UserProfile
{
    public class EducationProfile : Profile
    {
        public EducationProfile()
        {
            CreateMap<Education, EducationDto>().ReverseMap();
        }
    }
}
