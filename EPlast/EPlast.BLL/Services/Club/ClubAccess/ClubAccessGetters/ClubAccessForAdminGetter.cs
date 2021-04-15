using EPlast.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseEntities = EPlast.DataAccess.Entities;

namespace EPlast.BLL.Services.Club.ClubAccess.ClubAccessGetters
{
    public class ClubAccessForAdminGetter: IClubAccessGetter
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public ClubAccessForAdminGetter(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<IEnumerable<DatabaseEntities.Club>> GetClubs(string userId)
        {
            return await _repositoryWrapper.Club.GetAllAsync();
        }

        public async Task<IEnumerable<Tuple<int, string>>> GetClubsIdAndName(string userId)
        {
            return (await _repositoryWrapper.Club.GetAllAsync())
                .Select(c => new Tuple<int, string>(c.ID, c.Name))
                .AsEnumerable();
        }
    }
}
