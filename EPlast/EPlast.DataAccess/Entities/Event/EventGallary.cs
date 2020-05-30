using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class EventGallary
    {
        public int EventID { get; set; }
        public Event Event { get; set; }
        public int GallaryID { get; set; }
        public Gallary Gallary { get; set; }
    }
}
