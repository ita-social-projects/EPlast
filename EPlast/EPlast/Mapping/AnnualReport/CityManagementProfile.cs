using AutoMapper;
using EPlast.BLL.DTO.AnnualReport;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels.AnnualReport;

namespace EPlast.Mapping.AnnualReport
{
    public class CityManagementProfile : Profile
    {
        public CityManagementProfile()
        {
            CreateMap<CityManagement, CityManagementDTO>().ReverseMap();
            CreateMap<CityManagementViewModel, CityManagementDTO>().ReverseMap();
        }
    }
}