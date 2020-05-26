using EPlast.ViewModels.UserInformation.UserProfile;

namespace EPlast.ViewModels
{
    public class UserTableViewModel
    {
        public UserViewModel User { get; set; }
        public string RegionName { get; set; }
        public string CityName { get; set; }
        public string ClubName { get; set; }
        public string UserPlastDegreeName { get; set; }
        public string UserRoles { get; set; }
    }
}