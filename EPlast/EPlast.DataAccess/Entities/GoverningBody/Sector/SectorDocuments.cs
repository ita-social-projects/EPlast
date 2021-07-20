using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities.GoverningBody.Sector
{
    public class SectorDocuments
    {
        public int Id { get; set; }
        public DateTime? SubmitDate { get; set; }

        [Required, MaxLength(64)]
        public string BlobName { get; set; }

        [Required, MaxLength(120)]
        public string FileName { get; set; }

        [Required]
        public int SectorDocumentTypeId { get; set; }

        public SectorDocumentType SectorDocumentType { get; set; }

        [Required]
        public int SectorId { get; set; }

        public Sector Sector { get; set; }
    }
}