using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class AdminType
    {
        public int ID { get; set; }

        [Required, MaxLength(30, ErrorMessage = "AdminTypeName name cannot exceed 30 characters")]
        public string AdminTypeName { get; set; }

        public ICollection<CityAdministration> CityAdministration { get; set; }
        public ICollection<ClubAdministration> ClubAdministration { get; set; }
        public ICollection<RegionAdministration> RegionAdministration { get; set; }
    }
}
