using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class ClubDocumentType
    {
        public int ID { get; set; }
        [Required, MaxLength(50, ErrorMessage = "Document type name cannot exceed 50 characters")]
        public string Name { get; set; }
        public ICollection<ClubDocuments> ClubDocuments { get; set; }
    }
}