using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.WebApi.Models.City;

namespace EPlast.BLL.Mapping.City
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<CityViewModel, CityDTO>().ReverseMap();
        }
    }
}