using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Services.City.CityAccess.CityAccessGetters
{
    public class CityAccessForAdminGetter : ICItyAccessGetter
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public CityAccessForAdminGetter(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public IEnumerable<DatabaseEntities.City> GetCities(string userId)
        {
            return _repositoryWrapper.City.FindAll();
        }
    }
}