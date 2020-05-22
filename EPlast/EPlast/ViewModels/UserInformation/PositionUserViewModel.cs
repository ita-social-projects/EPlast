using EPlast.DataAccess.Entities;
using System;
using System.Collections.Generic;

namespace EPlast.ViewModels.UserInformation
{
    public class PositionUserViewModel
    {
        public User User { get; set; } 
        public IEnumerable<CityAdministration> UserPositions { get; set; }
        public bool HasAccessToManageUserPositions { get; set; }
        public TimeSpan TimeToJoinPlast { get; set; }
    }
}
