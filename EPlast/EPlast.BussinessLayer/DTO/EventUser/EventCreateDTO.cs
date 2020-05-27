using System.Collections.Generic;
using EPlast.BussinessLayer.DTO.Events;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EPlast.BussinessLayer.DTO.EventUser
{
    public class EventCreateDTO
    {
        public EventDTO Event { get; set; }
        public CreateEventAdminDTO EventAdmin { get; set; }
        public EventAdministration EventAdministration { get; set; }
        public IEnumerable<EventCategoryDTO> EventCategories { get; set; }
        public IEnumerable<EventTypeDTO> EventTypes { get; set; }
        public IEnumerable<UserDTO> Users { get; set; }
    }
}
