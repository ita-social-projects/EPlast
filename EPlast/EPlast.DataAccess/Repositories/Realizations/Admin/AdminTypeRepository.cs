using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.DataAccess.Repositories
{
    public class AdminTypeRepository : RepositoryBase<AdminType>, IAdminTypeRepository
    {
        public AdminTypeRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }

        public Task<int> GetUsersCountAsync()
        {
           return EPlastDBContext.Users.CountAsync();
        }

        public IEnumerable<UserTableObject> GetUserTableObjects(int pageNum, int pageSize)
        {
            var items = EPlastDBContext.Set<UserTableObject>().FromSqlRaw("dbo.usp_GetUserInfo @PageIndex = {0}, @PageSize = {1}", pageNum, pageSize);
            return items;
        }
    }
}
