using EPlast.DataAccess.Entities.EducatorsStaff;
using System.Collections.Generic;

namespace EPlast.BLL.DTO.EducatorsStaff
{
    public class EducatorsStaffTypesDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<DataAccess.Entities.EducatorsStaff.EducatorsStaff> UsersKadras { get; set; }
    }
}
