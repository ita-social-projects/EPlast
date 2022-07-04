using System.Collections.Generic;
using EPlast.BLL.DTO.GoverningBody.Announcement;

namespace EPlast.BLL.DTO.GoverningBody.Sector
{
    public class SectorProfileDto
    {
        public SectorDto Sector { get; set; }
        public IEnumerable<SectorDocumentsDto> Documents { get; set; }
        public IEnumerable<GoverningBodyAnnouncementDto> Announcements { get; set; }
        public SectorAdministrationDto Head { get; set; }
        public IEnumerable<SectorAdministrationDto> Administration { get; set; }
    }
}
