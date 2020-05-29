using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class Gallary
    {
        public int ID { get; set; }
        [Required]
        public string GalaryFileName { get; set; }
        public ICollection<EventGallary> Events { get; set; }
    }
}
