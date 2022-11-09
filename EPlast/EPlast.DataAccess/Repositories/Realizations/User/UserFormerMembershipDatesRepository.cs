using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using EPlast.DataAccess.Entities.UserEntities;
using System.Linq;

namespace EPlast.DataAccess.Repositories
{
    public class UserFormerMembershipDatesRepository : RepositoryBase<UserFormerMembershipDates>, IUserFormerMembershipDatesRepository
    {
        public UserFormerMembershipDatesRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }

        public Tuple<IEnumerable<UserFormerMembershipTable>, int> GetUserTableObjects(string userId)
        {
            var items = EPlastDBContext.Set<UserFormerMembershipDates>()
                .Where(x => x.UserId == userId)
                .Select(x => new UserFormerMembershipTable()
            {
                ID = x.Id,
                Entry = x.DateEntry,
                End = x.DateEnd
            });

            var rowCount = items.Count();

            return new Tuple<IEnumerable<UserFormerMembershipTable>, int>(items, rowCount);
        }
    }
}

