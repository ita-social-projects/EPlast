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

        public async Task<Tuple<IEnumerable<UserTableObject>, int>> GetUserTableObjects(int pageNum, int pageSize, string tab, int sortKey, string regions, string cities, string clubs, string degrees, string searchData, string filterRoles="", string andClubs = null)
        {
          //  andClubs = "Степові відьми";
            var items = EPlastDBContext.Set<UserTableObject>().FromSqlRaw("dbo.usp_GetUserInfo @PageIndex = {0}, @PageSize = {1}, @tab = {2}, @sort={3}, @filterRegion = {4}, " +
                "@filterCity = {5}, @filterClub = {6}, @filterDegree = {7}, @filterRoles={8}, @searchData = {9}, @andClub = {10}", pageNum, pageSize, tab, sortKey, regions, cities, clubs, degrees, filterRoles, searchData, andClubs);

            var num = items.Select(u => u.Count).ToList();
            int rowCount = num.Count>0? num[0]:0;

            return new Tuple<IEnumerable<UserTableObject>, int>(items, rowCount);
        }
    }
}
