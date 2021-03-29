using EPlast.DataAccess.Entities;
using System.Collections.Generic;

namespace EPlast.DataAccess.Repositories
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        IEnumerable<UserTableObject> GetUserTableObjects(int pageNum, int pageSize);
    }
}
