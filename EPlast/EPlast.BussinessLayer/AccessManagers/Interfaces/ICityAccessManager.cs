using System.Collections.Generic;
using EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.AccessManagers.Interfaces
{
    public interface ICityAccessManager
    {
        IEnumerable<City> GetCities(string userId);
        bool HasAccess(string userId, int cityId);
    }
}