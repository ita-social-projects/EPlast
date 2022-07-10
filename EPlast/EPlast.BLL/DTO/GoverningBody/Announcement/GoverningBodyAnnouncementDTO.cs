using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.DTO.GoverningBody.Announcement
{
    public class GoverningBodyAnnouncementDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public bool IsPined { get; set; }
        public int GoverningBodyId { get; set; }
        public int? SectorId { get; set; }
    }
}
