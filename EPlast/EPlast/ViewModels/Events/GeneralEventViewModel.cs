using EPlast.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.ViewModels.Events
{
    public class GeneralEventViewModel
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public bool IsUserEventAdmin { get; set; }
        public bool IsUserParticipant { get; set; }
        public bool IsUserApprovedParticipant { get; set; }
        public bool IsUserUndeterminedParticipant { get; set; }
        public bool IsUserRejectedParticipant { get; set; }
        public bool IsEventApproved { get; set; }
        public bool IsEventFinished { get; set; }
        public bool IsEventNotApproved { get; set; }
    }
}
