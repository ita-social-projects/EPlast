using EPlast.BussinessLayer.DTO.UserProfiles;
using System.Collections.Generic;

namespace EPlast.BussinessLayer.DTO.Club
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