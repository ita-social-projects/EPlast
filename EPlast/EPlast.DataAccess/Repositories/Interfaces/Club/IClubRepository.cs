using EPlast.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPlast.DataAccess.Repositories.Contracts
{
    public interface IClubRepository : IRepositoryBase<Club>
    {
        Task<Tuple<IEnumerable<ClubObject>, int>> GetClubsByPage(int pageNum, int pageSize, string searchData, bool isArchive);
    }
}
