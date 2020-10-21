using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPlast.ViewModels.Club
{
    public class ClubDocumentTypeViewModel
    {
        public int ID { get; set; }
        [Required, MaxLength(50, ErrorMessage = "Document type name cannot exceed 50 characters")]
        public string Name { get; set; }
    }
}
