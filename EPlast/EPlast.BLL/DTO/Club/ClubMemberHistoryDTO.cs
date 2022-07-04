using System;

namespace EPlast.BLL.DTO.Club
{
    public class ClubMemberHistoryDto
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public ClubReportUserDto User { get; set; }
        public int ClubId { get; set; }
        public bool IsFollower { get; set; }
        public bool IsDeleted { get; set; }
    }
}
