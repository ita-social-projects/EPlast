using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities.GoverningBody.Sector
{
    public class Sector
    {
        public int Id { get; set; }
        public int GoverningBodyId { get; set; }
        public Organization GoverningBody { get; set; }

        [MaxLength(255, ErrorMessage = "Назва повинна містити не більше 255 символів")]
        public string Name { get; set; }

        public string Description { get; set; }

        [StringLength(12, ErrorMessage = "Контактний номер напряму повинен містити 12 цифр")]
        public string PhoneNumber { get; set; }

        public string Email { get; set; }
        public string Logo { get; set; }

        public ICollection<SectorAdministration> Administration { get; set; }
        public ICollection<GoverningBodyAnnouncement> Announcements { get; set; }
        public ICollection<SectorDocuments> Documents { get; set; }
        public bool IsActive { get; set; }
    }
}
