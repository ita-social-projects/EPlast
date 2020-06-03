using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities.Event
{
    public class EventCategory
    {
        public int ID { get; set; }
        [Required]
        public string EventCategoryName { get; set; }
        public ICollection<Entities.Event.Event> Events { get; set; }
    }
}