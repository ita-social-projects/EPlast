using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.WebApi.Models.City;

namespace EPlast.BLL.Mapping.City
{
    public class CityUserProfile : Profile
    {
        public CityUserProfile()
        {
            CreateMap<CityUserViewModel, CityUserDto>().ReverseMap();
        }
    }
}
