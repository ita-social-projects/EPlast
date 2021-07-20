using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities.GoverningBody.Sector
{
    public class SectorDocumentType
    {
        public int Id { get; set; }

        [Required, MaxLength(50, ErrorMessage = "Document type name cannot exceed 50 characters")]
        public string Name { get; set; }

        public ICollection<SectorDocuments> Documents { get; set; }
    }
}