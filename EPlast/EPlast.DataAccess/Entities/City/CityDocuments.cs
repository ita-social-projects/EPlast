using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class CityDocuments
    {
        public int ID { get; set; }
        public DateTime? SubmitDate { get; set; }
        [Required, MaxLength(64)]
        public string BlobName { get; set; }
        public string FileName { get; set; }
        [Required]
        public int CityDocumentTypeId { get; set; }
        public CityDocumentType CityDocumentType { get; set; }
        [Required]
        public int CityId { get; set; }
        public City City { get; set; }
    }
}
