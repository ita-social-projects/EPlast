
namespace EPlast.DataAccess.Entities
{
    public class UnconfirmedCityMember
    {
        public int ID { get; set; }
        public User User { get; set; }
        public City City { get; set; }
    }
}
