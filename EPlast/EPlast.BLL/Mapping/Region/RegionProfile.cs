using AutoMapper;
using EPlast.BLL.DTO.Region;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.Region
{
    public class RegionProfile : Profile
    {
        public RegionProfile()
        {
            CreateMap<DatabaseEntities.Region, RegionDTO>().ReverseMap();
            CreateMap<DatabaseEntities.Region, RegionProfileDTO>().ReverseMap();
            CreateMap<DatabaseEntities.RegionAdministration, RegionAdministrationDTO>().ReverseMap();
        }
    }
}