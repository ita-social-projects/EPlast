using AutoMapper;
using EPlast.BLL.DTO.City;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.City
{
    public class CityUserProfile : Profile
    {
        public CityUserProfile()
        {
            CreateMap<DatabaseEntities.User, CityUserDto>().ReverseMap();
        }
    }
}
