using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.DataAccess.Entities
{
    public class ClubMemberHistory
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int ClubId { get; set; }
        public Club Club { get; set; }
        public bool IsFollower { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<ClubReportMember> ClubReportMembers { get; set; }
    }
}
