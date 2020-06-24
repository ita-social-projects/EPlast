using EPlast.Bussiness.DTO.UserProfiles;

namespace EPlast.Bussiness.DTO
{
    public class UserTableDTO
    {
        public UserDTO User { get; set; }
        public string RegionName { get; set; }
        public string CityName { get; set; }
        public string ClubName { get; set; }
        public string UserPlastDegreeName { get; set; }
        public string UserRoles { get; set; }
    }
}
