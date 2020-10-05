using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities.Blank
{
    public class BlankBiographyDocuments
    {
        public int ID { get; set; }
        public string BlobName { get; set; }
        [Required, MaxLength(120)]
        public string FileName { get; set; }
        [Required]
        public int BlankDocumentTypeId { get; set; }
        public BlankBiographyDocumentsType BlankBiographyDocumentsType { get; set; }
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
