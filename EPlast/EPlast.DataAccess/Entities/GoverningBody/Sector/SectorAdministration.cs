using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities.GoverningBody.Sector
{
    public class SectorAdministration
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int SectorId { get; set; }
        public Sector Sector { get; set; }

        [Required]
        public string UserId { get; set; }
        public User User { get; set; }

        public int AdminTypeId { get; set; }
        public AdminType AdminType { get; set; }

        public bool Status { get; set; }

        public string WorkEmail { get; set; }
    }
}