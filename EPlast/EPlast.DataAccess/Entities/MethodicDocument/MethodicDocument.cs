using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class MethodicDocument
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Назва документу не заповнена.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Тип документу не заповнено.")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Організація не вибрана.")]
        public GoverningBody GoverningBody { get; set; }

        [Required(ErrorMessage = "Опис документу не заповнено.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Дата документу не заповнена.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        public string FileName { get; set; }
    }
}
