using AutoMapper;
using EPlast.BLL.DTO.City;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.City
{
    public class CityLegalStatusProfile : Profile
    {
        public CityLegalStatusProfile()
        {
            CreateMap<DatabaseEntities.CityLegalStatus, CityLegalStatusDto>().ReverseMap();
        }
    }
}
