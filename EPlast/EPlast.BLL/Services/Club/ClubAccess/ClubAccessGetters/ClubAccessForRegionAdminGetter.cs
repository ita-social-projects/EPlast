using EPlast.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseEntities = EPlast.DataAccess.Entities;
using EPlast.DataAccess.Entities;
using EPlast.Resources;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Club.ClubAccess.ClubAccessGetters
{
    public class ClubAccessForRegionAdminGetter: IClubAccessGetter
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly AdminType _regionAdminType;

        public ClubAccessForRegionAdminGetter(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _regionAdminType = _repositoryWrapper.AdminType.GetFirstAsync(
                predicate: a => a.AdminTypeName == Roles.OkrugaHead).Result;
        }

        public async Task<IEnumerable<DatabaseEntities.Club>> GetClubs(string userId)
        {
            var regionAdministration = await _repositoryWrapper.RegionAdministration.GetFirstOrDefaultAsync(
                predicate: c => c.UserId == userId && (DateTime.Now < c.EndDate || c.EndDate == null) && c.AdminTypeId == _regionAdminType.ID);
            return regionAdministration != null ? await _repositoryWrapper.Club.GetAllAsync()
                : Enumerable.Empty<DatabaseEntities.Club>();
        }

        public async Task<IEnumerable<Tuple<int, string>>> GetClubsIdAndName(string userId)
        {
            var regionAdministration = await _repositoryWrapper.RegionAdministration.GetFirstOrDefaultAsync(
                predicate: c => c.UserId == userId && (DateTime.Now < c.EndDate || c.EndDate == null) && c.AdminTypeId == _regionAdminType.ID);
            return regionAdministration != null
                ? (await _repositoryWrapper.Club.GetAllAsync()).Select(c => new Tuple<int, string>(c.ID, c.Name))
                .ToList()
                : Enumerable.Empty<Tuple<int, string>>();

        }
    }
}
