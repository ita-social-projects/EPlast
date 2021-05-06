using System;

namespace EPlast.DataAccess.Entities.UserEntities
{
    public class UserDistinctionsTableObject
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int Count { get; set; }
        public int Total { get; set; }
        public string DistinctionName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Reporter { get; set; }
        public string Reason { get; set; }
        public DateTime Date { get; set; }
    }
}
