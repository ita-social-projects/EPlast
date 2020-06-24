using System.Collections.Generic;
using System.Threading.Tasks;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.Bussiness.Services.City.CityAccess.CityAccessGetters
{
    public interface ICItyAccessGetter
    {
        Task<IEnumerable<DatabaseEntities.City>> GetCities(string userId);
    }
}