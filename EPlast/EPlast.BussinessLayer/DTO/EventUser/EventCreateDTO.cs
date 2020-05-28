using EPlast.BussinessLayer.DTO.Events;
using System.Collections.Generic;

namespace EPlast.BussinessLayer.DTO.EventUser
{
    public class EventCreateDTO
    {
        public EventDTO Event { get; set; }
        public CreateEventAdminDTO EventAdmin { get; set; }
        public EventAdministrationDTO EventAdministration { get; set; }
        public IEnumerable<EventCategoryDTO> EventCategories { get; set; }
        public IEnumerable<EventTypeDTO> EventTypes { get; set; }
        public IEnumerable<UserDTO> Users { get; set; }
    }
}
