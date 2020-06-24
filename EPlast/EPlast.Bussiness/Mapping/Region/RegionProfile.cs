using AutoMapper;
using EPlast.BusinessLogicLayer.DTO;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BusinessLogicLayer.Mapping.Region
{
    public class RegionProfile : Profile
    {
        public RegionProfile()
        {
            CreateMap<DatabaseEntities.Region, RegionDTO>().ReverseMap();
        }
    }
}