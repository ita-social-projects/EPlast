using System.Collections.Generic;
using EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.DTO.Events
{
    public class EventDTO
    {
        public Event Event { get; set; }
        public bool IsUserEventAdmin { get; set; }
        public bool IsUserParticipant { get; set; }
        public bool IsUserApprovedParticipant { get; set; }
        public bool IsUserUndeterminedParticipant { get; set; }
        public bool IsUserRejectedParticipant { get; set; }
        public bool IsEventFinished { get; set; }
        public IEnumerable<Participant> EventParticipants { get; set; }
    }
}
