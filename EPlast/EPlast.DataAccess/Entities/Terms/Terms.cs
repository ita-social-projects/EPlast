using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class Terms
    {
        [Key]
        public int TermsId { get; set; }

        [MaxLength(255)]
        public string TermsTitle { get; set; }

        [MaxLength(40000)]
        public string TermsText { get; set; }

        public DateTime? DatePublication { get; set; }
    }
}