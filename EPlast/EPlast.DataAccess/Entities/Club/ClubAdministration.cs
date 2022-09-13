using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class ClubAdministration
    {
        public int ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int ClubId { get; set; }
        public Club Club { get; set; }

        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
        public bool Status { get; set; }
        public int AdminTypeId { get; set; }
        public AdminType AdminType { get; set; }

        public ICollection<ClubReportAdmins> ClubReportAdmins { get; set; }
    }
}