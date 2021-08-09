using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class ClubDocuments
    {
        public int ID { get; set; }
        public DateTime? SubmitDate { get; set; }
        [Required, MaxLength(64)]
        public string BlobName { get; set; }
        public string FileName { get; set; }
        [Required]
        public int ClubDocumentTypeId { get; set; }
        public ClubDocumentType ClubDocumentType { get; set; }
        [Required]
        public int ClubId { get; set; }
        public Club Club { get; set; }
    }
}