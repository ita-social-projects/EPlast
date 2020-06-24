using AutoMapper;
using EPlast.Bussiness.DTO.AnnualReport;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.Bussiness.Mapping.AnnualReport
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<DatabaseEntities.City, CityDTO>().ReverseMap();
        }
    }
}