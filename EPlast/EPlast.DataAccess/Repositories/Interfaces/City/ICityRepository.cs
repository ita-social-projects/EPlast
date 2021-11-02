using System.Collections.Generic;
using System.Threading.Tasks;
using EPlast.DataAccess.Entities;

namespace EPlast.DataAccess.Repositories.Contracts
{
    public interface ICityRepository : IRepositoryBase<City>
    {
        Task<IEnumerable<City>> GetCityById(int cityId);
    }
}
