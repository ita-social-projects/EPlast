using AutoMapper;
using EPlast.BussinessLayer.DTO.City;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Mapping.City
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<DatabaseEntities.City, CityDTO>().ReverseMap();
        }
    }
}