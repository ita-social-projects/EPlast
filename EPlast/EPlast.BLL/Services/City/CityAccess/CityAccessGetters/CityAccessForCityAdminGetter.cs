using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseEntities = EPlast.DataAccess.Entities;
using EPlast.Resources;

namespace EPlast.BLL.Services.City.CityAccess.CityAccessGetters
{
    public class CityAccessForCityAdminGetter : ICItyAccessGetter
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly AdminType _cityAdminType;

        public CityAccessForCityAdminGetter(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _cityAdminType = _repositoryWrapper.AdminType.GetFirstAsync(
                    predicate: a => a.AdminTypeName == Roles.CityHead).Result;
        }

        public async Task<IEnumerable<DatabaseEntities.City>> GetCities(string userId)
        {
            var cityAdministration = await _repositoryWrapper.CityAdministration.GetFirstOrDefaultAsync(
                    predicate: c => c.UserId == userId && (DateTime.Now < c.EndDate || c.EndDate == null) && c.AdminTypeId == _cityAdminType.ID);
            return cityAdministration != null ? await _repositoryWrapper.City.GetAllAsync(
                predicate: c => c.ID == cityAdministration.CityId, include: source => source.Include(c => c.Region))
                : Enumerable.Empty<DatabaseEntities.City>();
        }

        public async Task<IEnumerable<Tuple<int, string>>> GetCitiesIdAndName(string userId)
        {
            var cityAdministration = await _repositoryWrapper.CityAdministration.GetFirstOrDefaultAsync(
                predicate: c => c.UserId == userId && (DateTime.Now < c.EndDate || c.EndDate == null) && c.AdminTypeId == _cityAdminType.ID);
            return cityAdministration != null ? (await _repositoryWrapper.City.GetAllAsync(
                    predicate: c => c.ID == cityAdministration.CityId)).Select(c => new Tuple<int, string>(c.ID, c.Name)).AsEnumerable()
                : Enumerable.Empty<Tuple<int, string>>();
        }
    }
}