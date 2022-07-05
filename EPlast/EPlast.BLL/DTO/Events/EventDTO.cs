namespace EPlast.BLL.DTO.Events
{
    public class EventDto
    {
        public EventInfoDto Event { get; set; }
        public double ParticipantAssessment { get; set; }
        public bool IsUserEventAdmin { get; set; }
        public bool IsUserParticipant { get; set; }
        public bool IsUserApprovedParticipant { get; set; }
        public bool IsUserUndeterminedParticipant { get; set; }
        public bool IsUserRejectedParticipant { get; set; }
        public bool IsEventFinished { get; set; }
        public bool CanEstimate { get; set; }
    }
}
