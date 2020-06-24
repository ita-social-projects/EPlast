using AutoMapper;
using EPlast.Bussiness.DTO;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.Bussiness.Mapping.Region
{
    public class RegionProfile : Profile
    {
        public RegionProfile()
        {
            CreateMap<DatabaseEntities.Region, RegionDTO>().ReverseMap();
        }
    }
}