using System;

namespace EPlast.WebApi.Models.Decision
{
    public class DecisionViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string DecisionStatusType { get; set; }

        public string GoverningBody { get; set; }

        public string DecisionTarget { get; set; }

        public string Description { get; set; }

        public string Date { get; set; }

        public string FileName { get; set; }
    }
}
