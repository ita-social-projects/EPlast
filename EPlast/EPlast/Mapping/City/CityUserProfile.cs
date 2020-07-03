using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.ViewModels.City;

namespace EPlast.Mapping.City
{
    public class CityUserProfile : Profile
    {
        public CityUserProfile()
        {
            CreateMap<CityUserViewModel, CityUserDTO>().ReverseMap();
        }
    }
}
