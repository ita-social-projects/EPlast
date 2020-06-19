using EPlast.WebApi.Models.User;
using System.Collections.Generic;

namespace EPlast.WebApi.Models.Club
{
    public class ClubProfileViewModel
    {
        public ClubViewModel Club { get; set; }
        public UserInfoViewModel ClubAdmin { get; set; }
        public List<ClubMembersViewModel> Members { get; set; }
        public List<ClubMembersViewModel> Followers { get; set; }
        public ICollection<ClubAdministrationViewModel> ClubAdministration { get; set; }
        public bool IsCurrentUserClubAdmin { get; set; }
        public bool IsCurrentUserAdmin { get; set; }
    }
}
