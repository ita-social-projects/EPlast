using System.Collections.Generic;

namespace EPlast.BLL.DTO.GoverningBody.Sector
{
    public class SectorDTO
    {
        public int Id { get; set; }
        public int GoverningBodyId { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string PhoneNumber { get; set; }
        public int AdministrationCount { get; set; }
        public IEnumerable<SectorDocumentsDTO> Documents { get; set; }
        public IEnumerable<SectorAdministrationDTO> Administration { get; set; }
        public bool IsActive { get; set; }
    }
}
