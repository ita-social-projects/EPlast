using EPlast.DataAccess.Entities.UserEntities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EPlast.DataAccess.Repositories
{
    public class UserPrecautionRepository : RepositoryBase<UserPrecaution>, IUserPrecautionRepository
    {
        public UserPrecautionRepository(EPlastDBContext dBContext)
            : base(dBContext)
        {
        }

        public IEnumerable<UserPrecautionsTableObject> GetUsersPrecautions(string searchData, int page, int pageSize)
        {
            return EPlastDBContext.Set<UserPrecautionsTableObject>().FromSqlRaw(
                "dbo.getPrecautionsInfo  @searchData = {0}, @PageIndex ={1}, @PageSize={2}", searchData, page,
                pageSize);
        }
    }
}
