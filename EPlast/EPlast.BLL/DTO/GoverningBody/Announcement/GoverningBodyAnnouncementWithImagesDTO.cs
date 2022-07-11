using System;
using System.Collections.Generic;

namespace EPlast.BLL.DTO.GoverningBody.Announcement
{
    public class GoverningBodyAnnouncementWithImagesDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public string UserId { get; set; }
        public ICollection<string> ImagesBase64{ get; set; }
        public int GoverningBodyId { get; set; }
        public int? SectorId { get; set; }
        public bool IsPined { get; set; }
    }
}
