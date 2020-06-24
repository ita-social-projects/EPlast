using AutoMapper;
using EPlast.BusinessLogicLayer.DTO.UserProfiles;
using EPlast.DataAccess.Entities;

namespace EPlast.BusinessLogicLayer.Mapping.UserProfile
{
    public class EducationProfile : Profile
    {
        public EducationProfile()
        {
            CreateMap<Education, EducationDTO>().ReverseMap();
        }
    }
}
