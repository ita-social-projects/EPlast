using EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer
{
    public class BlankModel
    {
        public User User { get; set; }
        public UserProfile UserProfile { get; set; }
        public CityMembers CityMembers { get; set; }
    }
}