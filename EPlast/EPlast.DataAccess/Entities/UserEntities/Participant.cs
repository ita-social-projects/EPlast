using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class Participant
    {
        public int ID { get; set; }
        [Required]
        public int ParticipantStatusId { get; set; }
        public ParticipantStatus ParticipantStatus { get; set; }
        [Required]
        public int EventId { get; set; }
        public Event.Event Event { get; set; }
        [Required]
        public string UserId { get; set; }
        [DefaultValue(0)]
        public bool WasPresent { get; set; } 
        public double Estimate { get; set; }
        public User User { get; set; }
    }
}
