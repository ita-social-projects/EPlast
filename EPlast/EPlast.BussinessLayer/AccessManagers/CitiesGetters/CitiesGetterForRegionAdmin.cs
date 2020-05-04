using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.BussinessLayer.AccessManagers.Interfaces;

namespace EPlast.BussinessLayer.AccessManagers.CitiesGetters
{
    public class CitiesGetterForRegionAdmin : ICitiesGetter
    {
        private readonly IRepositoryWrapper _repoWrapper;

        public CitiesGetterForRegionAdmin(IRepositoryWrapper repositoryWrapper)
        {
            _repoWrapper = repositoryWrapper;
        }

        public IEnumerable<City> GetCities(string userId)
        {
            try
            {
                var cities = _repoWrapper.City
                    .FindByCondition(c => c.Region.RegionAdministration
                        .Any(ra => ra.User.Id == userId && ra.EndDate == null));
                return cities;
                //var regionAdministrations = _repoWrapper.RegionAdministration
                //    .FindByCondition(ra => ra.User.Id == userId && ra.EndDate == null)
                //    .Include(ra => ra.Region);
                //var cities = new List<City>();
                //foreach (var regionAdministration in regionAdministrations)
                //{
                //    cities.AddRange(_repoWrapper.City.FindByCondition(c => c.Region.ID == regionAdministration.Region.ID));
                //}
                //return cities.Distinct();
            }
            catch
            {
                return null;
            }
        }
    }
}