using EPlast.BLL.DTO.UserProfiles;
using System.Collections.Generic;

namespace EPlast.BLL.DTO.Club
{
    public class ClubProfileDTO
    {
        public ClubDTO Club { get; set; }
        public UserDTO ClubAdmin { get; set; }
        public IEnumerable<ClubMembersDTO> Members { get; set; }
        public IEnumerable<ClubMembersDTO> Followers { get; set; }
        public IEnumerable<ClubAdministrationDTO> ClubAdministration { get; set; }
    }
}