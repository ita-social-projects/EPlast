using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.ViewModels.Events
{
    public class EventViewModel
    {
        public Event Event { get; set; }
        public bool IsUserEventAdmin { get; set;}
        public bool IsUserParticipant { get; set; }
        public bool IsUserApprovedParticipant { get; set; }
        public bool IsUserUndeterminedParticipant { get; set; }
        public bool IsUserRejectedParticipant { get; set; }
        public bool IsEventFinished { get; set; }
        public IEnumerable<Participant> EventParticipants { get; set; }

    }
}
