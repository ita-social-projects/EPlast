using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EPlast.Resources;

namespace EPlast.BLL.DTO.Region
{
    public class RegionDto
    {
        public int ID { get; set; }
        [Required, MaxLength(50)]
        public string RegionName { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        public IEnumerable<RegionAdministrationDto> Administration { get; set; }
        public IEnumerable<RegionDocumentDto> Documents { get; set; }
        [Required, Phone, MaxLength(18)]
        public string PhoneNumber { get; set; }
        [Required, MaxLength(50)]
        public string City { get; set; }
        [Required, Range(1, int.MaxValue)]
        public UkraineOblasts Oblast { get; set; }
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
        public RegionsStatusTypeDto Status { get; set; }
    }
}
