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
using EPlast.Resources;
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

        public async Task<IEnumerable<CityForAdministrationDTO>> GetAllCitiesIdAndName(DatabaseEntities.User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains(Roles.Admin))
                return _mapper.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityForAdministrationDTO>>(
                    await _cityAccessGetters[Roles.Admin].GetCities(user.Id));
            if (roles.Contains(Roles.CityHead))
                return _mapper.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityForAdministrationDTO>>(
                    await _cityAccessGetters[Roles.CityHead].GetCities(user.Id));
<<<<<<< HEAD
            if (roles.Contains(Roles.CityHeadDeputy))
                return _mapper.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<DTO.AnnualReport.CityDTO>>(
                    await _cityAccessGetters[Roles.CityHeadDeputy].GetCities(user.Id));
            return Enumerable.Empty<DTO.AnnualReport.CityDTO>();
=======
            return Enumerable.Empty<CityForAdministrationDTO>();
>>>>>>> 74a8b02931785ffab442960a199022f2f35cb9fd
        }

        public async Task<bool> HasAccessAsync(DatabaseEntities.User user, int cityId)
        {
            var cities = await this.GetCitiesAsync(user);
            return cities.Any(c => c.ID == cityId);
        }

        public async Task<bool> HasAccessAsync(DatabaseEntities.User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                if (Roles.HeadsAndHeadDeputiesAndAdmin.Contains(role))
                    return true;
            }
            return false;
        }
    }
}