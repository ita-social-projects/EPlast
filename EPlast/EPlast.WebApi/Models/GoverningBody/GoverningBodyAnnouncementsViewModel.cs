using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.WebApi.Models.GoverningBody
{
    public class GoverningBodyAnnouncementsViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public bool IsPined { get; set; }
    }
}
