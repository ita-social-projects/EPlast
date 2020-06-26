using EPlast.BLL.DTO.UserProfiles;
using System.Collections.Generic;

namespace EPlast.BLL.DTO.Club
{
    public class ClubMembersDTO
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public UserDTO User { get; set; }
        public int ClubId { get; set; }
        public ClubDTO Club { get; set; }
        public bool IsApproved { get; set; }
        public IEnumerable<ClubAdministrationDTO> ClubAdministration { get; set; }
    }
}