using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;
using EPlast.Resources;

namespace EPlast.DataAccess.Repositories.Contracts
{
    public interface ICityRepository : IRepositoryBase<City>
    {
        Task<Tuple<IEnumerable<CityObject>, int>> GetCitiesObjects(int pageNum, int pageSize, string searchData, bool isArchive, UkraineOblasts oblasts = UkraineOblasts.NotSpecified);
    }
}
