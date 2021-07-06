using EPlast.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.BLL.DTO.Club;
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
    }
}
