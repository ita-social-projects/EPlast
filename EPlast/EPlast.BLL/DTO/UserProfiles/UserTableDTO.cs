using EPlast.BLL.DTO.UserProfiles;

namespace EPlast.BLL.DTO
{
    public class UserTableDTO
    {
        public ShortUserInformationDTO User { get; set; }
        public string RegionName { get; set; }
        public string CityName { get; set; }
        public string ClubName { get; set; }
        public string UserPlastDegreeName { get; set; }
        public string UserRoles { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
        public string UPUDegree { get; set; }
    }
}
