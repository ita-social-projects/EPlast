using AutoMapper;
using DatabaseEntities = EPlast.DataAccess.Entities;
using DTOs = EPlast.BLL.DTO.Statistics;

namespace EPlast.BLL.Mapping.Statistics
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<DTOs.City, DatabaseEntities.City>().ReverseMap();
        }
    }
}