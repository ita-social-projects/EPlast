using EPlast.BusinessLogicLayer.DTO.Events;
using System.Collections.Generic;
using EPlast.BusinessLogicLayer.DTO.UserProfiles;

namespace EPlast.BusinessLogicLayer.DTO.EventUser
{
    public class EventCreateDTO
    {
        public EventCreationDTO Event { get; set; }
        public CreateEventAdminDTO EventAdmin { get; set; }
        public EventAdministrationDTO EventAdministration { get; set; }
        public IEnumerable<EventCategoryDTO> EventCategories { get; set; }
        public IEnumerable<EventTypeDTO> EventTypes { get; set; }
        public IEnumerable<UserDTO> Users { get; set; }
    }
}
