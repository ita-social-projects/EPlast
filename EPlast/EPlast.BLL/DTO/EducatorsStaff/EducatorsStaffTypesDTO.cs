using System.Collections.Generic;

namespace EPlast.BLL.DTO.EducatorsStaff
{
    public class EducatorsStaffTypesDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<DataAccess.Entities.EducatorsStaff.EducatorsStaff> UsersKadras { get; set; }
    }
}
