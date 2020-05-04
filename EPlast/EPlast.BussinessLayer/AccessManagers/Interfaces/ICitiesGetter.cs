using System.Collections.Generic;
using EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.AccessManagers.Interfaces
{
    public interface ICitiesGetter
    {
        IEnumerable<City> GetCities(string userId);
    }
}