using System.Collections.Generic;
using EPlast.DataAccess.Entities.EducatorsStaff;
using Microsoft.EntityFrameworkCore;

namespace EPlast.DataAccess.Repositories
{
    public class EducatorsStaffRepository: RepositoryBase<EducatorsStaff>, IEducatorsStaffRepository
    {
        public EducatorsStaffRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }

        public IEnumerable<EducatorsStaffTableObject> GetEducatorsStaff(int kadraType, string searchData, int page,
            int pageSize)
        {
            return EPlastDBContext.Set<EducatorsStaffTableObject>().FromSqlRaw(
                "dbo.usp_GetKadras @searchData = {0}, @KadraType = {1}, @PageIndex = {2}, @PageSize = {3} ",
                searchData, kadraType, page, pageSize);
        }
    }
}
