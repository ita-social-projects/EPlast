using System.Collections.Generic;
using EPlast.WebApi.Models.GoverningBody.Sector;

namespace EPlast.WebApi.Models.GoverningBody
{
    public class GoverningBodyViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string GoverningBodyName { get; set; }
        public string Logo { get; set; }
        public string PhoneNumber { get; set; }
        public int AdministrationCount { get; set; }
        public GoverningBodyAdministrationViewModel Head { get; set; }
        public IEnumerable<SectorViewModel> Sectors { get; set; }
        public IEnumerable<GoverningBodyAdministrationViewModel> Administration { get; set; }
        public IEnumerable<GoverningBodyDocumentsViewModel> Documents { get; set; }
        public IEnumerable<GoverningBodyAnnouncementsViewModel> Announcements { get; set; }
    }
}
