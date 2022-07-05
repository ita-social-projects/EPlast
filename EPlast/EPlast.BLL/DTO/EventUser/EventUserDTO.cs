using System.Collections.Generic;
using EPlast.BLL.DTO.UserProfiles;

namespace EPlast.BLL.DTO.EventUser
{
    public class EventUserDto
    {
        public UserDto User { get; set; }
        public ICollection<EventGeneralInfoDto> PlanedEvents { get; set; }
        public ICollection<EventGeneralInfoDto> CreatedEvents { get; set; }
        public ICollection<EventGeneralInfoDto> VisitedEvents { get; set; }
        public ICollection<string> UserRoles { get; set; }
    }
}
