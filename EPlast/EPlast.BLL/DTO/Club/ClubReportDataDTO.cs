using System.Collections.Generic;
using EPlast.DataAccess.Entities;

namespace EPlast.BLL.DTO.Club
{
    public class ClubReportDataDTO
    {
        public ClubDTO Club { get; set; }
        public ClubReportAdministrationDTO Head { get; set; }
        public List<ClubReportAdministrationDTO> Admins { get; set; }
        public List<ClubMemberHistoryDTO> Members { get; set; }
        public List<ClubMemberHistoryDTO> Followers { get; set; }
        public int CountUsersPerYear {get;set;}
        public int CountDeletedUsersPerYear { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ClubURL { get; set; }
        public string Slogan { get; set; }
    }
}
