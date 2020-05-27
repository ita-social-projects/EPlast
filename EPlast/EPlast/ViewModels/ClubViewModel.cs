using EPlast.DataAccess.Entities;
using EPlast.ViewModels.Club;
using EPlast.ViewModels.UserInformation.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.ViewModels
{
    public class ClubViewModel
    {
        public CLubViewModel Club { get; set; }
        public UserViewModel ClubAdmin { get; set; }
        public List<ClubMembersViewModel> Members { get; set; }
        public List<ClubMembersViewModel> Followers { get; set; }
    }
}
