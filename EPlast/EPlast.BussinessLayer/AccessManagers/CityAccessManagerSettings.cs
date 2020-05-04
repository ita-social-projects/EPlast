using System.Collections.Generic;
using EPlast.DataAccess.Repositories;
using EPlast.BussinessLayer.AccessManagers.Interfaces;
using EPlast.BussinessLayer.AccessManagers.CitiesGetters;

namespace EPlast.BussinessLayer.AccessManagers
{
    public class CityAccessManagerSettings : ICityAccessManagerSettings
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public CityAccessManagerSettings(IRepositoryWrapper repositoryWrapper)
        {
            _repoWrapper = repositoryWrapper;
        }

        public Dictionary<string, ICitiesGetter> GetCitiesGetters()
        {
            var citiesGetters = new Dictionary<string, ICitiesGetter>
            {
                { "Admin", new CitiesGetterForAdmin(_repoWrapper) },
                { "Голова Округу", new CitiesGetterForRegionAdmin(_repoWrapper) },
                { "Голова Станиці", new CitiesGetterForCityAdmin(_repoWrapper) }
            };
            return citiesGetters;
        }
    }
}