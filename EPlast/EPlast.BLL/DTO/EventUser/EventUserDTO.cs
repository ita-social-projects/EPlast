using EPlast.BLL.DTO.UserProfiles;
using System.Collections.Generic;

namespace EPlast.BLL.DTO.EventUser
{
    public class EventUserDTO
    {
        public UserDTO User { get; set; }
        public ICollection<EventGeneralInfoDTO> PlanedEvents { get; set; }
        public ICollection<EventGeneralInfoDTO> CreatedEvents { get; set; }
        public ICollection<EventGeneralInfoDTO> VisitedEvents { get; set; }
        public ICollection<string> UserRoles { get; set; }
    }
}
