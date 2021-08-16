using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class Section
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public ICollection<Subsection> Subsections { get; set; }
    }
}
