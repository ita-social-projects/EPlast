using System;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities.EducatorsStaff
{
    public class EducatorsStaff
    {
        [Key]
        public int ID { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public int KadraVykhovnykivTypeId { get; set; }

        public EducatorsStaffTypes KadraVykhovnykivType { get; set; }

        public DateTime DateOfGranting { get; set; }

        public int NumberInRegister { get; set; }
    }
}
