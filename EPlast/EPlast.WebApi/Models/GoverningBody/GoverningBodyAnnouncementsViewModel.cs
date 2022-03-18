using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.WebApi.Models.GoverningBody
{
    public class GoverningBodyAnnouncementsViewModel
    {
        public string Text { get; set; }
        public string Title { get; set; }
        public string UserId { get; set; }
        public int GoverningBodyId { get; set; }
    }
}
