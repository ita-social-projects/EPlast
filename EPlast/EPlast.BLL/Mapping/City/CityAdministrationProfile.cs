using AutoMapper;
using EPlast.BLL.DTO.City;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.City
{
    public class CityAdministrationProfile : Profile
    {
        public CityAdministrationProfile()
        {
            CreateMap<DatabaseEntities.CityAdministration, CityAdministrationDTO>().ReverseMap();
            CreateMap<DatabaseEntities.CityAdministration, CityAdministrationGetDTO>().ReverseMap();
        }
    }
}
