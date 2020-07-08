using EPlast.BLL.DTO.Events;
using System.Collections.Generic;

namespace EPlast.BLL.DTO.EventUser
{
    public class EventCreateDTO
    {
        public EventCreationDTO Event { get; set; }
        public EventAdministrationDTO Сommandant { get; set; }
        public EventAdministrationDTO Alternate { get; set; }
        public EventAdministrationDTO Bunchuzhnyi { get; set; }
        public EventAdministrationDTO Pysar { get; set; }
        public IEnumerable<EventCategoryDTO> EventCategories { get; set; }
        public IEnumerable<EventTypeDTO> EventTypes { get; set; }
        public IEnumerable<UserInfoDTO> Users { get; set; }
    }
}
