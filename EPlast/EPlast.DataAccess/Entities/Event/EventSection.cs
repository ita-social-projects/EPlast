using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities.Event
{
    public class EventSection
    {
        public int ID { get; set; }
        public string EventSectionName { get; set; }
        public ICollection<EventCategory> EventCategories { get; set; }
    }
}
