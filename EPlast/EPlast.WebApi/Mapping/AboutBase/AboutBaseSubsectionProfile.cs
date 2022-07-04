using AutoMapper;
using EPlast.BLL.DTO.AboutBase;
using EPlast.WebApi.Models.AboutBase;

namespace EPlast.WebApi.Mapping.AboutBase
{
    public class AboutBaseSubsectionProfile: Profile
    {
        public AboutBaseSubsectionProfile()
        {
            CreateMap<SubsectionDto, AboutBaseSubsectionViewModel>().ReverseMap();
        }
    }
}
