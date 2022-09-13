using EPlast.Resources;
using System;

namespace EPlast.DataAccess.Entities
{
    public class UserTableObject
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public string Gender { get; set; }
        public string RegionName { get; set; }
        public string CityName { get; set; }
        public string ClubName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Referal { get; set; }
        public string Comment { get; set; }
        public UkraineOblasts Oblast { get; set; }
        public string PlastDegree { get; set; }
        public string Roles { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string UPUDegree { get; set; }
        public int UserSystemId { get; set; }
        public int? RegionId { get; set; }
        public int? CityId { get; set; }
        public int? ClubId { get; set; }
        public int? DegreeId { get; set; }
        public bool IsCityFollower { get; set; }
        public bool IsClubFollower { get; set; }

    }
}
