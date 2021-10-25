using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.BLL.DTO
{
    public class MethodicDocumentDTO
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public MethodicDocumentTypeDTO Type { get; set; }
        [Required]
        public GoverningBodyDTO GoverningBody { get; set; }

        [Required, MaxLength(200, ErrorMessage = "Короткий зміст(опис) не має перевищувати 200 символів")]
        public string Description { get; set; }
        [Required]
        public DateTime Date { get; set; }

        public string FileName { get; set; }
    }
}
