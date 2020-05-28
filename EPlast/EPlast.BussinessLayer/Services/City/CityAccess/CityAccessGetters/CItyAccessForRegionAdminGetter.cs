using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Services.City.CityAccess.CityAccessGetters
{
    public class CItyAccessForRegionAdminGetter : ICItyAccessGetter
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public CItyAccessForRegionAdminGetter(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public IEnumerable<DatabaseEntities.City> GetCities(string userId)
        {
            var regionAdministration = _repositoryWrapper.RegionAdministration
                .FindByCondition(ra => ra.User.Id == userId && ra.EndDate == null)
                .Include(ra => ra.Region)
                .FirstOrDefault();
            return regionAdministration != null ? _repositoryWrapper.City.FindByCondition(c => c.Region.ID == regionAdministration.Region.ID)
                : Enumerable.Empty<DatabaseEntities.City>();
        }
    }
}