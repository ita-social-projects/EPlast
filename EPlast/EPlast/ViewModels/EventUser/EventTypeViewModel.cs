using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.ViewModels.EventUser
{
    public class EventTypeViewModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Вам необхідно обрати тип події!")]
        public string EventTypeName { get; set; }
    }
}
