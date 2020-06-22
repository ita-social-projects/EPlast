using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities.Event
{
    public class EventAdministration
    {
        public int ID { get; set; }
        public EventAdministrationType EventAdministrationType { get; set; }
        [Required]
        public int EventAdministrationTypeID { get; set; }
        public Event Event { get; set; }
        public int EventID { get; set; }
        public User User { get; set; }
        [Required]
        public string UserID { get; set; }
    }
}