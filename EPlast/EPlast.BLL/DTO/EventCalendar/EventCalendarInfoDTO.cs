using System;

namespace EPlast.BLL.DTO.EventCalendar
{
    public class EventCalendarInfoDto
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Eventlocation { get; set; }
        public string Description { get; set; }
        public int EventTypeID { get; set; }
    }
}
