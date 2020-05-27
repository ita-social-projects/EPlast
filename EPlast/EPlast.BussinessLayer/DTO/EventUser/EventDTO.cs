using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BussinessLayer.DTO.EventUser
{
    public class EventDTO
    {
        public int ID { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        public string Questions { get; set; }
        public DateTime EventDateStart { get; set; }
        public DateTime EventDateEnd { get; set; }
        public string Eventlocation { get; set; }
        public int EventTypeID { get; set; }
        public int EventCategoryID { get; set; }
        public int EventStatusID { get; set; }
        public string FormOfHolding { get; set; }
        public string ForWhom { get; set; }
        public int NumberOfPartisipants { get; set; }
    }
}
