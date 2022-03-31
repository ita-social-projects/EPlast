using EPlast.BLL.DTO.GoverningBody.Announcement;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.BLL.DTO.GoverningBody.Sector
{
    public class SectorProfileDTO
    {
        public SectorDTO Sector { get; set; }
        public IEnumerable<SectorDocumentsDTO> Documents { get; set; }
        public IEnumerable<SectorAnnouncementDTO> Announcements { get; set; }
        public SectorAdministrationDTO Head { get; set; }
        public IEnumerable<SectorAdministrationDTO> Administration { get; set; }
    }
}
