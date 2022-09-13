using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.DTO.GoverningBody.Announcement
{
    public class GoverningBodyAnnouncementUserWithImagesDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public UserDto User { get; set; }
        public IEnumerable<GoverningBodyAnnouncementImageDto> Images { get; set; }
        public int GoverningBodyId { get; set; }
        public int? SectorId { get; set; }
        public bool ImagesPresent { get; set; }
        public bool IsPined { get; set; }
    }
}
