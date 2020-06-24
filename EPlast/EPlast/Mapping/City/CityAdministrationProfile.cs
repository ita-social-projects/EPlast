using AutoMapper;
using EPlast.BusinessLogicLayer.DTO.City;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels.City;

namespace EPlast.Mapping.City
{
    public class CityAdministrationProfile : Profile
    {
        public CityAdministrationProfile()
        {
            CreateMap<CityAdministration, CityAdministrationDTO>().ReverseMap();
            CreateMap<CityAdministrationViewModel, CityAdministrationDTO>().ReverseMap();
        }
    }
}
