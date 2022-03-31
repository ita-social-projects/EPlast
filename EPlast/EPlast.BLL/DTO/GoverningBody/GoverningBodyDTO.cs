using System.Collections.Generic;
using EPlast.BLL.DTO.GoverningBody;
using EPlast.BLL.DTO.GoverningBody.Announcement;
using EPlast.BLL.DTO.GoverningBody.Sector;

namespace EPlast.BLL.DTO
{
    public class GoverningBodyDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string GoverningBodyName { get; set; }
        public string Logo { get; set; }
        public string PhoneNumber { get; set; }
        public int AdministrationCount { get; set; }
        public IEnumerable<SectorDTO> GoverningBodySectors { get; set; }
        public IEnumerable<GoverningBodyDocumentsDTO> GoverningBodyDocuments { get; set; }
        public IEnumerable<GoverningBodyAnnouncementDTO> GoverningBodyAnnouncements { get; set; }
        public IEnumerable<GoverningBodyAdministrationDTO> GoverningBodyAdministration { get; set; }
        public bool IsActive { get; set; }
    }
}
