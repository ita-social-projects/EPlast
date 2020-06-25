namespace EPlast.BLL.DTO.Statistics
{
    public class City
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public int RegionId { get; set; }
        public Region Region { get; set; }
    }
}