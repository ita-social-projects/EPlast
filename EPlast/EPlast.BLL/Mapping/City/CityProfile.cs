using AutoMapper;
using EPlast.BLL.DTO.City;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.City
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<DatabaseEntities.City, CityDto>().ReverseMap();
            CreateMap<DatabaseEntities.CityObject, CityObjectDto>().ReverseMap();
        }
    }
}