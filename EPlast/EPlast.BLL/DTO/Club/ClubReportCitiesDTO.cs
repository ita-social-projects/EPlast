namespace EPlast.BLL.DTO.Club
{
    public class ClubReportCitiesDto
    {
        public int ID { get; set; }
        public int ClubAnnualReportId { get; set; }
        public string UserId { get; set; }
        public int CityId { get; set; }
        public ClubReportCityDto City { get; set; }
    }
}
