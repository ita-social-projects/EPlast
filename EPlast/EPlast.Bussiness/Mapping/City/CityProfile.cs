using AutoMapper;
using EPlast.BusinessLogicLayer.DTO.City;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BusinessLogicLayer.Mapping.City
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<DatabaseEntities.City, CityDTO>().ReverseMap();
        }
    }
}