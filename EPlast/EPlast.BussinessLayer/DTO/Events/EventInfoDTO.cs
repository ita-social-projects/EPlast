using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BussinessLayer.DTO.Events
{
    public class EventInfoDTO
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        public string EventDateStart { get; set; }
        public string EventDateEnd { get; set; }
        public string EventLocation { get; set; }
        public string EventType { get; set; }
        public string EventCategory { get; set; }
        public string EventStatus { get; set; }
        public string FormOfHolding { get; set; }
        public string ForWhom { get; set; }
        public int NumberOfParticipants { get; set; }
        public List<EventAdminDTO> EventAdmins { get; set; }
        public List<EventParticipantDTO> EventParticipants { get; set; }
        public List<EventGalleryDTO> EventGallery { get; set; }
    }
}
