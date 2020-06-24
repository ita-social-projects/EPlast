namespace EPlast.ViewModels.AnnualReport
{
    public class CityViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public int RegionId { get; set; }
        public RegionViewModel Region { get; set; }
    }
}