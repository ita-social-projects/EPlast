using EPlast.BussinessLayer.Services.City.CityAccess.CityAccessGetters;
using EPlast.BussinessLayer.Services.Interfaces;
using EPlast.BussinessLayer.Settings;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BussinessLayer.Services.City.CityAccess
{
    public class CityAccessService : ICityAccessService
    {
        private readonly UserManager<DatabaseEntities.User> _userManager;

        private readonly Dictionary<string, ICItyAccessGetter> _cityAccessGetters;

        public CityAccessService(CityAccessSettings settings, UserManager<DatabaseEntities.User> userManager)
        {
            _cityAccessGetters = settings.CitiAccessGetters;
            _userManager = userManager;
        }

        public async Task<IEnumerable<DatabaseEntities.City>> GetCities(ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipal);
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var key in _cityAccessGetters.Keys)
            {
                if (roles.Contains(key))
                {
                    return _cityAccessGetters[key].GetCities(user.Id);
                }
            }
            return Enumerable.Empty<DatabaseEntities.City>();
        }

        public async Task<bool> HasAccess(ClaimsPrincipal claimsPrincipal, int cityId)
        {
            var cities = await this.GetCities(claimsPrincipal);
            return cities.Any(c => c.ID == cityId);
        }
    }
}