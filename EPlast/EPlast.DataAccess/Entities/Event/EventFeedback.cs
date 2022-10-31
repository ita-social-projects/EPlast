using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities.Event
{
    public class EventFeedback
    {
        public int Id { get; set; }
        public double Rating { get; set; }
        public string Text { get; set; }
        public int ParticipantId { get; set; }
        public Participant Participant { get; set; }
    }
}
