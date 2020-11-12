using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities.Event
{
    public class EventCategory
    {
        public int ID { get; set; }
        public string EventCategoryName { get; set; }
        public int EventSectionId { get; set; }
        public EventSection EventSection { get; set; }
        public ICollection<Event> Events { get; set; }
        public ICollection<EventCategoryType> EventTypes { get; set; }
    }
}