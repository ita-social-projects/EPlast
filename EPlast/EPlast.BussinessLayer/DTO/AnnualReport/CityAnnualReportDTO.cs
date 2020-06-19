namespace EPlast.BussinessLayer.DTO.AnnualReport
{
    public class CityAnnualReportDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public int RegionId { get; set; }
        public RegionAnnualReportDTO Region { get; set; }
    }
}