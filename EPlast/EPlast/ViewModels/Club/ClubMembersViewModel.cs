using EPlast.ViewModels.UserInformation.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.ViewModels.Club
{
    public class ClubMembersViewModel
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public UserViewModel User { get; set; }
        public int ClubId { get; set; }
        public CLubViewModel Club { get; set; }
        public bool IsApproved { get; set; }
        public IEnumerable<ClubAdministrationViewModel> ClubAdministration { get; set; }
    }
}
