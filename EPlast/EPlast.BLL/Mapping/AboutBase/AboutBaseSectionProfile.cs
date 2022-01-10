using AutoMapper;
using EPlast.BLL.DTO.AboutBase;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities.AboutBase;

namespace EPlast.BLL.Mapping.AboutBase
{
    class AboutBaseSectionProfile: Profile
    {
        public AboutBaseSectionProfile()
        {
            CreateMap<Section, SectionDTO>().ReverseMap();
        }
    }
}
