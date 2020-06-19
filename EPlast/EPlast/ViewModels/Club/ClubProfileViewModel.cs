using EPlast.ViewModels.UserProfile;
using System.Collections.Generic;

namespace EPlast.ViewModels
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
