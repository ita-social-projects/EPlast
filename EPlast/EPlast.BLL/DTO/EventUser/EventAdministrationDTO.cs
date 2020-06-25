
using EPlast.BLL.DTO.UserProfiles;

namespace EPlast.BLL.DTO.EventUser
{
    public class EventAdministrationDTO
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public UserDTO User { get; set; }
    }
}
