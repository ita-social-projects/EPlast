using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities.Event
{
    public class EventStatus
    {
        public int ID { get; set; }
        [Required]
        public string EventStatusName { get; set; }
        public ICollection<Entities.Event.Event> Events { get; set; }

    }
}