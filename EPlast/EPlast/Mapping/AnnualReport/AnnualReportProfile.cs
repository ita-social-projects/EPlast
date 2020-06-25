using AutoMapper;
using EPlast.BLL.DTO.AnnualReport;
using EPlast.ViewModels.AnnualReport;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.Mapping.AnnualReport
{
    public class AnnualReportProfile : Profile
    {
        public AnnualReportProfile()
        {
            CreateMap<DatabaseEntities.AnnualReport, AnnualReportDTO>().ReverseMap();
            CreateMap<AnnualReportViewModel, AnnualReportDTO>().ReverseMap();
        }
    }
}