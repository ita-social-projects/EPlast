using System;

namespace EPlast.DataAccess.Entities
{
    public class UserTableObject
    {
        public string ID { get; set; }
        public long Index { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public string Gender { get; set; }
        public string RegionName { get; set; }
        public string CityName { get; set; }
        public string ClubName { get; set; }
        public string PlastDegree { get; set; }
        public string Roles { get; set; }
        public string Email { get; set; }
        public string UPUDegree { get; set; }
        public int Count { get; set; }
        public int UserSystemId { get; set; }
        public int? RegionId { get; set; }
        public int? CityId { get; set; }
        public int? ClubId { get; set; }

    }
}
