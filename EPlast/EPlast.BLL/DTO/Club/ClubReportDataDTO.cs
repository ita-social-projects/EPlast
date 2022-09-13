using System.Collections.Generic;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.DTO.Club
{
    public class ClubReportDataDto
    {
        public ClubDto Club { get; set; }
        public ClubReportAdministrationDto Head { get; set; }
        public List<ClubReportAdministrationDto> Admins { get; set; }
        public List<ClubMemberHistoryDto> Members { get; set; }
        public List<ClubMemberHistoryDto> Followers { get; set; }
        public int CountUsersPerYear {get;set;}
        public int CountDeletedUsersPerYear { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ClubURL { get; set; }
        public string Slogan { get; set; }
    }
}
