using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.WebApi.Models.City;

namespace EPlast.BLL.Mapping.City
{
    public class CityAdministrationProfile : Profile
    {
        public CityAdministrationProfile()
        {
            CreateMap<CityAdministrationViewModel, CityAdministrationDto>().ReverseMap();
        }
    }
}
