using EPlast.ViewModels.UserProfile;
using System.Collections.Generic;

namespace EPlast.ViewModels
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
    }
}
