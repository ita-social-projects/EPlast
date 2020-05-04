using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class ParticipantStatus
    {
        public int ID { get; set; }
        [Required]
        public string ParticipantStatusName { get; set; }
        public ICollection<Participant> Participants { get; set; }

    }
}
