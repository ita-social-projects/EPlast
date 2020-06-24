using EPlast.ViewModels.UserProfile;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.ViewModels
{
    public class ClubMembersViewModel
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public UserInfoViewModel User { get; set; }
        public int ClubId { get; set; }
        public ClubViewModel Club { get; set; }
        [Required]
        public bool IsApproved { get; set; }
        public IEnumerable<ClubAdministrationViewModel> ClubAdministration { get; set; }
    }
}
