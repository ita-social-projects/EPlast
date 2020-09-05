using EPlast.DataAccess.Entities.EducatorsStaff;
using System.Collections.Generic;

namespace EPlast.BLL.DTO.EducatorsStaff
{
    public class KadraVykhovnykivTypesDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<KadraVykhovnykiv> UsersKadras { get; set; }
    }
}
