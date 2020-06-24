using AutoMapper;
using EPlast.Bussiness.DTO.AnnualReport;
using EPlast.DataAccess.Entities;

namespace EPlast.Bussiness.Mapping.AnnualReport
{
    public class CityManagementProfile : Profile
    {
        public CityManagementProfile()
        {
            CreateMap<CityManagement, CityManagementDTO>().ReverseMap();
        }
    }
}