using AutoMapper;
using EPlast.Bussiness.DTO.City;
using EPlast.Bussiness.Services.City.CityAccess.CityAccessGetters;
using EPlast.Bussiness.Services.Interfaces;
using EPlast.Bussiness.Settings;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.Bussiness.Services.City.CityAccess
{
    public class CityAccessService : ICityAccessService
    {
        private readonly UserManager<DatabaseEntities.User> _userManager;
        private readonly IMapper _mapper;

        private readonly Dictionary<string, ICItyAccessGetter> _cityAccessGetters;

        public CityAccessService(CityAccessSettings settings, UserManager<DatabaseEntities.User> userManager, IMapper mapper)
        {
            _cityAccessGetters = settings.CitiAccessGetters;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CityDTO>> GetCitiesAsync(ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipal);
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var key in _cityAccessGetters.Keys)
            {
                if (roles.Contains(key))
                {
                    var cities = await _cityAccessGetters[key].GetCities(user.Id);
                    return _mapper.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityDTO>>(cities);
                }
            }
            return Enumerable.Empty<CityDTO>();
        }

        public async Task<bool> HasAccessAsync(ClaimsPrincipal claimsPrincipal, int cityId)
        {
            var cities = await this.GetCitiesAsync(claimsPrincipal);
            return cities.Any(c => c.ID == cityId);
        }
    }
}