using EPlast.ViewModels.Events;
using System.Collections.Generic;
using EPlast.ViewModels.UserInformation.UserProfile;

namespace EPlast.ViewModels.EventUser
{
    public class EventCreateViewModel
    {
        public EventCreationViewModel Event { get; set; }
        //public CreateEventAdminViewModel EventAdmin { get; set; }
        public EventAdministrationViewModel Сommandant { get; set; }
        public EventAdministrationViewModel Alternate { get; set; }
        public EventAdministrationViewModel Bunchuzhnyi { get; set; }
        public EventAdministrationViewModel Pysar { get; set; }
        public IEnumerable<EventCategoryViewModel> EventCategories { get; set; }
        public IEnumerable<EventTypeViewModel> EventTypes { get; set; }
        public IEnumerable<UserViewModel> Users { get; set; }
    }
}

