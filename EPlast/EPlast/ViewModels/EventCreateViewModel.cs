
using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EPlast.ViewModels.Events
{
    public class EventCreateViewModel
    {
        public Event Event { get; set; }
        public EventAdmin EventAdmin { get; set; }
        public EventAdministration EventAdministration { get; set; }
        public IEnumerable<SelectListItem> EventCategories { get; set; }
        public IEnumerable<EventType> EventTypes { get; set; }
        public IEnumerable<EventAdmin> EventAdmins { get; set; }
        public EventStatus EventStatuses { get; set; }
        public IEnumerable<EventAdministration> EventAdministrations { get; set; }
        public IEnumerable<User> Users { get; set; }
        public User User { get; set; }
    }
}

