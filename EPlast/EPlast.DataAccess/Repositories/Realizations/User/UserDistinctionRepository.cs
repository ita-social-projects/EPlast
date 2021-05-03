using EPlast.DataAccess.Entities.UserEntities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EPlast.DataAccess.Repositories
{
    public class UserDistinctionRepository : RepositoryBase<UserDistinction>, IUserDistinctionRepository
    {
        public UserDistinctionRepository(EPlastDBContext dBContext)
            : base(dBContext)
        {
        }

        public IEnumerable<UserDistinctionsTableObject> GetUsersDistinctions(string searchData, int page, int pageSize)
        {
            return EPlastDBContext.Set<UserDistinctionsTableObject>().FromSqlRaw(
                "dbo.getDistinctionsInfo  @searchData = {0}, @PageIndex ={1}, @PageSize={2}", searchData, page,
                pageSize);
        }
    }
}
