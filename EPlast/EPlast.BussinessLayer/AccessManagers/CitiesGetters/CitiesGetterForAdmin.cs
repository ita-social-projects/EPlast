using System.Collections.Generic;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.BussinessLayer.AccessManagers.Interfaces;

namespace EPlast.BussinessLayer.AccessManagers.CitiesGetters
{
    public class CitiesGetterForAdmin : ICitiesGetter
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public CitiesGetterForAdmin(IRepositoryWrapper repositoryWrapper)
        {
            _repoWrapper = repositoryWrapper;
        }

        public IEnumerable<City> GetCities(string userId)
        {
            try
            {
                var cities = _repoWrapper.City.FindAll();
                return cities;
            }
            catch
            {
                return null;
            }
        }
    }
}