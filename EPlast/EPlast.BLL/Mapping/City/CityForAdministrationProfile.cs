using AutoMapper;
using EPlast.BLL.DTO.City;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessCity = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.City
{
    public class CityForAdministrationProfile : Profile
    {
        public CityForAdministrationProfile()
        {
            CreateMap<DataAccessCity.City, CityForAdministrationDTO>().ReverseMap();
        }
    }
}
