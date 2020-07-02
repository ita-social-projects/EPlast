using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.ViewModels.City
{
    public class CityDocumentViewModel
    {
        public int ID { get; set; }
        public DateTime? SubmitDate { get; set; }
        [Required, MaxLength(256, ErrorMessage = "Document url cannot exceed 256 characters")]
        public string DocumentURL { get; set; }
        public CityDocumentTypeViewModel CityDocumentType { get; set; }
    }
}
