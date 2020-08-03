using System;

namespace EPlast.WebApi.Models.City
{
    public class CityMembersViewModel
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public CityUserViewModel User { get; set; }
        public string CityId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}