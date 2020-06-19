using AutoMapper;
using EPlast.BussinessLayer.DTO.AnnualReport;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Mapping.AnnualReport
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<DatabaseEntities.City, CityAnnualReportDTO>().ReverseMap();
        }
    }
}