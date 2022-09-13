using AutoMapper;
using EPlast.BLL.DTO.AnnualReport;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.AnnualReport
{
    public class RegionProfile : Profile
    {
        public RegionProfile()
        {
            CreateMap<DatabaseEntities.Region, RegionDto>().ReverseMap();
        }
    }
}