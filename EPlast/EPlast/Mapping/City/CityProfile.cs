using AutoMapper;
using EPlast.BusinessLogicLayer.DTO.City;
using EPlast.ViewModels.City;
using DataAccessCity = EPlast.DataAccess.Entities;

namespace EPlast.Mapping.City
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<CityDTO, DataAccessCity.City>().ReverseMap();
            CreateMap<CityViewModel, CityDTO>().ReverseMap();
        }
    }
}
