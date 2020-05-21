using EPlast.ViewModels.UserInformation.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.ViewModels.UserInformation
{
    public class PersonalDataViewModel
    {
        public UserViewModel User { get; set; }
        public TimeSpan timeToJoinPlast { get; set; }
        public bool IsUserPlastun { get; set; }
    }
}
