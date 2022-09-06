namespace EPlast.BLL.DTO.Events
{
    public class EventFeedbackDto
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public string AuthorAvatarUrl { get; set; }
        public string AuthorUserId { get; set; }
        public double Rating { get; set; }
        public string Text { get; set; }
    }
}
