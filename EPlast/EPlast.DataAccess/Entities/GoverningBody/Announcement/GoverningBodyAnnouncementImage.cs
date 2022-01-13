using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities.GoverningBody.Announcement
{
    public class GoverningBodyAnnouncementImage
    {
        public int Id { get; set; }
        [Required]
        public string ImagePath { get; set; }
        public int GoverningBodyAnnouncementId { get; set; }
        public GoverningBodyAnnouncement GoverningBodyAnnouncement { get; set; }
    }
}
