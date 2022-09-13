using System.Collections.Generic;
using EPlast.BLL.DTO.GoverningBody.Announcement;

namespace EPlast.BLL.DTO.GoverningBody.Sector
{
    public class SectorDto
    {
        public int Id { get; set; }
        public int GoverningBodyId { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string PhoneNumber { get; set; }
        public int AdministrationCount { get; set; }
        public ICollection<SectorDocumentsDto> Documents { get; set; }
        public ICollection<GoverningBodyAnnouncementDto> Announcements { get; set; }
        public ICollection<SectorAdministrationDto> Administration { get; set; }
        public bool IsActive { get; set; }
    }
}
