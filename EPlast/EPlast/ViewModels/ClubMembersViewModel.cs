using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.ViewModels.UserInformation.UserProfile;
using System.ComponentModel.DataAnnotations;

namespace EPlast.ViewModels
{
    public class ClubMembersViewModel
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public UserViewModel User { get; set; }
        public int ClubId { get; set; }
        public ClubViewModel Club { get; set; }
        [Required]
        public bool IsApproved { get; set; }
        public ICollection<ClubAdministrationViewModel> ClubAdministration { get; set; }
    }
}
