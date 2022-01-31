using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.DataAccess.Entities.GoverningBody.Announcement
{
    public class GoverningBodyAnnouncement
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public ICollection<GoverningBodyAnnouncementImage> Images { get; set; }
    }
}
