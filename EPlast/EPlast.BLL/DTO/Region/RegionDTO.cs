using EPlast.BLL.DTO.City;
using System.Collections.Generic;


namespace EPlast.BLL.DTO.Region
{
    public class RegionDTO
    {
        public int ID { get; set; }
        public string RegionName { get; set; }
        public string Description { get; set; }
        public IEnumerable<RegionAdministrationDTO> Administration { get; set; }
        public IEnumerable<RegionDocumentDTO> Documents { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string Logo { get; set; }
        public string Link { get; set; }
        public bool IsActive { get; set; }
        public string Street { get; set; }
        public bool CanCreate { get; set; }
        public string HouseNumber { get; set; }
        public string OfficeNumber { get; set; }
        public int PostIndex { get; set; }
        public RegionsStatusTypeDTO Status { get; set; }
    }
}
