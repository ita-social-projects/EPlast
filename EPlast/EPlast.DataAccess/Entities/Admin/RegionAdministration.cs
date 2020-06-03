using System;
using System.ComponentModel.DataAnnotations;


namespace EPlast.DataAccess.Entities
{
    public class RegionAdministration
    {
        public int ID { get; set; }
        [Required]
        public AdminType AdminType { get; set; }
        public User User { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Region Region { get; set; }
    }
}
