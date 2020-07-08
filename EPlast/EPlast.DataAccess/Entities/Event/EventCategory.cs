using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities.Event
{
    public class EventCategory
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Вам потрібно обрати категорію події")]
        public string EventCategoryName { get; set; }
        public ICollection<Event> Events { get; set; }
        public ICollection<EventCategoryType> EventTypes { get; set; }
    }
}