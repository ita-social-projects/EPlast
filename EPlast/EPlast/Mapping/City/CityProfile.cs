using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.ViewModels.City;
using DataAccessCity = EPlast.DataAccess.Entities;

namespace EPlast.Mapping.City
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<CityViewModel, CityDTO>().ReverseMap();
        }
    }
}
