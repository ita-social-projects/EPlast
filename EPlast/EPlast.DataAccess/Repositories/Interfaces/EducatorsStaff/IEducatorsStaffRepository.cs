using System.Collections.Generic;
using EPlast.DataAccess.Entities.EducatorsStaff;

namespace EPlast.DataAccess.Repositories
{
    public interface IEducatorsStaffRepository:IRepositoryBase<EducatorsStaff>
    {
        IEnumerable<EducatorsStaffTableObject> GetEducatorsStaff (int kadraType, string searchData, int page, int pageSize);
    }
}
