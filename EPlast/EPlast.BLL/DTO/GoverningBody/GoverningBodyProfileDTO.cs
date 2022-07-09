using System.Collections.Generic;
using EPlast.BLL.DTO.GoverningBody.Announcement;
using EPlast.BLL.DTO.GoverningBody.Sector;

namespace EPlast.BLL.DTO.GoverningBody
{
    public class GoverningBodyProfileDto
    {
        public GoverningBodyDto GoverningBody { get; set; }
        public IEnumerable<SectorDto> Sectors { get; set; }
        public IEnumerable<GoverningBodyDocumentsDto> Documents { get; set; }
        public IEnumerable<GoverningBodyAnnouncementDto> Announcements { get; set; }
        public GoverningBodyAdministrationDto Head { get; set; }
        public IEnumerable<GoverningBodyAdministrationDto> GoverningBodyAdministration { get; set; }
    }
}
