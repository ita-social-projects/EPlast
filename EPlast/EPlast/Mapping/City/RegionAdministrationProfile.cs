using AutoMapper;
using EPlast.BLL.DTO.Region;
using EPlast.DataAccess.Entities;

namespace EPlast.Mapping
{
    public class RegionAdministrationProfile : Profile
    {
        public RegionAdministrationProfile()
        {
            CreateMap<RegionAdministration, RegionAdministrationDTO>().ReverseMap();
        }
    }
}