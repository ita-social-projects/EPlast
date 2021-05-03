using EPlast.DataAccess.Entities.UserEntities;
using System.Collections.Generic;

namespace EPlast.DataAccess.Repositories
{
    public interface IUserPrecautionRepository : IRepositoryBase<UserPrecaution>
    {
        IEnumerable<UserPrecautionsTableObject> GetUsersPrecautions(string searchData, int page, int pageSize);
    }
}
