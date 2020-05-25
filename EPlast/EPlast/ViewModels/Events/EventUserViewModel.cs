using EPlast.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EPlast.ViewModels.Events
{
    public class EventUserViewModel
    {
        public User User { get; set; }
        public int CreatedEventCount { get; set; }
        public int VisitedEventsCount { get; set; }
        public int PlanedEventCount { get; set; }
        public ParticipantStatus ParticipantStatus { get; set; }
        public ICollection<EventAdmin> EventAdmins { get; set; }
        public ICollection<Event> PlanedEvents { get; set; }
        public ICollection<Event> CreatedEvents { get; set; }
        public ICollection<Event> VisitedEvents { get; set; }
        public ICollection<Participant> Participants { get; set; }
    }
}