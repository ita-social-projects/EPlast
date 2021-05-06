using EPlast.DataAccess.Entities.UserEntities;
using System.Collections.Generic;

namespace EPlast.DataAccess.Repositories
{
    public interface IUserDistinctionRepository : IRepositoryBase<UserDistinction>
    {
        IEnumerable<UserDistinctionsTableObject> GetUsersDistinctions(string searchData, int page, int pageSize);
    }
}
