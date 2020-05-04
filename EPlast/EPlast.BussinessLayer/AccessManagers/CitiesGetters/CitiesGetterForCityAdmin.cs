using System.Linq;
using System.Collections.Generic;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.BussinessLayer.AccessManagers.Interfaces;

namespace EPlast.BussinessLayer.AccessManagers.CitiesGetters
{
    public class CitiesGetterForCityAdmin : ICitiesGetter
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public CitiesGetterForCityAdmin(IRepositoryWrapper repositoryWrapper)
        {
            _repoWrapper = repositoryWrapper;
        }

        public IEnumerable<City> GetCities(string userId)
        {
            try
            {
                var cities = _repoWrapper.City
                    .FindByCondition(c => c.CityAdministration.Any(ca => ca.UserId == userId));
                return cities;
            }
            catch
            {
                return null;
            }
        }
    }
}