using EPlast.ViewModels.Club;
using System.Collections.Generic;

namespace EPlast.ViewModels
{
    public class ClubProfileViewModel
    {
        public ClubViewModel Club { get; set; }
        public ClubAdministrationViewModel Head { get; set; }
        public List<ClubAdministrationViewModel> Admins { get; set; }
        public List<ClubMembersViewModel> Members { get; set; }
        public List<ClubMembersViewModel> Followers { get; set; }
        public List<ClubDocumentViewModel> Documents { get; set; }
    }
}
