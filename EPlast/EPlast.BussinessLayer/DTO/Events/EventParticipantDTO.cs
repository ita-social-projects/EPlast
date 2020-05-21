using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BussinessLayer.DTO.Events
{
    public class EventParticipantDTO
    {
        public int ParticipantId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
    }
}
