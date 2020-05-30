namespace EPlast.BussinessLayer.DTO
{
    public class MembersStatisticDTO
    {
        public int Id { get; set; }

        public int NumberOfPtashata { get; set; }

        public int NumberOfNovatstva { get; set; }

        public int NumberOfUnatstvaNoname { get; set; }

        public int NumberOfUnatstvaSupporters { get; set; }

        public int NumberOfUnatstvaMembers { get; set; }

        public int NumberOfUnatstvaProspectors { get; set; }

        public int NumberOfUnatstvaSkobVirlyts { get; set; }

        public int NumberOfSeniorPlastynSupporters { get; set; }

        public int NumberOfSeniorPlastynMembers { get; set; }

        public int NumberOfSeigneurSupporters { get; set; }

        public int NumberOfSeigneurMembers { get; set; }

        public int AnnualReportId { get; set; }
        public AnnualReportDTO AnnualReport { get; set; }
    }
}