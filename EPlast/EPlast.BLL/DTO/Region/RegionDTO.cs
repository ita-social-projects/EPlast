using EPlast.BLL.DTO.City;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.BLL.DTO.Region
{
    public class RegionDTO
    {
        public int ID { get; set; }
        [Required, MaxLength(50)]
        public string RegionName { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        public IEnumerable<RegionAdministrationDTO> Administration { get; set; }
        public IEnumerable<RegionDocumentDTO> Documents { get; set; }
        [Required, Phone, MaxLength(18)]
        public string PhoneNumber { get; set; }
        [Required, MaxLength(50)]
        public string City { get; set; }
        [Required, EmailAddress, MaxLength(50)]
        public string Email { get; set; }
        public string Logo { get; set; }
        public string Link { get; set; }
        public bool IsActive { get; set; }
        [Required, MaxLength(50)]
        public string Street { get; set; }
        public bool CanCreate { get; set; }
        [Required, MaxLength(5)]
        public string HouseNumber { get; set; }
        public string OfficeNumber { get; set; }
        [Required, Range(10000, 99999)]
        public int PostIndex { get; set; }
        public RegionsStatusTypeDTO Status { get; set; }
    }
}
