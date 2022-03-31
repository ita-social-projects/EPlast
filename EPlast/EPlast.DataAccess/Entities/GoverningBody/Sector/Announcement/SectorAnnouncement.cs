using EPlast.DataAccess.Entities.GoverningBody.Announcement;
using System;
using System.Collections.Generic;

namespace EPlast.DataAccess.Entities.GoverningBody.Sector
{
    public class SectorAnnouncement
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public int SectorId { get; set; }
        public Sector Sector { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public ICollection<SectorAnnouncementImage> Images { get; set; }
    }
}
