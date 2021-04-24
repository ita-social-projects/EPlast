using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Services.City.CityAccess.CityAccessGetters;
using EPlast.BLL.Settings;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Services.City.CityAccess
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

        public async Task<IEnumerable<CityDTO>> GetCitiesAsync(DatabaseEntities.User user)
        {
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

        public async Task<IEnumerable<Tuple<int, string>>> GetAllCitiesIdAndName(DatabaseEntities.User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var key in _cityAccessGetters.Keys)
            {
                if (roles.Contains(key))
                {
                    return await _cityAccessGetters[key].GetCitiesIdAndName(user.Id);
                }
            }
            return Enumerable.Empty<Tuple<int, string>>();
        }

        public async Task<bool> HasAccessAsync(DatabaseEntities.User user, int cityId)
        {
            var cities = await this.GetCitiesAsync(user);
            return cities.Any(c => c.ID == cityId);
        }

        public async Task<bool> HasAccessAsync(DatabaseEntities.User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var key in _cityAccessGetters.Keys)
            {
                if (roles.Contains(key))
                    return true;
            }

            return false;
        }
    }
}