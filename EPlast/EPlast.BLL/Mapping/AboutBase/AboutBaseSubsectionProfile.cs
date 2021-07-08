using AutoMapper;
using EPlast.BLL.DTO.AboutBase;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.AboutBase
{
    class AboutBaseSubsectionProfile:Profile
    {
        public AboutBaseSubsectionProfile()
        {
            CreateMap<Subsection, SubsectionDTO>().ReverseMap();
        }
    }
}
