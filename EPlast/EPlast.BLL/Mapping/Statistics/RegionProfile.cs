using AutoMapper;
using DatabaseEntities = EPlast.DataAccess.Entities;
using DTOs = EPlast.BLL.DTO.Statistics;

namespace EPlast.BLL.Mapping.Statistics
{
    public class RegionProfile : Profile
    {
        public RegionProfile()
        {
            CreateMap<DTOs.Region, DatabaseEntities.Region>().ReverseMap();
        }
    }
}