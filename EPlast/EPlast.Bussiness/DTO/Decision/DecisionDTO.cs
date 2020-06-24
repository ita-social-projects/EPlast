using System;

namespace EPlast.BusinessLogicLayer.DTO
{
    public class DecisionDTO
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public DecisionStatusTypeDTO DecisionStatusType { get; set; }

        public OrganizationDTO Organization { get; set; }

        public DecisionTargetDTO DecisionTarget { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public bool HaveFile { get; set; }
    }
}