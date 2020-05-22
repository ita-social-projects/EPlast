using EPlast.ViewModels.UserInformation.UserProfile;
using System;

namespace EPlast.ViewModels.UserInformation
{
    public class PersonalDataViewModel
    {
        public UserViewModel User { get; set; }
        public TimeSpan TimeToJoinPlast { get; set; }
        public bool IsUserPlastun { get; set; }
    }
}
