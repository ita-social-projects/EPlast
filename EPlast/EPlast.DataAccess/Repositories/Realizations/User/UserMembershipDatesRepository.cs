using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;

namespace EPlast.DataAccess.Repositories
{
    public class UserMembershipDatesRepository : RepositoryBase<UserMembershipDates>, IUserMembershipDatesRepository
    {
        public UserMembershipDatesRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
