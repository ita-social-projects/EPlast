using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class DocumentTemplate
    {
        public int ID { get; set; }

        [DisplayName("Document Name")]
        [Required, MaxLength(50, ErrorMessage = "Document Name cannot exceed 50 characters")]
        public string DocumentName { get; set; }

        [DisplayName("Document File Name")]
        [Required, MaxLength(50, ErrorMessage = "Document File Name cannot exceed 50 characters")]
        public string DocumentFIleName { get; set; }
    }
}