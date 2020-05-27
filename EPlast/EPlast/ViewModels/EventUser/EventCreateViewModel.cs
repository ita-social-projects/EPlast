using System.Collections.Generic;
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EPlast.ViewModels.EventUser
{
    public class EventCreateViewModel
    {
        public Event Event { get; set; }
        public EventAdmin EventAdmin { get; set; }
        public EventAdministration EventAdministration { get; set; }
        public IEnumerable<SelectListItem> EventCategories { get; set; }
        public IEnumerable<EventType> EventTypes { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}

