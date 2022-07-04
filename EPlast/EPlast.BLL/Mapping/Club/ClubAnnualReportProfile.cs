using AutoMapper;
using EPlast.BLL.DTO.Club;
using DatabaseEntities = EPlast.DataAccess.Entities;


namespace EPlast.BLL.Mapping.Club
{
    public class ClubAnnualReportProfile:Profile
    {
        public ClubAnnualReportProfile()
        {
            CreateMap<DatabaseEntities.ClubAnnualReport, ClubAnnualReportDto>().ReverseMap();
        }
    }
}
