using EPlast.BLL.DTO.UserProfiles;
using System;

namespace EPlast.BLL.DTO.EducatorsStaff
{
    public class KadraVykhovnykivDTO
    {
        public int ID { get; set; }

        public string UserId { get; set; }
        public UserDTO User { get; set; }
        public int KadraVykhovnykivTypeId { get; set; }
        public KadraVykhovnykivTypesDTO KadraVykhovnykivType { get; set; }
        public DateTime DateOfGranting { get; set; }

        public int NumberInRegister { get; set; }

        public string BasisOfGranting { get; set; }

        public string Link { get; set; }
    }
}
