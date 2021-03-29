using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Tuple<IEnumerable<UserTableObject>, int>> GetUserTableObjects(int pageNum, int pageSize, string tab, string regions, string cities, string clubs, string degrees)
        {
            var items = EPlastDBContext.Set<UserTableObject>().FromSqlRaw("dbo.usp_GetUserInfo @PageIndex = {0}, @PageSize = {1}, @tab = {2}, @filterRegion = {3}, " +
                "@filterCity = {4}, @filterClub = {5}, @filterDegree = {6}", pageNum, pageSize, tab, regions, cities, clubs, degrees);

            var num = items.Select(u => u.cnt).ToList();
            int count = num[0];

            return new Tuple<IEnumerable<UserTableObject>, int>(items, count);
        }
    }
}
