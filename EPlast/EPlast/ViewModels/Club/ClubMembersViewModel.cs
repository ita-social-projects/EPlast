using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.ViewModels
{
    public class ClubMembersViewModel
    {
        public int ID { get; set; }

        [Required]
        public string UserId { get; set; }
        public ClubUserViewModel User { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
