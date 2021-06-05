using EPlast.WebApi.Models.Approver;
using EPlast.WebApi.Models.User;
using System.Collections.Generic;

namespace EPlast.WebApi.Models.UserModels
{
    public class UserApproversViewModel
    {
        public UserInfoViewModel User { get; set; }
        public IEnumerable<ConfirmedUserViewModel> ConfirmedUsers { get; set; }
        public bool canApprove { get; set; }
        public int TimeToJoinPlast { get; set; }
        public ConfirmedUserViewModel ClubApprover { get; set; }
        public ConfirmedUserViewModel CityApprover { get; set; }
        public bool IsUserPlastun { get; set; }
        public bool IsUserHeadOfClub { get; set; }
        public bool IsUserHeadDeputyOfClub { get; set; }
        public bool IsUserHeadOfCity { get; set; }
        public bool IsUserHeadDeputyOfCity { get; set; }
        public bool IsUserHeadOfRegion { get; set; }
        public string CurrentUserId { get; set; }
    }
}
