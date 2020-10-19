using System;

namespace EPlast.WebApi.Models.Club
{
    public class ClubMembersViewModel
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public ClubUserViewModel User { get; set; }
        public string ClubId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
