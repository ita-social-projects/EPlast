using AutoMapper;
using EPlast.BussinessLayer.DTO;
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