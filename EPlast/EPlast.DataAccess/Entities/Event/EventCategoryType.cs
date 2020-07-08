namespace EPlast.DataAccess.Entities.Event
{
    public class EventCategoryType
    {
        public int EventTypeId { get; set; }
        public EventType EventType { get; set; }

        public int EventCategoryId { get; set; }
        public EventCategory EventCategory { get; set; }
    }
}
