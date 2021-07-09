using System.Collections.Generic;

namespace EPlast.WebApi.Models.GoverningBody.Sector
{
    public class SectorViewModel
    {
        public int Id { get; set; }
        public int GoverningBodyId { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string PhoneNumber { get; set; }
        public int AdministrationCount { get; set; }
        public SectorAdministrationViewModel Head { get; set; }
        public IEnumerable<SectorAdministrationViewModel> Administration { get; set; }
        public IEnumerable<SectorDocumentsViewModel> Documents { get; set; }
    }
}