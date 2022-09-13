using AutoMapper;
using EPlast.BLL.DTO.Region;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.Region
{
    public class RegionAnnualReportProfile : Profile
    {
        public RegionAnnualReportProfile()
        {
            CreateMap<DatabaseEntities.RegionAnnualReport, RegionAnnualReportDto>().ReverseMap();
        }
    }
}
