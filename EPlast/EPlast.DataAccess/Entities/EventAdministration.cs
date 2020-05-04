using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class EventAdministration
    {
        public int ID { get; set; }
        public string AdministrationType { get; set; }
        public Event Event { get; set; }
        public int EventID { get; set; }
        public User User { get; set; }
        [Required(ErrorMessage = "Ви повинні обрати адміністрацію події")]
        public string UserID { get; set; }
    }
}
