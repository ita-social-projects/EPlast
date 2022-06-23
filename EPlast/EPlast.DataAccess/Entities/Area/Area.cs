using System.Collections.Generic;

namespace EPlast.DataAccess.Entities
{
    public class Area
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<UserProfile> UserProfiles { get; set; }
        public ICollection<City> Cities { get; set; }
        public ICollection<Region> Regions { get; set; }
    }
}
