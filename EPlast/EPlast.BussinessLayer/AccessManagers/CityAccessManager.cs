using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.BussinessLayer.AccessManagers.Interfaces;

namespace EPlast.BussinessLayer.AccessManagers
{
    public class CityAccessManager : ICityAccessManager
    {
        private readonly IRepositoryWrapper _repowrapper;
        private readonly UserManager<User> _userManager;

        private readonly Dictionary<string, ICitiesGetter> citiesGetters;

        public CityAccessManager(IRepositoryWrapper repositoryWrapper, UserManager<User> userManager, ICityAccessManagerSettings managerSettings)
        {
            _repowrapper = repositoryWrapper;
            _userManager = userManager;
            citiesGetters = managerSettings.GetCitiesGetters();
        }

        public IEnumerable<City> GetCities(string userId)
        {
            var roles = GetRoles(userId);
            foreach (var role in roles)
            {
                if (citiesGetters.ContainsKey(role))
                {
                    var cities = citiesGetters[role].GetCities(userId);
                    if (cities != null)
                        return cities;
                }
            }
            return Enumerable.Empty<City>();
        }

        public bool HasAccess(string userId, int cityId)
        {
            var roles = GetRoles(userId);
            foreach (var role in roles)
            {
                if (citiesGetters.ContainsKey(role))
                {
                    var cities = citiesGetters[role].GetCities(userId);
                    if (cities?.Any(c => c.ID == cityId) == true)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private IEnumerable<string> GetRoles(string userId)
        {
            try
            {
                var user = _repowrapper.User
                    .FindByCondition(u => u.Id == userId)
                    .First();
                var roles = _userManager.GetRolesAsync(user);
                return roles.Result;
            }
            catch
            {
                return Enumerable.Empty<string>();
            }
        }
    }
}