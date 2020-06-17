using System.Collections.Generic;
using EPlast.BussinessLayer.DTO.Club;
using EPlast.BussinessLayer.DTO.UserProfiles;

namespace EPlast.WebApi.Models.Club
{
    public class ClubProfileViewModel
    {
        public ClubDTO Club { get; set; }
        public UserDTO ClubAdmin { get; set; }
        public List<ClubMembersDTO> Members { get; set; }
        public List<ClubMembersDTO> Followers { get; set; }
        public ICollection<ClubAdministrationDTO> ClubAdministration { get; set; }
        public bool IsCurrentUserClubAdmin { get; set; }
        public bool IsCurrentUserAdmin { get; set; }
    }
}
