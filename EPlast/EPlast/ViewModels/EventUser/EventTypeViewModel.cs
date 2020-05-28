using System.ComponentModel.DataAnnotations;

namespace EPlast.ViewModels.EventUser
{
    public class EventTypeViewModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Вам необхідно обрати тип події!")]
        public string EventTypeName { get; set; }
    }
}
