using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.WebApi.Models.City;
using System.Collections.Generic;

namespace EPlast.WebApi.Mapping.City
{
    public class CityAdministrationStatusProfile:Profile
    {
        public CityAdministrationStatusProfile()
        {
            CreateMap<CityAdministrationStatusViewModel, CityAdministrationStatusDTO>().ReverseMap();
            CreateMap<IEnumerable<CityAdministrationStatusViewModel>,IEnumerable<CityAdministrationStatusDTO>>().ReverseMap();

        }
    }
}
