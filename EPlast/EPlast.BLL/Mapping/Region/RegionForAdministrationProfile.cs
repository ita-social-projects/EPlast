using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.Region;
using DataAccessCity = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.Region
{
    public class RegionForAdministrationProfile : Profile
    {
        public RegionForAdministrationProfile()
        {
            CreateMap<DataAccessCity.Region, RegionForAdministrationDto>().ReverseMap();
        }
    }
}
