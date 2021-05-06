using EPlast.BLL.DTO.UserProfiles;
using System;

namespace EPlast.BLL.DTO.EducatorsStaff
{
    public class EducatorsStaffDTO
    {
        public int ID { get; set; }

        public string UserId { get; set; }

        public UserDTO User { get; set; }

        public int KadraVykhovnykivTypeId { get; set; }

        public EducatorsStaffTypesDTO KadraVykhovnykivType { get; set; }

        public DateTime DateOfGranting { get; set; }

        public int NumberInRegister { get; set; }
    }
}
