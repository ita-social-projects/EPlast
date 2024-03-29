﻿#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;
using EPlast.Resources;
using Microsoft.EntityFrameworkCore;

namespace EPlast.DataAccess.Repositories
{
    public class CityRepository : RepositoryBase<City>, ICityRepository
    {
        public CityRepository(EPlastDBContext dbContext) : base(dbContext) { }

        public async Task<Tuple<IEnumerable<CityObject>, int>> GetCitiesObjects(int pageNum, int pageSize, string? searchData, bool isArchive, UkraineOblasts oblast = UkraineOblasts.NotSpecified)
        {
            searchData = searchData?.ToLower();

            IQueryable<City> found = EPlastDBContext.Set<City>()
                .Where(s => string.IsNullOrWhiteSpace(searchData) || s.Name.ToLower().Contains(searchData))
                .Where(s => oblast == UkraineOblasts.NotSpecified || s.Oblast == oblast)
                .Where(c => c.IsActive != isArchive);

            IEnumerable<CityObject> result = await found
                .Select(c => new CityObject()
                {
                    ID = c.ID,
                    Name = c.Name,
                    Logo = c.Logo,
                    Count = found.Count()
                })
                .OrderBy(c => c.Name)
                .Skip(pageSize * (pageNum - 1))
                .Take(pageSize)
                .ToListAsync();

            return new Tuple<IEnumerable<CityObject>, int>(result, result.FirstOrDefault()?.Count ?? 0);
        }
    }
}
