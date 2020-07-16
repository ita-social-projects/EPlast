using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.DTO.EventCalendar
{
    public class EventCalendarInfoDTO
    {
        public int ID { get; set; }
        public string EventName { get; set; }
        public DateTime EventDateStart { get; set; }
        public DateTime EventDateEnd { get; set; }
        public string Eventlocation { get; set; }
        public string Description { get; set; }
    }
}
