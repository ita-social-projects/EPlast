using EPlast.Resources;
using System;

namespace EPlast.DataAccess.Entities.UserEntities
{
    public class UserPrecautionsTableObject
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string PrecautionName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Reporter { get; set; }
        public string Reason { get; set; }
        public UserPrecautionStatus Status { get; set; }
        public DateTime Date { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
