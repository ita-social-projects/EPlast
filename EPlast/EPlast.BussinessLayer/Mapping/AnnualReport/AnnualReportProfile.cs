using AutoMapper;
using EPlast.BussinessLayer.DTO.AnnualReport;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Mapping.AnnualReport
{
    public class AnnualReportProfile : Profile
    {
        public AnnualReportProfile()
        {
            CreateMap<DatabaseEntities.AnnualReport, AnnualReportDTO>().ReverseMap();
        }
    }
}