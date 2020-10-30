using EPlast.DataAccess.Entities;

namespace EPlast.BLL
{
    public class BlankModel
    {
        public User User { get; set; }
        public UserProfile UserProfile { get; set; }
        public CityMembers CityMembers { get; set; }
        public ClubMembers ClubMembers { get; set; }
    }
}