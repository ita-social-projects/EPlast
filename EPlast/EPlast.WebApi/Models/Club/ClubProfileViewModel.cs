using EPlast.WebApi.Models.User;
using System.Collections.Generic;

namespace EPlast.WebApi.Models.Club
{
    public class ClubProfileViewModel
    {
        public ClubViewModel Club { get; set; }
        public UserInfoViewModel ClubAdmin { get; set; }
        public IEnumerable<ClubMembersViewModel> Members { get; set; }
        public IEnumerable<ClubMembersViewModel> Followers { get; set; }
        public IEnumerable<ClubAdministrationViewModel> ClubAdministration { get; set; }
        public bool IsCurrentUserClubAdmin { get; set; }
        public bool IsCurrentUserAdmin { get; set; }
        public bool CanJoin { get; set; }
    }
}
