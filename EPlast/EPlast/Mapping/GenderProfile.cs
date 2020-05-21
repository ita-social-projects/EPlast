using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels.UserInformation.UserProfile;

namespace EPlast.Mapping
{
    public class GenderProfile:Profile
    {
        public GenderProfile()
        {
            CreateMap<Gender, GenderDTO>();
            CreateMap<GenderDTO, Gender>();
            CreateMap<GenderViewModel, GenderDTO>();
            CreateMap<GenderDTO, GenderViewModel>();
        }
    }
}
