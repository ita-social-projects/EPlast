using AutoMapper;
using EPlast.BLL.DTO.Region;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.Region
{
    class RegionAdministrationProfile : Profile
    {
        public RegionAdministrationProfile()
        {
            CreateMap<DatabaseEntities.RegionAdministration, RegionAdministrationDto>().ReverseMap();
        }
    }
}
