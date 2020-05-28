using EPlast.ViewModels.Events;
using System.Collections.Generic;

namespace EPlast.ViewModels.EventUser
{
    public class EventCreateViewModel
    {
        public EventViewModel Event { get; set; }
        public CreateEventAdminViewModel EventAdmin { get; set; }
        public EventAdministrationViewModel EventAdministration { get; set; }
        public IEnumerable<EventCategoryViewModel> EventCategories { get; set; }
        public IEnumerable<EventTypeViewModel> EventTypes { get; set; }
        public IEnumerable<UserViewModel> Users { get; set; }
    }
}

