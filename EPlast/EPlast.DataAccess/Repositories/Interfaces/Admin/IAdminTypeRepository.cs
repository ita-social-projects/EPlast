using EPlast.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.DataAccess.Repositories.Contracts
{
    public interface IAdminTypeRepository: IRepositoryBase<AdminType>
    {
        IEnumerable<UserTableObject> GetUserTableObjects(int pageNum, int pageSize, string tab);
        Task<int> GetUsersCountAsync();
    }
}
