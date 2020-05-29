using AutoMapper;
using EPlast.BussinessLayer.DTO.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels.UserInformation.UserProfile;

namespace EPlast.Mapping
{
    public class GenderProfile : Profile
    {
        public GenderProfile()
        {
            CreateMap<Gender, GenderDTO>().ReverseMap();
            CreateMap<GenderViewModel, GenderDTO>().ReverseMap();
        }
    }
}
