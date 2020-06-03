namespace EPlast.DataAccess.Entities.Event
{
    public class EventGallary
    {
        public int EventID { get; set; }
        public Entities.Event.Event Event { get; set; }
        public int GallaryID { get; set; }
        public Gallary Gallary { get; set; }
    }
}
