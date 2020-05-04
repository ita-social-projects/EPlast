using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class EventStatus
    {
        public int ID { get; set; }
        [Required]
        public string EventStatusName { get; set; }
        public ICollection<Event> Events { get; set; }

    }
}