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

        public async Task<Tuple<IEnumerable<UserTableObject>, int>> GetUserTableObjects(int pageNum, int pageSize, string tab, string regions, string cities, string clubs, string degrees, int sortKey, string searchData, string filterRoles="")
        {
            var items = EPlastDBContext.Set<UserTableObject>().FromSqlRaw("dbo.usp_GetUserInfo @PageIndex = {0}, @PageSize = {1}, @tab = {2}, @filterRegion = {3}, " +
                "@filterCity = {4}, @filterClub = {5}, @filterDegree = {6}, @sort={7}, @filterRoles={8}, @searchData = {9}", pageNum, pageSize, tab, regions, cities, clubs, degrees, sortKey, filterRoles, searchData);

            var num = items.Select(u => u.Count).ToList();
            int rowCount = num.Count>0? num[0]:0;

            return new Tuple<IEnumerable<UserTableObject>, int>(items, rowCount);
        }
    }
}
