using EPlast.DataAccess.Entities;
using System;
using System.Collections.Generic;

namespace EPlast.ViewModels
{
    public class UserViewModel
    {
        public User User { get; set; }
        public IEnumerable<CityAdministration> UserPositions { get; set; }

        public ICollection<Approver> Approvers { get; set; }

        public UserViewModel()
        {
            Approvers = new List<Approver>();
        }
        public bool HasAccessToManageUserPositions { get; set; }
        public bool canApprove { get; set; }
        public TimeSpan timeToJoinPlast { get; set; }
    }
}
