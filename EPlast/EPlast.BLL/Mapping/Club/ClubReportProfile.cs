using AutoMapper;
using EPlast.BLL.DTO.Club;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Mapping.Club
{
    public class ClubReportProfile:Profile
    {
        public ClubReportProfile()
        {
            CreateMap<DatabaseEntities.Club, ClubReportDto>().ReverseMap();
        }
    }
}
