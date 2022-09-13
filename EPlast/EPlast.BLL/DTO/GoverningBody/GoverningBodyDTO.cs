using System.Collections.Generic;
using EPlast.BLL.DTO.GoverningBody;
using EPlast.BLL.DTO.GoverningBody.Announcement;
using EPlast.BLL.DTO.GoverningBody.Sector;

namespace EPlast.BLL.DTO
{
    public class GoverningBodyDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string GoverningBodyName { get; set; }
        public string Logo { get; set; }
        public string PhoneNumber { get; set; }
        public int AdministrationCount { get; set; }
        public IEnumerable<SectorDto> GoverningBodySectors { get; set; }
        public IEnumerable<GoverningBodyDocumentsDto> GoverningBodyDocuments { get; set; }
        public IEnumerable<GoverningBodyAnnouncementDto> GoverningBodyAnnouncements { get; set; }
        public IEnumerable<GoverningBodyAdministrationDto> GoverningBodyAdministration { get; set; }
        public bool IsActive { get; set; }
    }
}
