using System;
using EPlast.BLL.Services.Club.ClubAccess.ClubAccessGetters;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DatabaseEntities = EPlast.DataAccess.Entities;


namespace EPlast.BLL.Services.City.CityAccess.CityAccessGetters
{
    public class CityAccessForClubAdminGetter: ICItyAccessGetter
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly AdminType _clubAdminType;

        public CityAccessForClubAdminGetter(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _clubAdminType = _repositoryWrapper.AdminType.GetFirstAsync(
                predicate: a => a.AdminTypeName == Roles.KurinHead).Result;
        }

        public async Task<IEnumerable<DatabaseEntities.City>> GetCities(string userId)
        {
            var clubAdministration = await _repositoryWrapper.ClubAdministration.GetFirstOrDefaultAsync(
                predicate: r => r.User.Id == userId && (r.EndDate == null || r.EndDate > DateTime.Now) && r.AdminTypeId == _clubAdminType.ID);
            return clubAdministration != null ? await _repositoryWrapper.City.GetAllAsync()
                : Enumerable.Empty<DatabaseEntities.City>();
        }

        public async Task<IEnumerable<Tuple<int, string>>> GetCitiesIdAndName(string userId)
        {
            var clubAdministration = await _repositoryWrapper.ClubAdministration.GetFirstOrDefaultAsync(
                predicate: r => r.User.Id == userId && (r.EndDate == null || r.EndDate > DateTime.Now) && r.AdminTypeId == _clubAdminType.ID);
            return clubAdministration != null ? (await _repositoryWrapper.City.GetAllAsync()).Select(c => new Tuple<int, string>(c.ID, c.Name))
                    .ToList()
                : Enumerable.Empty<Tuple<int, string>>();
        }
    }
}
