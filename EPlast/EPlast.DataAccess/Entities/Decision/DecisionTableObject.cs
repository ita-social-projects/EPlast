using System;

namespace EPlast.DataAccess.Entities.Decision
{
    public class DecisionTableObject
    {
        public int Total { get; set; }

        public int Count { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public int DecisionStatusType { get; set; }

        public string GoverningBody { get; set; }

        public string DecisionTarget { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public string UserId { get; set; }

        public string FileName { get; set; }
    }
}
