using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels;

namespace EPlast.Mapping
{
    public class AnnualReportProfile : Profile
    {
        public AnnualReportProfile()
        {
            CreateMap<AnnualReport, AnnualReportDTO>().ReverseMap();
            CreateMap<AnnualReportViewModel, AnnualReportDTO>().ReverseMap();
        }
    }
}