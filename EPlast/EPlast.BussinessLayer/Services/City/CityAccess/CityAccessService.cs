using AutoMapper;
using EPlast.BussinessLayer.DTO.City;
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
                    var cities = _cityAccessGetters[key].GetCities(user.Id);
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