namespace EPlast.Bussiness.DTO.Events
{
    public class EventDTO
    {
        public EventInfoDTO Event { get; set; }
        public bool IsUserEventAdmin { get; set; }
        public bool IsUserParticipant { get; set; }
        public bool IsUserApprovedParticipant { get; set; }
        public bool IsUserUndeterminedParticipant { get; set; }
        public bool IsUserRejectedParticipant { get; set; }
        public bool IsEventFinished { get; set; }
    }
}
