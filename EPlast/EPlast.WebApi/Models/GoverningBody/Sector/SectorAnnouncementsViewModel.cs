using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.WebApi.Models.GoverningBody.Sector
{
    public class SectorAnnouncementsViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public int SectorId { get; set; }
    }
}
