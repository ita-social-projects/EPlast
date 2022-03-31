using System;
using System.Collections.Generic;

namespace EPlast.DataAccess.Entities.GoverningBody.Announcement
{
    public class GoverningBodyAnnouncement
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public int? GoverningBodyId { get; set; }
        public Organization GoverningBody { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public ICollection<GoverningBodyAnnouncementImage> Images { get; set; }
    }
}
