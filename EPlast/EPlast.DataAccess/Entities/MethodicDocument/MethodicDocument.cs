using System;
using System.ComponentModel.DataAnnotations;
using EPlast.DataAccess.Entities.GoverningBody;

namespace EPlast.DataAccess.Entities
{
    public class MethodicDocument
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Назва документу не заповнена.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Тип документу не заповнено.")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Організація не вибрана.")]
        public Organization Organization { get; set; }

        [Required(ErrorMessage = "Опис документу не заповнено.")]
        [MaxLength(200, ErrorMessage = "Short description cannot exceed 200 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Дата документу не заповнена.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        public string FileName { get; set; }
    }
}
