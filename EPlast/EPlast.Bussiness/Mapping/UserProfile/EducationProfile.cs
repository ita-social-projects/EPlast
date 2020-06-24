using AutoMapper;
using EPlast.Bussiness.DTO.UserProfiles;
using EPlast.DataAccess.Entities;

namespace EPlast.Bussiness.Mapping.UserProfile
{
    public class EducationProfile : Profile
    {
        public EducationProfile()
        {
            CreateMap<Education, EducationDTO>().ReverseMap();
        }
    }
}
