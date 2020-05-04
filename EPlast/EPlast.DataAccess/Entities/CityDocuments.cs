using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class CityDocuments
    {
        public int ID { get; set; }
        public DateTime? SubmitDate { get; set; }
        [Required, MaxLength(256, ErrorMessage = "Document url cannot exceed 256 characters")]
        public string DocumentURL { get; set; }
        public CityDocumentType CityDocumentType { get; set; }
        public City City { get; set; }
    }
}
