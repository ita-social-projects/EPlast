using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities.Event
{
    public class EventType
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Вам необхідно обрати тип події!")]
        public string EventTypeName { get; set; }
        public ICollection<Event> Events { get; set; }
        public ICollection<EventCategoryType> EventCategories { get; set; }
    }
}