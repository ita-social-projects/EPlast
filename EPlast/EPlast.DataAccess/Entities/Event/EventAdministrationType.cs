using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EPlast.DataAccess.Entities.Event
{
    public class EventAdministrationType
    {
        public int ID { get; set; }
        [Required]
        public string EventAdministrationTypeName { get; set; }
        public ICollection<EventAdministration> EventAdministrations { get; set; }
    }
}