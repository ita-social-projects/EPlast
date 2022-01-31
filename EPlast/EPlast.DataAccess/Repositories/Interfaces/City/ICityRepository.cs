using EPlast.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.DataAccess.Repositories.Contracts
{
    public interface ICityRepository : IRepositoryBase<City>
    {
        Task<Tuple<IEnumerable<CityObject>, int>> GetCitiesObjects(int pageNum, int pageSize, string searchData, bool isArchive);
    }
}
