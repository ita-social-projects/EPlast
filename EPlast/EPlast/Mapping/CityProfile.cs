using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels;

namespace EPlast.Mapping
{
    public class CityProfile:Profile
    {
        public CityProfile()
        {
            CreateMap<City, CityDTO>().ReverseMap();
            CreateMap<CityViewModel2, CityDTO>().ReverseMap();
        }
    }
}
