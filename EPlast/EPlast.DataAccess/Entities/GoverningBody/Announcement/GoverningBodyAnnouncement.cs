using System;
using System.Collections.Generic;
using EPlast.DataAccess.Entities.GoverningBody.Announcement;

namespace EPlast.DataAccess.Entities.GoverningBody
{
    public class GoverningBodyAnnouncement
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public int? GoverningBodyId { get; set; }
        public Organization GoverningBody { get; set; }
        public int? SectorId { get; set; }
        public Sector.Sector Sector { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public bool IsPined { get; set; }
        public ICollection<GoverningBodyAnnouncementImage> Images { get; set; }
    }
}
