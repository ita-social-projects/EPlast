using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities.Event
{
    public class EventAdmin
    {
        public int EventID { get; set; }
        public Event Event { get; set; }
        [Required(ErrorMessage = "Вам потрібно обрати Коменданта!")]
        public string UserID { get; set; }
        public User User { get; set; }
    }
}
