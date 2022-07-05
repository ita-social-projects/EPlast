using AutoMapper;
using EPlast.BLL.DTO.Club;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.Club
{
    public class ClubAdministrationStatusProfile:Profile
    {
        public ClubAdministrationStatusProfile()
        {
            CreateMap<DatabaseEntities.ClubAdministration, ClubAdministrationStatusDto>().ReverseMap();
        }
    }
}
