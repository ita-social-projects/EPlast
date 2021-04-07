using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.BLL.DTO
{
    public class DecisionDTO
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DecisionStatusTypeDTO DecisionStatusType { get; set; }
        [Required]
        public GoverningBodyDTO GoverningBody { get; set; }
        [Required]
        public DecisionTargetDTO DecisionTarget { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime Date { get; set; }

        public string FileName { get; set; }
    }
}