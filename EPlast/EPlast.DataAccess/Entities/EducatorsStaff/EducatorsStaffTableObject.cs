using System;

namespace EPlast.DataAccess.Entities.EducatorsStaff
{
    public class EducatorsStaffTableObject
    {
        public int Id { get; set; }
        public int Subtotal { get; set; }
        public int Total { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime DateOfGranting { get; set; }
        public int NumberInRegister { get; set; }
        public int KadraVykhovnykivTypeId { get; set; }
        public string KadraName { get; set; }
    }
}
