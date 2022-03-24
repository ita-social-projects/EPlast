using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.DTO.GoverningBody.Announcement
{
    public class SectorAnnouncementWithImagesDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public string UserId { get; set; }
        public ICollection<string> ImagesBase64 { get; set; }
        public int SectorId { get; set; }
    }
}
