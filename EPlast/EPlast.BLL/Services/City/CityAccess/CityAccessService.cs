﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BLL.DTO.City;
using EPlast.BLL.Interfaces.City;
using EPlast.BLL.Services.City.CityAccess.CityAccessGetters;
using EPlast.BLL.Settings;
using EPlast.DataAccess.Repositories;
using EPlast.DataAccess.Repositories.Realizations.Base;
using EPlast.Resources;
using Microsoft.AspNetCore.Identity;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Services.City.CityAccess
{
    public class CityAccessService : ICityAccessService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly UserManager<DatabaseEntities.User> _userManager;
        private readonly IMapper _mapper;

        private readonly Dictionary<string, ICItyAccessGetter> _cityAccessGetters;

        public CityAccessService(IRepositoryWrapper repositoryWrapper, CityAccessSettings settings, UserManager<DatabaseEntities.User> userManager, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _cityAccessGetters = settings.CitiAccessGetters;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CityDto>> GetCitiesAsync(DatabaseEntities.User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var key = _cityAccessGetters.Keys.FirstOrDefault(x => roles.Contains(x));
            if(key != null)
            {
                var cities = await _cityAccessGetters[key].GetCities(user.Id);
                return _mapper.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityDto>>(cities);
            }
            return Enumerable.Empty<CityDto>();
        }

        public async Task<IEnumerable<CityForAdministrationDto>> GetAllCitiesIdAndName(DatabaseEntities.User user)
        {
            IEnumerable<CityForAdministrationDto> options = Enumerable.Empty<CityForAdministrationDto>();
            var roles = await _userManager.GetRolesAsync(user);
            var citiesId =
                (await _repositoryWrapper.AnnualReports.GetAllAsync(predicate: x => x.Date.Year == DateTime.Now.Year))
                .Select(x => x.CityId).ToList();
            if (roles.Contains(Roles.Admin))
                options = _mapper.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityForAdministrationDto>>(
                    await _cityAccessGetters[Roles.Admin].GetCities(user.Id));
            else if (roles.Contains(Roles.GoverningBodyAdmin))
                options = _mapper.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityForAdministrationDto>>(
                    await _cityAccessGetters[Roles.GoverningBodyAdmin].GetCities(user.Id));
            else if ((roles.Contains(Roles.OkrugaHead) || roles.Contains(Roles.OkrugaHeadDeputy)) && (roles.Contains(Roles.CityHead) || roles.Contains(Roles.CityHeadDeputy)))
                options = _mapper.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityForAdministrationDto>>(
                    await _cityAccessGetters[Roles.OkrugaHead].GetCities(user.Id));
            else if (roles.Contains(Roles.OkrugaHead) || roles.Contains(Roles.OkrugaHeadDeputy))
                options = _mapper.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityForAdministrationDto>>(
                    await _cityAccessGetters[Roles.OkrugaHead].GetCities(user.Id));
            else if (roles.Contains(Roles.CityHead) || roles.Contains(Roles.CityHeadDeputy))
                options = _mapper.Map<IEnumerable<DatabaseEntities.City>, IEnumerable<CityForAdministrationDto>>(
                    await _cityAccessGetters[Roles.CityHead].GetCities(user.Id));
            foreach (var item in options)
            {
                item.HasReport = citiesId.Any(x => x == item.ID);
            }

            return options;
        }

        public async Task<bool> HasAccessAsync(DatabaseEntities.User user, int cityId)
        {
            var cities = await this.GetCitiesAsync(user);
            return cities.Any(c => c.ID == cityId);
        }

        public async Task<bool> HasAccessAsync(DatabaseEntities.User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault(x => Roles.HeadsAndHeadDeputiesAndAdmin.Contains(x));
            if(role != null)
            {
                return true;
            }
            return false;
        }
    }
}