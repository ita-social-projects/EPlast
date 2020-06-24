using AutoMapper;
using EPlast.Bussiness.DTO.AnnualReport;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.Bussiness.Mapping.AnnualReport
{
    public class AnnualReportProfile : Profile
    {
        public AnnualReportProfile()
        {
            CreateMap<DatabaseEntities.AnnualReport, AnnualReportDTO>().ReverseMap();
        }
    }
}