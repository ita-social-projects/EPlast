using System.Collections.Generic;
using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.WebApi.Models.City;

namespace EPlast.WebApi.Mapping.City
{
    public class CityAdministrationStatusProfile:Profile
    {
        public CityAdministrationStatusProfile()
        {
            CreateMap<CityAdministrationStatusViewModel, CityAdministrationStatusDto>().ReverseMap();
            CreateMap<IEnumerable<CityAdministrationStatusViewModel>, IEnumerable<CityAdministrationStatusDto>>().ReverseMap();

        }
    }
}
