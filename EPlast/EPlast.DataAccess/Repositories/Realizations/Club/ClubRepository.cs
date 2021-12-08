using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPlast.DataAccess.Repositories
{
    public class ClubRepository : RepositoryBase<Club>, IClubRepository
    {
        public ClubRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<Tuple<IEnumerable<ClubObject>, int>> GetClubsByPage(int pageNum, int pageSize, string searchData, bool isArchive)
        {
            var items = EPlastDBContext.Set<Club>()
                .Where(c => (string.IsNullOrEmpty(searchData) && c.IsActive == isArchive) || (c.IsActive == isArchive && c.Name.Contains(searchData)))
                .Select(c => new ClubObject() { ID = c.ID, Name = c.Name, Logo = c.Logo})
                .OrderBy(c => c.ID)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize);
            var rowCount = EPlastDBContext.Set<Club>()
                .Where(c => (string.IsNullOrEmpty(searchData) && c.IsActive == isArchive) || (c.IsActive == isArchive && c.Name.Contains(searchData)))
                .Count();
            return new Tuple<IEnumerable<ClubObject>, int>(items, rowCount);
        }
    }
}
