using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Services.City.CityAccess.CityAccessGetters
{
    public interface ICItyAccessGetter
    {
        Task<IEnumerable<DatabaseEntities.City>> GetCities(string userId);

        Task<IEnumerable<Tuple<int, string>>> GetCitiesIdAndName(string userId);
    }
}