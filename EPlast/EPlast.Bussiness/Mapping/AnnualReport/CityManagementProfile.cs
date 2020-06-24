using AutoMapper;
using EPlast.BusinessLogicLayer.DTO.AnnualReport;
using EPlast.DataAccess.Entities;

namespace EPlast.BusinessLogicLayer.Mapping.AnnualReport
{
    public class CityManagementProfile : Profile
    {
        public CityManagementProfile()
        {
            CreateMap<CityManagement, CityManagementDTO>().ReverseMap();
        }
    }
}