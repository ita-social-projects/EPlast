using EPlast.BLL.DTO.ActiveMembership;

namespace EPlast.BLL.DTO.Club
{
    public class ClubReportPlastDegreesDto
    {
        public int ID { get; set; }
        public int ClubAnnualReportId { get; set; }
        public string UserId { get; set; }
        public int PlastDegreeId { get; set; }
        public PlastDegreeDto PlastDegree { get; set; }
    }
}
