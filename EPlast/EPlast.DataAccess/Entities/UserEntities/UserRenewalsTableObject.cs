using System;

namespace EPlast.DataAccess.Entities.UserEntities
{
    public class UserRenewalsTableObject
    {
        public int Id { get; set; }
        public int Subtotal { get; set; }
        public int Total { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string RegionName { get; set; }
        public DateTime RequestDate { get; set; }
        public string Email { get; set; }
        public bool Approved { get; set; }
    }
}
