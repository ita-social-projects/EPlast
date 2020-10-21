using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class ClubMembers
    {
        public int ID { get; set; }
        public bool IsApproved { get; set; }

        public int ClubId { get; set; }
        public Club Club { get; set; }

        [Required]
        public string UserId { get; set; }
        public User User { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
