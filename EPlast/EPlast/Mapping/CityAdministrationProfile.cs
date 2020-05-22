using AutoMapper;
using EPlast.BussinessLayer.DTO;
using EPlast.DataAccess.Entities;
using EPlast.ViewModels;

namespace EPlast.Mapping
{
    public class CityAdministrationProfile:Profile
    {
        public CityAdministrationProfile()
        {
            CreateMap<CityAdministration, CityAdministrationDTO>().ReverseMap();
            CreateMap<CityAdministrationViewModel, CityAdministrationDTO>().ReverseMap();
        }
    }
}
