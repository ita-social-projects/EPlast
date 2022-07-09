using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.BLL.DTO
{
    public class MethodicDocumentDto
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public MethodicDocumentTypeDto Type { get; set; }
        [Required]
        public GoverningBodyDto GoverningBody { get; set; }

        [Required, MaxLength(200, ErrorMessage = "Короткий зміст(опис) не має перевищувати 200 символів")]
        public string Description { get; set; }
        [Required]
        public DateTime Date { get; set; }

        public string FileName { get; set; }
    }
}
