using System;

namespace EPlast.BLL.DTO.EventUser
{
    public class EventCreationDTO
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
