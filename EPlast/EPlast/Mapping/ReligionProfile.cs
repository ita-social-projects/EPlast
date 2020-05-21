using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels.UserInformation.UserProfile;

namespace EPlast.Mapping
{
    public class ReligionProfile:Profile
    {
        public ReligionProfile()
        {
            CreateMap<Religion, ReligionDTO>();
            CreateMap<ReligionDTO, Religion>();
            CreateMap<ReligionViewModel, ReligionDTO>();
            CreateMap<ReligionDTO, ReligionViewModel>();
        }
    }
}
