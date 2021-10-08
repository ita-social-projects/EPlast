
namespace EPlast.DataAccess.Entities
{
    public class ClubReportMember
    {
        public int ID { get; set; }
        public int ClubAnnualReportId { get; set; }
        public ClubAnnualReport ClubAnnualReport { get; set; }
        public int ClubMemberHistoryId { get; set; }
        public ClubMemberHistory ClubMemberHistory { get; set; }
    }
}
