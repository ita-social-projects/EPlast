using System;
using EPlast.BLL.DTO.UserProfiles;

namespace EPlast.BLL.DTO.EducatorsStaff
{
    public class EducatorsStaffDto
    {
        public int ID { get; set; }

        public string UserId { get; set; }

        public UserDto User { get; set; }

        public int KadraVykhovnykivTypeId { get; set; }

        public EducatorsStaffTypesDto KadraVykhovnykivType { get; set; }

        public DateTime DateOfGranting { get; set; }

        public int NumberInRegister { get; set; }
    }
}
