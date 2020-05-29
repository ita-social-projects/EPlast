using System.Collections.Generic;
using EPlast.ViewModels.UserInformation.UserProfile;

namespace EPlast.ViewModels.EventUser
{
    public class EventUserViewModel
    {
        public UserViewModel User { get; set; }
        public ICollection<EventGeneralInfoViewModel> PlanedEvents { get; set; }
        public ICollection<EventGeneralInfoViewModel> CreatedEvents { get; set; }
        public ICollection<EventGeneralInfoViewModel> VisitedEvents { get; set; }

    }
}