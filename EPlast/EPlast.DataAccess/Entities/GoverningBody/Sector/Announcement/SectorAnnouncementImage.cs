using EPlast.DataAccess.Entities.GoverningBody.Sector;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EPlast.DataAccess.Entities.GoverningBody.Announcement
{
    public class SectorAnnouncementImage
    {
        public int Id { get; set; }
        [Required]
        public string ImagePath { get; set; }
        public int SectorAnnouncementId { get; set; }
        public SectorAnnouncement SectorAnnouncement { get; set; }
    }
}
