using AutoMapper;
using EPlast.BLL.DTO.Club;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.Club
{
    class ClubReportCitiesProfile : Profile
    {
        public ClubReportCitiesProfile()
        {
            CreateMap<DatabaseEntities.ClubReportCities, ClubReportCitiesDto>().ReverseMap();
        }
    }
}
