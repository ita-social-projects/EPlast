using System;
using System.ComponentModel.DataAnnotations;
using EPlast.DataAccess.Entities.GoverningBody;

namespace EPlast.DataAccess.Entities
{
    public class Decesion
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Назва рішення не заповнена.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Назва рішення не заповнена.")]
        public DecesionStatusType DecesionStatusType { get; set; }

        [Required(ErrorMessage = "Організація не вибрана.")]
        public Organization Organization { get; set; }

        [Required(ErrorMessage = "Тематика рішення не заповнена.")]
        public DecesionTarget DecesionTarget { get; set; }

        [Required(ErrorMessage = "Опис рішення не заповнено.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Дата рішення не заповнена.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        public string FileName { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
