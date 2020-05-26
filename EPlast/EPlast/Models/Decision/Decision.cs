using System;

namespace EPlast.Models
{
    public class Decision
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public DecisionStatusType DecisionStatusType { get; set; }

        public Organization Organization { get; set; }

        public DecisionTarget DecisionTarget { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public bool HaveFile { get; set; }
    }
}