using EPlast.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.DataAccess.Repositories.Contracts
{
    public interface IAdminTypeRepository : IRepositoryBase<AdminType>
    {
        Task<Tuple<IEnumerable<UserTableObject>, int>> GetUserTableObjects(int pageNum, int pageSize, string tab,
            string regions, string cities, string clubs, string degrees, int sortKey, string searchData,
            string filterRoles = "", string andClubs = null);

        Task<int> GetUsersCountAsync();
    }
}
