using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace EPlast.DataAccess.Repositories
{
    public class CityRepository : RepositoryBase<City>, ICityRepository
    {
        public CityRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<Tuple<IEnumerable<CityObject>, int>> GetCitiesObjects(int pageNum, int pageSize, string searchData, bool isArchive)
        {
            var items = await Task.Run(() => EPlastDBContext.Set<CityObject>().FromSqlRaw("dbo.sp_GetCities @PageIndex = {0}, @PageSize = {1}, @IsArhivated = {2}, @searchData = {3}", pageNum, pageSize, isArchive, searchData));
            var num = items.Select(u => u.Count).ToList();
            int rowCount = num.Count > 0 ? num[0] : 0;
            return new Tuple<IEnumerable<CityObject>, int>(items, rowCount);
        }
    }
}
