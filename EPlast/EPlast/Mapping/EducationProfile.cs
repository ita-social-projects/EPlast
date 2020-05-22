using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels.UserInformation.UserProfile;

namespace EPlast.Mapping
{
    public class EducationProfile : Profile
    {
        public EducationProfile()
        {
            CreateMap<Education, EducationDTO>().ReverseMap();
            CreateMap<EducationViewModel, EducationDTO>().ReverseMap();
        }
    }
}
