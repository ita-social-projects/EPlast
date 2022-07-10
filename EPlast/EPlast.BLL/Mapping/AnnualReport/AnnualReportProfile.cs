using AutoMapper;
using EPlast.BLL.DTO.AnnualReport;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.AnnualReport
{
    public class AnnualReportProfile : Profile
    {
        public AnnualReportProfile()
        {
            CreateMap<DatabaseEntities.AnnualReport, AnnualReportDto>().ReverseMap();
        }
    }
}