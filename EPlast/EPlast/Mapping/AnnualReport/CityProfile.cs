using AutoMapper;
using EPlast.BLL.DTO.AnnualReport;
using EPlast.ViewModels.AnnualReport;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.Mapping.AnnualReport
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<DatabaseEntities.City, CityDTO>().ReverseMap();
            CreateMap<CityViewModel, CityDTO>().ReverseMap();
        }
    }
}