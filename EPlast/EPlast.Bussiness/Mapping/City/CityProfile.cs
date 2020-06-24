using AutoMapper;
using EPlast.Bussiness.DTO.City;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.Bussiness.Mapping.City
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<DatabaseEntities.City, CityDTO>().ReverseMap();
        }
    }
}