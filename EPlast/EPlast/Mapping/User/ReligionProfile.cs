using AutoMapper;
using EPlast.BLL.DTO.UserProfiles;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels.UserInformation.UserProfile;

namespace EPlast.Mapping
{
    public class ReligionProfile:Profile
    {
        public ReligionProfile()
        {
            CreateMap<Religion, ReligionDTO>().ReverseMap();
            CreateMap<ReligionViewModel, ReligionDTO>().ReverseMap();
        }
    }
}
