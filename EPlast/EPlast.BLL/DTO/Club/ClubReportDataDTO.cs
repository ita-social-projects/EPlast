using System.Collections.Generic;

namespace EPlast.BLL.DTO.Club
{
    public class ClubReportDataDTO
    {
        public ClubDTO Club { get; set; }
        public ClubAdministrationDTO Head { get; set; }
        public List<ClubAdministrationDTO> Admins { get; set; }
        public List<ClubMemberHistoryDTO> Members { get; set; }
        public List<ClubMemberHistoryDTO> Followers { get; set; }
        public int CountUsersPerYear {get;set;}
        public int CountdeletedUsersPerYear { get; set; }
    }
}
