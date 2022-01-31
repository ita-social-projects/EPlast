using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.WebApi.Models.Terms
{
    public class TermsViewModel
    {
        public int TermsId { get; set; }
        public string TermsTitle { get; set; }
        public string TermsText { get; set; }
        public DateTime DatePublication { get; set; }
    }
}