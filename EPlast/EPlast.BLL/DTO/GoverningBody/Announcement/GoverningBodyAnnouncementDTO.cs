using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.DTO.GoverningBody.Announcement
{
    public class GoverningBodyAnnouncementDTO
    {
        public string Text { get; set; }
        public string Title { get; set; }
        public string UserId { get; set; }
        public int GoverningBodyId { get; set; }
    }
}
