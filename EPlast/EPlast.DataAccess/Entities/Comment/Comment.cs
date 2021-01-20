using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EPlast.DataAccess.Entities
{
    public class Comment
    {
        public int ID { get; set; }
        [Required]
        [MaxLength(200, ErrorMessage = "Поле не може перевищувати 200 символів")]
        public string Text { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Date { get; set; }
        public string UserID { get; set; }
        public User User { get; set; }

        public string AuthorID { get; set; }
        public User Author { get; set; }
    }
}
