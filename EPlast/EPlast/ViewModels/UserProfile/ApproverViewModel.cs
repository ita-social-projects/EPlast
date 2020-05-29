using EPlast.ViewModels.UserInformation.UserProfile;

namespace EPlast.ViewModels
{
    public class ApproverViewModel
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public UserViewModel User { get; set; }
        public ConfirmedUserViewModel ConfirmedUser { get; set; }
    }
}
