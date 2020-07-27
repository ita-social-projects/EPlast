using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
    }
}