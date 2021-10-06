using AutoMapper;
using EPlast.BLL.DTO.Club;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.Club
{
    class ClubReportCitiMembersProfile : Profile
    {
        public ClubReportCitiMembersProfile()
        {
            CreateMap<DatabaseEntities.CityMembers, ClubReportCityMembersDTO>().ReverseMap();
        }
    }
}
