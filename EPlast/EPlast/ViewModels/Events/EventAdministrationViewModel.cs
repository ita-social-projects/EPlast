using EPlast.ViewModels.EventUser;
using EPlast.ViewModels.UserInformation.UserProfile;

namespace EPlast.ViewModels.Events
{
    public class EventAdministrationViewModel
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public UserInfoViewModel User { get; set; }
    }

}
