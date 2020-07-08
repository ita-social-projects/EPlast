using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.WebApi.Models.City;

namespace EPlast.BLL.Mapping.City
{
    public class CityProfileProfile : Profile
    {
        public CityProfileProfile()
        {
            CreateMap<CityProfileViewModel, CityProfileDTO>().ReverseMap();
        }
    }
}
