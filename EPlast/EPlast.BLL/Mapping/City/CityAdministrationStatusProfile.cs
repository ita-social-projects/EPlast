using AutoMapper;
using EPlast.BLL.DTO.City;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.City
{
    public class CityAdministrationStatusProfile:Profile
    {
        public CityAdministrationStatusProfile()
        {
            CreateMap<DatabaseEntities.CityAdministration, CityAdministrationStatusDto>().ReverseMap();

        }
    }
}
