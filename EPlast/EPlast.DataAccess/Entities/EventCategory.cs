using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class EventCategory
    {
        public int ID { get; set; }
        [Required]
        public string EventCategoryName { get; set; }
        public ICollection<Event> Events { get; set; }
    }
}