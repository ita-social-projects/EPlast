using AutoMapper;
using EPlast.BLL.DTO.Region;
using EPlast.DataAccess.Entities;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.Region
{
    public class RegionProfile : Profile
    {
        public RegionProfile()
        {
            CreateMap<DatabaseEntities.Region, RegionDto>().ReverseMap();
            CreateMap<DatabaseEntities.Region, RegionObject>().ReverseMap();
            CreateMap<DatabaseEntities.RegionNamesObject, RegionNamesDto>().ReverseMap();
            CreateMap<DatabaseEntities.RegionObject, RegionObjectsDto>().ReverseMap();
            CreateMap<DatabaseEntities.Region, RegionProfileDto>().ReverseMap();
            CreateMap<DatabaseEntities.Region, RegionProfileDto>()
                .ForMember(r => r.IsActive, s => s.MapFrom(t => t.IsActive));
            CreateMap<RegionDto, RegionProfileDto>().ReverseMap();
        }
    }
}