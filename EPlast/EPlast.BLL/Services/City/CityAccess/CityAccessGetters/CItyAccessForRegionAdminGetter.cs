using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseEntities = EPlast.DataAccess.Entities;
using  EPlast.Resources;

namespace EPlast.BLL.Services.City.CityAccess.CityAccessGetters
{
    public class CItyAccessForRegionAdminGetter : ICItyAccessGetter
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly AdminType _regionAdminType;
        private readonly AdminType _regionAdminDeputyType;

        public CItyAccessForRegionAdminGetter(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _regionAdminType = _repositoryWrapper.AdminType.GetFirstAsync(
                    predicate: a => a.AdminTypeName == Roles.OkrugaHead).Result;
            _regionAdminDeputyType = _repositoryWrapper.AdminType.GetFirstAsync(
                    predicate: a => a.AdminTypeName == Roles.OkrugaHeadDeputy).Result;
        }

        public async Task<IEnumerable<DatabaseEntities.City>> GetCities(string userId)
        {
            var regionAdministration = await _repositoryWrapper.RegionAdministration.GetFirstOrDefaultAsync(
                    predicate: r => r.User.Id == userId && (r.EndDate == null || r.EndDate > DateTime.Now) && (r.AdminTypeId == _regionAdminType.ID || r.AdminTypeId == _regionAdminDeputyType.ID),
                    include: source => source
                        .Include(r => r.Region));
            return regionAdministration != null ? await _repositoryWrapper.City.GetAllAsync(
                predicate: c => c.Region.ID == regionAdministration.Region.ID, include: source => source.Include(c => c.Region))
                : Enumerable.Empty<DatabaseEntities.City>();
        }

        public async Task<IEnumerable<Tuple<int, string>>> GetCitiesIdAndName(string userId)
        {
            var regionAdministration = await _repositoryWrapper.RegionAdministration.GetFirstOrDefaultAsync(
                predicate: r => r.User.Id == userId && (r.EndDate == null || r.EndDate > DateTime.Now) && (r.AdminTypeId == _regionAdminType.ID || r.AdminTypeId == _regionAdminDeputyType.ID),
                include: source => source
                    .Include(r => r.Region));
            return regionAdministration != null
                ? (await _repositoryWrapper.City.GetAllAsync(
                    predicate: c => c.Region.ID == regionAdministration.Region.ID,
                    include: source => source.Include(c => c.Region))).Select(c => new Tuple<int, string>(c.ID, c.Name))
                .ToList()
                : Enumerable.Empty<Tuple<int, string>>();
        }
    }
}