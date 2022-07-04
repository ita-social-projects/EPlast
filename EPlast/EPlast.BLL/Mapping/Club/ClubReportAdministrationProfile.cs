using AutoMapper;
using EPlast.BLL.DTO.Club;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.Club
{
    public class ClubReportAdministrationProfile: Profile
    {
        public ClubReportAdministrationProfile()
        {
            CreateMap<DatabaseEntities.ClubAdministration, ClubReportAdministrationDto>().ReverseMap();
        }
    }
}
