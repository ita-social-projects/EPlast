using System.Collections.Generic;

namespace EPlast.BLL.DTO.Events
{
    public class EventInfoDto
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        public string EventDateStart { get; set; }
        public string EventDateEnd { get; set; }
        public string EventLocation { get; set; }
        public int EventTypeId { get; set; }
        public string EventType { get; set; }
        public int EventCategoryId { get; set; }
        public int NumberOfPartisipants { get; set; }
        public string EventCategory { get; set; }
        public string EventStatus { get; set; }
        public string FormOfHolding { get; set; }
        public string ForWhom { get; set; }
        public double Rating { get; set; }
        public List<EventAdminDto> EventAdmins { get; set; }
        public IEnumerable<EventParticipantDto> EventParticipants { get; set; }
    }
}
