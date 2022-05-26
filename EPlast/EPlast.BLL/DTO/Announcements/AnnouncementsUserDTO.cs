using EPlast.BLL.DTO.GoverningBody.Announcement;
using System;
using System.Collections.Generic;

namespace EPlast.BLL.DTO.Announcements
{
    public class AnnouncementsUserDTO
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public UserDTO User { get; set; }
        public IEnumerable<GoverningBodyAnnouncementImageDTO> Images { get; set; }
        public bool ImagesPresent { get; set; }
    }
}
