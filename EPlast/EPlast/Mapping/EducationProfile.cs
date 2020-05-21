using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels.UserInformation.UserProfile;

namespace EPlast.Mapping
{
    public class EducationProfile:Profile
    {
        public EducationProfile()
        {
            CreateMap<Education, EducationDTO>();
            CreateMap<EducationDTO, Education>();
            CreateMap<EducationViewModel, EducationDTO>();
            CreateMap<EducationDTO, EducationViewModel>();
        }
    }
}
