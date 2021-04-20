using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseEntities = EPlast.DataAccess.Entities;
using System.Threading.Tasks;

namespace EPlast.BLL.Services.Club.ClubAccess.ClubAccessGetters
{
    public class ClubAccessForCityAdminGetter: IClubAccessGetter
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly AdminType _cityAdminType;

        public ClubAccessForCityAdminGetter(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _cityAdminType = _repositoryWrapper.AdminType.GetFirstAsync(
                predicate: a => a.AdminTypeName == Roles.CityHead).Result;
        }

        public async Task<IEnumerable<DatabaseEntities.Club>> GetClubs(string userId)
        {
            var cityAdministration = await _repositoryWrapper.CityAdministration.GetFirstOrDefaultAsync(
                predicate: c => c.UserId == userId && (DateTime.Now < c.EndDate || c.EndDate == null) && c.AdminTypeId == _cityAdminType.ID);
            return cityAdministration != null ? await _repositoryWrapper.Club.GetAllAsync()
                : Enumerable.Empty<DatabaseEntities.Club>();
        }

        public async Task<IEnumerable<Tuple<int, string>>> GetClubsIdAndName(string userId)
        {
            var cityAdministration = await _repositoryWrapper.CityAdministration.GetFirstOrDefaultAsync(
                predicate: c => c.UserId == userId && (DateTime.Now < c.EndDate || c.EndDate == null) && c.AdminTypeId == _cityAdminType.ID);
            return cityAdministration != null
                ? (await _repositoryWrapper.Club.GetAllAsync()).Select(c => new Tuple<int, string>(c.ID, c.Name))
                .ToList()
                : Enumerable.Empty<Tuple<int, string>>();
        }
    }
}
