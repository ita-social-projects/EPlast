using System;
using System.Collections.Generic;
using EPlast.ViewModels.UserInformation.UserProfile;

namespace EPlast.ViewModels.UserInformation
{
    public class UserApproversViewModel
    {
        public UserViewModel User { get; set; }
        public IEnumerable<ConfirmedUserViewModel> ConfirmedUsers { get; set; }
        public bool canApprove { get; set; }
        public TimeSpan TimeToJoinPlast { get; set; }
        public ConfirmedUserViewModel ClubApprover { get; set; }
        public ConfirmedUserViewModel CityApprover { get; set; }
        public bool IsUserPlastun { get; set; }
        public bool IsUserHeadOfClub { get; set; }
        public bool IsUserHeadOfCity { get; set; }
        public bool IsUserHeadOfRegion { get; set; }
        public string CurrentUserId { get; set; }
    }
}
