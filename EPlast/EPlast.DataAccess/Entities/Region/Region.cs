using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EPlast.Resources;

namespace EPlast.DataAccess.Entities
{
    public class Region
    {
        public int ID { get; set; }

        [Required, MaxLength(50, ErrorMessage = "Region name cannot exceed 50 characters")]
        public string RegionName { get; set; }

        [MaxLength(1024, ErrorMessage = "Region description cannot exceed 1024 characters")]
        public string Description { get; set; }
        
        public ICollection<RegionAdministration> RegionAdministration { get; set; }
        public ICollection<City> Cities { get; set; }
        public ICollection<RegionDocuments> Documents { get; set; }
        public string City { get; set; }
        public UkraineOblasts Oblast { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Link { get; set; }
        public string Logo { get; set; }
        public bool IsActive { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string OfficeNumber { get; set; }
        public int PostIndex { get; set; }
        public RegionsStatusType Status { get; set; }
    }
}