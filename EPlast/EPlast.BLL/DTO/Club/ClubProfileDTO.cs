using EPlast.BLL.DTO.UserProfiles;
using System.Collections.Generic;

namespace EPlast.BLL.DTO.Club
{
    public class ClubProfileDTO
    {
        public ClubDTO Club { get; set; }
        public ClubAdministrationDTO Head { get; set; }
        public ClubAdministrationDTO HeadDeputy { get; set; }
        public List<ClubAdministrationDTO> Admins { get; set; }
        public List<ClubMembersDTO> Members { get; set; }
        public List<ClubMembersDTO> Followers { get; set; }
        public IEnumerable<ClubDocumentsDTO> Documents { get; set; }
    }
}