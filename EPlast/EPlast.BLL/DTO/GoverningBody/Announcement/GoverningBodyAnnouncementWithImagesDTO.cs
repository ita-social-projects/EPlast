using System;
using System.Collections.Generic;

namespace EPlast.BLL.DTO.GoverningBody.Announcement
{
    public class GoverningBodyAnnouncementWithImagesDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public ICollection<string> ImagesBase64{ get; set; }
    }
}
