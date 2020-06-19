using AutoMapper;
using EPlast.BussinessLayer.DTO.AnnualReport;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Mapping.AnnualReport
{
    public class RegionProfile : Profile
    {
        public RegionProfile()
        {
            CreateMap<DatabaseEntities.Region, RegionDTO>().ReverseMap();
        }
    }
}