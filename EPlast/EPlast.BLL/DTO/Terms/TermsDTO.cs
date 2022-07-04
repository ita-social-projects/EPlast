using System;

namespace EPlast.BLL.DTO.Terms
{
    public class TermsDto
    {
        public int TermsId { get; set; }
        public string TermsTitle { get; set; }
        [AllowHtml]
        public string TermsText { get; set; }
        public DateTime DatePublication { get; set; }
    }
}
