using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.ViewModels.Events
{
    public class EventInfoViewModel
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        public string EventDateStart { get; set; }
        public string EventDateEnd { get; set; }
        public string EventLocation { get; set; }
        public string EventType { get; set; }
        public string EventCategory { get; set; }
        public string EventStatus { get; set; }
        public string FormOfHolding { get; set; }
        public string ForWhom { get; set; }
        public int NumberOfParticipants { get; set; }
        public List<EventAdminViewModel> EventAdmins { get; set; }
        public IEnumerable<EventParticipantViewModel> EventParticipants { get; set; }
        public List<EventGalleryViewModel> EventGallery { get; set; }
    }
}
