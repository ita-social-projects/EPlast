using EPlast.DataAccess.Entities;
using System;
using System.Collections.Generic;

namespace EPlast.ViewModels
{
    public class UserViewModel
    {
        public User User { get; set; }
        public IEnumerable<CityAdministration> UserPositions { get; set; }

        public ICollection<ConfirmedUser> ConfirmedUsers { get; set; }
        public bool HasAccessToManageUserPositions { get; set; }
        public bool canApprove { get; set; }
        public TimeSpan timeToJoinPlast { get; set; }
        public ConfirmedUser ClubApprover { get; set; }
        public ConfirmedUser CityApprover { get; set; }
    }
}
