using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public User User { get; set; }
    }
}
