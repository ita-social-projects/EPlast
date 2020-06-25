using AutoMapper;
using EPlast.BLL.DTO.AnnualReport;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.AnnualReport
{
    public class CityManagementProfile : Profile
    {
        public CityManagementProfile()
        {
            CreateMap<CityManagement, CityManagementDTO>().ReverseMap();
        }
    }
}