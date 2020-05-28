using System.Collections.Generic;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Services.City.CityAccess.CityAccessGetters
{
    public interface ICItyAccessGetter
    {
        IEnumerable<DatabaseEntities.City> GetCities(string userId);
    }
}