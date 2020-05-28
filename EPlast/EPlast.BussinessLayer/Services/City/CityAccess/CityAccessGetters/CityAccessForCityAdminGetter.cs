using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using System.Linq;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Services.City.CityAccess.CityAccessGetters
{
    public class CityAccessForCityAdminGetter : ICItyAccessGetter
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public CityAccessForCityAdminGetter(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public IEnumerable<DatabaseEntities.City> GetCities(string userId)
        {
            var cityAdministration = _repositoryWrapper.CityAdministration
                .FindByCondition(ca => ca.UserId == userId && ca.EndDate == null)
                .FirstOrDefault();
            return cityAdministration != null ? _repositoryWrapper.City.FindByCondition(c => c.ID == cityAdministration.CityId)
                : Enumerable.Empty<DatabaseEntities.City>();
        }
    }
}