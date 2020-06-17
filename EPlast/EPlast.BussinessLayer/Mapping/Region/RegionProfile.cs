using AutoMapper;
using EPlast.BussinessLayer.DTO;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Mapping.Region
{
    public class RegionProfile : Profile
    {
        public RegionProfile()
        {
            CreateMap<DatabaseEntities.Region, RegionDTO>().ReverseMap();
        }
    }
}