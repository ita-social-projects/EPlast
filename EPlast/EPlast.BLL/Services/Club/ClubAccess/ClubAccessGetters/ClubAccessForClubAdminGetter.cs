using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseEntities = EPlast.DataAccess.Entities;
using EPlast.Resources;

namespace EPlast.BLL.Services.Club.ClubAccess.ClubAccessGetters
{
    public class ClubAccessForClubAdminGetter: IClubAccessGetter
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly AdminType _ClubAdminType;

        public ClubAccessForClubAdminGetter(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _ClubAdminType = _repositoryWrapper.AdminType.GetFirstAsync(
                    predicate: a => a.AdminTypeName == Roles.KurinHead).Result;
        }

        public async Task<IEnumerable<DatabaseEntities.Club>> GetClubs(string userId)
        {
            var ClubAdministration = await _repositoryWrapper.ClubAdministration.GetFirstOrDefaultAsync(
                    predicate: c => c.UserId == userId && (DateTime.Now < c.EndDate || c.EndDate == null) && c.AdminTypeId == _ClubAdminType.ID);
            return ClubAdministration != null ? await _repositoryWrapper.Club.GetAllAsync(
                predicate: c => c.ID == ClubAdministration.ClubId)
                : Enumerable.Empty<DatabaseEntities.Club>();
        }

    }
}
