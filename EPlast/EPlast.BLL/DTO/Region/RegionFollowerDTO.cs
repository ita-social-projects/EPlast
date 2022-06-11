using EPlast.DataAccess.Entities;

namespace EPlast.BLL.DTO.Region
{
    public class RegionFollowerDTO
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public string Appeal { get; set; }
        public string CityName { get; set; }
        public string CityDescription { get; set; }
        public string Logo { get; set; }
        public int RegionId { get; set; }
        public string Adress { get; set; }
        public CityLevel Level { get; set; }
        public string СityURL { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
