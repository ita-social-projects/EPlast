using System.Collections.Generic;
using EPlast.DataAccess.Entities.EducatorsStaff;

namespace EPlast.BLL.DTO.EducatorsStaff
{
    public class EducatorsStaffTypesDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<DataAccess.Entities.EducatorsStaff.EducatorsStaff> UsersKadras { get; set; }
    }
}
