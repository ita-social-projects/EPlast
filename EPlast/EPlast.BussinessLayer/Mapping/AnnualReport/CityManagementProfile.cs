using AutoMapper;
using EPlast.BussinessLayer.DTO.AnnualReport;
using EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Mapping.AnnualReport
{
    public class CityManagementProfile : Profile
    {
        public CityManagementProfile()
        {
            CreateMap<CityManagement, CityManagementDTO>().ReverseMap();
        }
    }
}